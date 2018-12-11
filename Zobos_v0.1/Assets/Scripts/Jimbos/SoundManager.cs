
using UnityEngine;

public class SoundManager : MonoBehaviour //Interface usage will come soon 
{
    private AudioSource ShootSFX;
    private AudioSource HitSFX;
    public AudioClip WalkSound;
    public AudioClip JumpSound;
    
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

    public void PlayShoot (AudioClip clip, AudioSource source)
    {
        ShootSFX = source;
        ShootSFX.clip = clip;
        ShootSFX.PlayOneShot(clip);        
    }

    public void PlayHit (AudioClip clip, AudioSource source)
    {       
        HitSFX = source;
        HitSFX.clip = clip;
        HitSFX.PlayOneShot(clip);       
    }
    
    public void WalkPitchRange(AudioSource source)
    {
        source.pitch = Random.Range(0.8f, 1.1f);
    }
    public void RunningMode(AudioSource source)
    {
        source.pitch = 1.5f;
    }
    
}
