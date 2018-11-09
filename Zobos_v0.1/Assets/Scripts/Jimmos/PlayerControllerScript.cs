using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 10.0f;
    [SerializeField]
    private float sprintMultiplier = 5f;

    private Rigidbody playerRigidbody;

    void Start ()
    {
        playerRigidbody = GetComponent<Rigidbody>(); //grabbing the rigidbody component from gameObject
	}
	
	void FixedUpdate ()
    {
        Movement();
	}

    Vector2 GetInput()
    {
        return new Vector2 //making vector2 
        {
            x = Input.GetAxis("Vertical") * movementSpeed,
            y = Input.GetAxis("Horizontal") * movementSpeed
        };
    }

    public void Movement()
    {
        if (Input.GetKey(KeyCode.LeftShift)) //if we want to run faster we press shift and
        {
            Vector3 momentum = new Vector3(GetInput().y, -5f, GetInput().x);
            momentum = transform.rotation * momentum;
            playerRigidbody.velocity = momentum * sprintMultiplier; //multiplies the momentum by sprintMultiplier
        }
        else
        {
            Vector3 momentum = new Vector3(GetInput().y, -5f, GetInput().x);
            momentum = transform.rotation * momentum;
            playerRigidbody.velocity = momentum;
        }
    }
}