using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource ShootSFX;
    private AudioSource HitSFX;
   
    private GameObject whereithits;


    public AudioSource Music; //this will remain blank for a while

    public static SoundManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            //whereithits = GameObject.Find("AudioSource_Gunhit_1(Clone)");
           

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
        
            //Debug.Log("IT HIT");
        


        
        whereithits = GameObject.Find("AudioSource_Gunhit_1(Clone)");

        HitSFX = whereithits.GetComponent<AudioSource>();



            HitSFX.clip = clip;
            HitSFX.PlayOneShot(clip);
            // Destroy(instantiatedAudioSource, hitSFXToDestroyTime);
        



    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
