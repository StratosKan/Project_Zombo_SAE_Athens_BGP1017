using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookScript : MonoBehaviour
{
    [Header("Sensitivity for camera")]
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    [Header("Clamp values for camera")]
    public float minimumX = -60F;
    public float maximumX = 60F;

    public float lookSmoothness = 0.1f;

    private float rotationY = 0f;
    private float rotationX = 0f;
    private float currentXRotation;
    private float currentYRotation;

    private float xRotationVelocity = 0f;
    private float yRotationVelocity = 0f;

    private InputManager input;

    void Awake()
    {
        input = GameObject.FindGameObjectWithTag("Manager").GetComponent<InputManager>();

        Cursor.lockState = CursorLockMode.Locked; //Locks cursor in game window 
        Cursor.visible = false;

        //Sets initial rotation to the one we have at inspector
        rotationY = transform.rotation.eulerAngles.y;
        rotationX = transform.rotation.eulerAngles.x;
        currentYRotation = transform.rotation.eulerAngles.y;
        currentXRotation = transform.rotation.eulerAngles.x;

    }

    void Update()
    {
        rotationY += input.MouseX * sensitivityY;
        rotationX -= input.MouseY * sensitivityX;
        rotationX = Mathf.Clamp(rotationX, minimumX, maximumX); // Clamps the rotation on the X axis
        currentXRotation = Mathf.SmoothDamp(currentXRotation, rotationX, ref xRotationVelocity, lookSmoothness); // Interpolates 
        currentYRotation = Mathf.SmoothDamp(currentYRotation, rotationY, ref yRotationVelocity, lookSmoothness); //  SAME
        transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);
    }
}
