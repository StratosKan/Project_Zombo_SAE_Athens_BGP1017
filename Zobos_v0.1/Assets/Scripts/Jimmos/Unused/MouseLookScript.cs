using UnityEngine;

public class MouseLookScript : MonoBehaviour
{

    //Add this as a component to your main camera
    //Simple rotation no smoothing, no acceleration

    [Header("Sensitivity for camera")]
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    [Header("Clamp values for camera")]
    public float minimumY = -60F;
    public float maximumY = 60F;
    private float rotationY = 0F;
    private float rotationX = 0f;


    void LateUpdate()
    {
        ToRotateCamera();
    }


    public void ToRotateCamera() 
    {
        //This method makes the parent rotate with the camera as to always the parent look the correct position

        Cursor.lockState = CursorLockMode.Locked; //Locks cursor in game window 
        Cursor.visible = false;

        rotationX = Input.GetAxis("Mouse X") * sensitivityX;
        transform.parent.parent.Rotate(0, rotationX, 0);

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY); //clamping the fov of the player as to not become dizzy
        transform.localRotation = Quaternion.Euler(-rotationY, 0, 0);
       
    }
}