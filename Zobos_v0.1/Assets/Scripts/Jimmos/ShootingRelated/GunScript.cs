using UnityEngine;

public class GunScript : MonoBehaviour
{
    // In this script I try to have each Gun have each own properties, !(if not merged with shooting - merge ShootingScript)!
    // Input, Player and MainCamera are referenced by tags!

    private float targetXRotation;
    private float targetYRotation;
    private float targetXRotationVelocity; // Ref setup for SmoothDamp
    private float targetYRotationVelocity; // Ref setup for SmoothDamp


    [Header("Player Properties !TESTING!")]
    public bool useMainCamera;
    [Header("Leave empty if above is enabled")]
    public Transform shootingVector;
    [Header("Gun Properties")]
    public float gunHeight = -0.35f; // Moves the gun accordingly, 0 is full horizontal on our screen 
    public float gunSide = 0.5f; // -0.5 moves the gun to be left handed, 0.5 moves the gun to be right handed, 0 is DOOM style
    public float gunDepth = 0f; // Moves the gun accordingly, < 0 moves it towards the player, > 0 moves it away
    public float gunDamage;
    public bool isGunSemiAuto = false;

    [Header("Aiming Properties")]
    public float rotateLatency = 0.05f; // How long to take the current rotation of our gun and rotate it to point the center, bigger is slower
    public float hipToAimLatency = 0.1f; // How long to take the current position of our gun and place it to point the center (ADS), bigger is slower
    public float aimSensitivityRatio = 0.4f; // This talks to the CameraLookScript which can decrease the sensitivity, thereby giving some weight to the gun
    private float ratioHipHoldVelocity; // Ref setup for SmoothDamp
    private float ratioHipHold = 1f; // Multiplier that goes from 0 to 1 in a Damp function
    public float zoomAngle = 30f; // Sets the FOV, default is 60, less than 60 will zoom in
    [Range(0f, 1f)]
    public float zoomLatency = 0.2f; // How long to take the current Field of View of our camera and change it so we zoom in/ out, bigger is slower

    [Header("Shooting Properties")]
    public float rateOfFirePerSecond = 1f;
    private float nextTimeToFire;
    private bool shouldFire = false;
    public float spreadAimed = 1f;
    public float spreadNotAimed = 1f;
    public float recoilAmount = 0.1f;
    public float recoilRecoverTime = 0.2f;
    private float currentRecoilZPos;
    private float currentRecoilZPosV;

    [Header("VFX Properties")]
    public GameObject hitVFX;
    public ParticleSystem tracerEffect;
    public float hitVFXToDestroyTime = 0.1f;

    [Header("SFX Properties")]
    public AudioClip fireSFX;
    private AudioSource fireSFXsource; //  Jimmo i put these on comments in case you or someone else need them, the whole SFX section is gonna be through
    public AudioClip hitSFX;           //   the sound manager....hopefully.
    public GameObject hitSFXPrefab;
    private AudioSource hitsSFXsource;
    public float hitSFXToDestroyTime = 1f;

    private InputManager input;
    private Transform playerObject;
    private GameObject cameraObject;
    private RaycastHit hit;
    private InteractiveTargetScript target = null;

    #region Experimental Options this is handled by GunDrawerScript
    [SerializeField]
    [HideInInspector]
    private bool enableExperimentalOptions;

    [SerializeField]
    [HideInInspector]
    private float cameraCenterRotationSpeed = 500f;

    [SerializeField]
    [HideInInspector]
    private float pushbackGunPosition = 0.6f;

    [SerializeField]
    [HideInInspector]
    private float distanceBeforePushback = 1.5f;

    private float defaultGunPosition;
    private float cameraCenterRotationSpeedVelocity;
    #endregion

    void Start()
    {
        if (enableExperimentalOptions)
        {
            defaultGunPosition = gunDepth;
        }

        //fireSFXObject = GetComponent<AudioSource>();
        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>();
        playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        fireSFXsource = GameObject.FindGameObjectWithTag("Gun").GetComponent<AudioSource>();
        cameraObject = Camera.main.gameObject;
        cameraObject.GetComponent<CameraLookScript>().currentTargetCameraAngle = zoomAngle;
        cameraObject.GetComponent<CameraLookScript>().zoomLatency = zoomLatency;

        targetYRotation = playerObject.transform.rotation.eulerAngles.y;

        if (useMainCamera)
        {
            shootingVector = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        Shoot();

        GunPositionalData();

        if (enableExperimentalOptions)
        {
            ExperimentalGunMovement();
        }
    }

    //Primary Methods
    private void Shoot()
    {
        shouldFire = false;
        if (isGunSemiAuto)
        {
            if (input.MouseFireDown)
            {
                shouldFire = !shouldFire;
            }
        }
        else
        {
            if (input.MouseFireHold && Time.time >= nextTimeToFire)
            {
                shouldFire = !shouldFire;
                nextTimeToFire = Time.time + 1f / rateOfFirePerSecond;
            }
        }

        if (shouldFire)
        {
            tracerEffect.Play();
            targetXRotation += (Random.value - 0.5f) * Mathf.Lerp(spreadAimed, spreadNotAimed, ratioHipHold * .3f); // Controls spread up/down
            targetYRotation += (Random.value - 0.5f) * Mathf.Lerp(spreadAimed, spreadNotAimed, ratioHipHold * .3f); // Controls spread left/right
            currentRecoilZPos -= recoilAmount;
            if (Physics.Raycast(shootingVector.position, shootingVector.forward, out hit, Mathf.Infinity, 9)) // Layer 9 is player layer
            {
                // Handles Bullet Spread
                shootingVector.rotation = CalculateBulletSpread(spreadAimed);
                // Handles Damage
                Damage(hit);
                // Handles VFX
                CreateVFX(hit);
            }
            // Handles SFX
            SoundManager.instance.PlayShoot(fireSFX, fireSFXsource);
            if (hit.collider)
           {
               GameObject instantiatedAudioSource = Instantiate(hitSFXPrefab, hit.point, Quaternion.identity);
               instantiatedAudioSource.transform.SetParent(hit.transform, true);
               hitsSFXsource = instantiatedAudioSource.GetComponent<AudioSource>();
                
                SoundManager.instance.PlayHit(hitSFX, hitsSFXsource);
                Destroy(instantiatedAudioSource, hitSFXToDestroyTime);
               

            }
            
            //CreateSFX();
        }
    }

    private void GunPositionalData()
    {
        if (input.MouseAimHold)
        {
            cameraObject.GetComponent<CameraLookScript>().currentAimRatio = aimSensitivityRatio;
            ratioHipHold = Mathf.SmoothDamp(ratioHipHold, 0, ref ratioHipHoldVelocity, hipToAimLatency);
        }
        else
        {
            cameraObject.GetComponent<CameraLookScript>().currentAimRatio = 1;
            ratioHipHold = Mathf.SmoothDamp(ratioHipHold, 1, ref ratioHipHoldVelocity, hipToAimLatency);
        }

        currentRecoilZPos = Mathf.SmoothDamp(currentRecoilZPos, 0, ref currentRecoilZPosV, recoilRecoverTime);

        transform.position =
        (
            cameraObject.transform.position +
            (
            Quaternion.Euler(0, targetYRotation, 0) * new Vector3(gunSide * ratioHipHold, gunHeight * ratioHipHold, gunDepth * ratioHipHold) +
            Quaternion.Euler(targetXRotation, targetYRotation, 0) * new Vector3(0, 0, currentRecoilZPos)
            )
        );

        cameraObject.transform.rotation *= Quaternion.Euler(cameraObject.transform.rotation.x - currentRecoilZPos * 10f, 0, 0); // CAMERA RECOIL

        targetXRotation = Mathf.SmoothDamp(targetXRotation, cameraObject.GetComponent<CameraLookScript>().rotationX, ref targetXRotationVelocity, rotateLatency);
        targetYRotation = Mathf.SmoothDamp(targetYRotation, cameraObject.GetComponent<CameraLookScript>().rotationY, ref targetYRotationVelocity, rotateLatency);

        transform.rotation = Quaternion.Euler(targetXRotation, targetYRotation, 0);
    }

    private void ExperimentalGunMovement()
    {
        // This part of code in hopefully a placeholder, it tries to make the gun aim at the center of the screen,
        // when there is an object and said object is atleast 3units away from the player
        RaycastHit centerObjectInfo;

        if (Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out centerObjectInfo, Mathf.Infinity, 9)) // Layer 9 is player layer
        {
            if (centerObjectInfo.transform) // If it detects an object
            {
                Quaternion targetRotation = Quaternion.LookRotation(centerObjectInfo.point - transform.position);

                // Rotate towards the target point.
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, cameraCenterRotationSpeed * Time.deltaTime);

                if (centerObjectInfo.distance < distanceBeforePushback)
                {
                    gunDepth = Mathf.SmoothDamp(gunDepth, pushbackGunPosition, ref cameraCenterRotationSpeedVelocity, 0.05f);
                }
                else
                {
                    gunDepth = Mathf.SmoothDamp(gunDepth, defaultGunPosition, ref cameraCenterRotationSpeedVelocity, 0.1f);
                }
            }
        }
    }

    //Secondary Methods
    private Quaternion CalculateBulletSpread(float spread)
    {
        Quaternion fireRotation = Quaternion.LookRotation(transform.forward);
        Quaternion randomRotation = Random.rotation;
        float currentSpread = Mathf.Lerp(spread, spreadNotAimed, ratioHipHold);
        fireRotation = Quaternion.RotateTowards(fireRotation, randomRotation, Random.Range(.0f, currentSpread));
        return fireRotation;
    }

    private void Damage(RaycastHit hit)
    {
        target = hit.transform.GetComponent<InteractiveTargetScript>();
        if (target != null)
        {
            target.TakeDamage(gunDamage);
        }
    }

    private void CreateVFX(RaycastHit hit)
    {
        if (hit.collider)
        {
            GameObject instantiatedTexture = Instantiate(hitVFX, hit.point, Quaternion.LookRotation(hit.normal));
            instantiatedTexture.transform.SetParent(hit.transform, true);
            Destroy(instantiatedTexture, hitVFXToDestroyTime);
        }
    }
    //THIS PART IS FUNCTIONING THROUGH THE SOUND MANAGER 
   /* private void CreateSFX()
    {
        if (fireSFX != null)
        {
            fireSFXObject.PlayOneShot(fireSFX);
        }
        if (hitSFX != null)
        {
            if (hit.collider)
            {
                GameObject instantiatedAudioSource = Instantiate(hitSFXPrefab, hit.point, Quaternion.identity);
                instantiatedAudioSource.transform.SetParent(hit.transform, true);
                AudioSource audioSrc;
                audioSrc = instantiatedAudioSource.GetComponent<AudioSource>();
                audioSrc.PlayOneShot(hitSFX);
                Destroy(instantiatedAudioSource, hitSFXToDestroyTime);
            }
        }
    } */
}
