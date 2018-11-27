using UnityEngine;

public class BulletScript : MonoBehaviour
{
    //TEST SCRIPT PLEASE IGNORE//

    public float maxDist = Mathf.Infinity;
    public GameObject VFXDecal;
    public float timeForVFXToDestroy = 0.1f;
    private RaycastHit hit;
    private float spread = 0.1f;
    private Transform cameraObject;

    void Start()
    {
        cameraObject = Camera.main.transform;

    }

    void Update()
    {
        if (Physics.Raycast(cameraObject.position, (cameraObject.forward + Random.insideUnitSphere * spread).normalized, out hit, maxDist))
        {
            if (VFXDecal && hit.collider)
            {
                GameObject instantiatedTexture = Instantiate(VFXDecal, hit.point, Quaternion.LookRotation(hit.normal));
                instantiatedTexture.transform.SetParent(hit.transform, true);
                Destroy(instantiatedTexture, timeForVFXToDestroy);
            }
        }
        Destroy(gameObject);
    }
}