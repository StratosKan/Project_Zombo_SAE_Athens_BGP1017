using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    //Add this script to gun/s gameobject
    [Header("Player Properties !TESTING")]
    public bool useMainCamera;
    [Space(-10f)]
    [Header("Leave empty if above is enabled")]
    public Transform playerCameraVector;
    [Header("Gun Properties")]
    public float gunDamage;
    public bool isGunSemiAuto = false;
    public float rateOfFirePerSecond;
    private float nextTimeToFire;
    private bool shouldFire = false;
    [Header("VFX Properties")]
    public GameObject hitVFX;
    public ParticleSystem tracerEffect;
    public float timeForVFXToDestroy = .1f;
    private RaycastHit hit;
    private InteractiveTargetScript target = null;


    void Awake()
    {
        if (useMainCamera)
        {
            playerCameraVector = Camera.main.transform;
        }
    }

    void Update()
    {
        Shoot_Raycast_Camera();
    }

    void Shoot_Raycast_Camera()
    {
        shouldFire = false;
        if (isGunSemiAuto)
        {
            if (Input.GetMouseButtonDown(0))
            {
                shouldFire = !shouldFire;
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
            {
                shouldFire = !shouldFire;
                nextTimeToFire = Time.time + 1f / rateOfFirePerSecond;
            }
        }
        if (shouldFire)
        {
            tracerEffect.Play();
            if (Physics.Raycast(playerCameraVector.transform.position, playerCameraVector.transform.forward, out hit, Mathf.Infinity))
            {
                //Handles Damage
                Damage(hit);
                //Handles VFX
                CreateVFX(hit);
            }
        }

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
            Destroy(instantiatedTexture, timeForVFXToDestroy);
        }
    }
}