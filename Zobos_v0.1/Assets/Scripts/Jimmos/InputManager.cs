using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //This player input manager tries to add an extra layer between character movement and player input.
    public bool IsRunning { get; private set; }
    public bool Jump { get; private set; }

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    public float MouseX { get; private set; }
    public float MouseY { get; private set; }
    public bool MouseFireDown { get; private set; }
    public bool MouseFireHold { get; private set; }
    public bool MouseAimDown { get; private set; }


    //This guy controls all, also should probably make these an enumarator and use switch/case
    private bool PLAYING_STATE = true; //Fake state machine.
    private bool PAUSE_STATE = false;

    private void Update()
    {
        if (PLAYING_STATE)
        {
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");

            Jump = Input.GetKey(KeyCode.Space);
            IsRunning = Input.GetKey(KeyCode.LeftShift);

            MouseX = Input.GetAxis("Mouse X");
            MouseY = Input.GetAxis("Mouse Y");
            MouseFireDown = Input.GetMouseButtonDown(0);
            MouseFireHold = Input.GetMouseButton(0);
            MouseAimDown = Input.GetMouseButtonDown(1);
        }
        else if (PAUSE_STATE)
        {
            Debug.Log("Am now set at Pause Menu");
        }
    }

    private void LateUpdate()
    {
        Options();
    }

    public void Options()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            QuitGame(); // Later version will have a way to pause instead of quit.
        }
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
