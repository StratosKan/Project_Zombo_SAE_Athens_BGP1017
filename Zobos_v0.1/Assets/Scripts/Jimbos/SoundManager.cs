using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource ShootSFX;
    private AudioSource HitSFX;
    public GameObject hitSFXPrefab;
    public float hitSFXToDestroyTime = 1f;
    private RaycastHit hit;


    public AudioSource Music; //this will remain blank for a while

    public static SoundManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            

        }
        


        //DontDestroyOnLoad(gameObject); // Thats to secure that it's not gonna be destoyed at everytime we reload the scene


    }

    public void PlayShoot (AudioClip clip)
    {
        ShootSFX = GameObject.FindGameObjectWithTag("Gun").GetComponent<AudioSource>();
        ShootSFX.clip = clip;
        ShootSFX.PlayOneShot(clip);


    }

    public void PlayHit (AudioClip clip)
    {
        HitSFX = GameObject.FindGameObjectWithTag("Gun").GetComponent<AudioSource>();
        //GameObject instantiatedAudioSource = Instantiate(hitSFXPrefab, hit.point, Quaternion.identity);
       // instantiatedAudioSource.transform.SetParent(hit.transform, true);
        HitSFX.clip = clip;
        HitSFX.PlayOneShot(clip, 3f);
        // Destroy(instantiatedAudioSource, hitSFXToDestroyTime);



    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
