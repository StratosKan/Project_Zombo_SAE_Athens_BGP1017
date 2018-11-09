using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    //This player input manager serves this simulation as a newbish game manager and input manager
    //It can be split and improved in the future.

    private PlayerController playerController;
    private bool statePlaying = true; //Fake state machine.
    private bool statePause = false;

    private float x;
    private float y;
    private float z;

    private Vector3 defaultForce;

    private float inputPower = 1.0f;       //input power

    public bool boostPickedUp;             //This is to let booster, boost.
    private float boostDisplayTime;
    
    private float gameStartMessagesTimer = 32f;
    private bool isTutorialActive = true;

    void Start ()
    {
        //setting up refs
        this.playerController = this.GetComponent<PlayerController>();
        this.defaultForce = Vector3.zero;  //(0,0,0)
  	}
	
	// Update is called once per frame
	void Update ()
    {
        if (statePlaying)     
        {
            x = 0.0f;                   //Always resetting the inputs

            if (Input.GetKey(KeyCode.A))
            {
                x -= inputPower;
            }
            if (Input.GetKey(KeyCode.D))
            {
                x += inputPower;
            }

            z = 0.0f;

            if (Input.GetKey(KeyCode.S))
            {
                z -= inputPower;
            }
            if (Input.GetKey(KeyCode.W))
            {
                z += inputPower;
                Debug.Log("Trying to GO FORWARD");

            }

            y = 0.0f;

            if (Input.GetKey(KeyCode.Space))
            {
                y += inputPower;
                Debug.Log("Trying to Jump");
            }

            if (boostPickedUp)                       //If the boost is picked up
            {
                boostDisplayTime = 2f;                    //... set the display timer to 3 seconds so OnGui message works

                if (Input.GetKey(KeyCode.LeftShift))      
                {
                    playerController.Boost();
                    boostPickedUp = false;
                }
            }

            var forceInput = new Vector3(x, y, z);

            if (forceInput != defaultForce)         // ...checking if it's null
            {
                //Debug.Log("Sent force " + forceInput);
                playerController.Move(forceInput);  // ...and sending it to player.
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                QuitGame();
            }
        }
        if (statePause)
        {
            //Not Implemented
            //Sleep rigidbodies
            if (Input.GetKey(KeyCode.Escape))
            {
                QuitGame();
            }
        }
    }

    //public void OnGUI()
    //{
    //    if (isTutorialActive)
    //    {
    //        if (gameStartMessagesTimer >= 24)
    //        {
    //            GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, 200f, 200f), ""); 
    //            gameStartMessagesTimer -= Time.deltaTime;
    //        }
    //        else if (gameStartMessagesTimer >= 16)
    //        {
    //            GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, 200f, 200f), ""); 
    //            gameStartMessagesTimer -= Time.deltaTime;
    //        }
    //        else if (gameStartMessagesTimer >= 8)
    //        {
    //            GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, 200f, 200f), ""); 
    //            gameStartMessagesTimer -= Time.deltaTime;
    //        }
    //        else if (gameStartMessagesTimer > 0)
    //        {
    //            GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, 200f, 200f), ""); 
    //            gameStartMessagesTimer -= Time.deltaTime;
    //        }
    //        else if (gameStartMessagesTimer <= 0)
    //        {
    //            isTutorialActive = false;
    //        }
    //    }
    //    else if (boostPickedUp && boostDisplayTime >= 0)
    //    {
    //        GUI.Label(new Rect(Screen.width / 2, Screen.height / 4, 150f, 150f), ""); 

    //        boostDisplayTime -= Time.deltaTime;
    //    }
    //}

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
