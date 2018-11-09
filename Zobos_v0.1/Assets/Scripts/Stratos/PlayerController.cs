using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    //The actual force we apply to the rigidbody
    [SerializeField]
    private float xForce = 50.0f;                  
    [SerializeField]
    private float zForce = 50.0f;
    [SerializeField]
    private float yForce = 500.0f;

    private bool isGrounded;
    private string whatIsGround = "Floor";

    private bool boosted;
    [SerializeField]
    private float boosterTimer = 15f;

	private void Start ()
    {
        this.rb = this.GetComponent<Rigidbody>();
	}
	
	private void Update ()
    {
        BoostManager();
	}

    void InputMove()
    {
        //this.rb.AddForce(force);
    }

    public void Move(Vector3 force)                                                   // Receiving forces in a Vector3 with 
    {
        if (isGrounded)                                                               // Applying force only when user is grounded...
        {
            this.rb.AddForce(force.x * xForce, force.y * yForce, force.z * zForce);   //
        }
    }
    private void BoostManager()
    {
        if (boosted)                            //If player is boosted
        {
            if (boosterTimer > 0)               //... start timer
            {
                boosterTimer -= Time.deltaTime; 
            }
            else                               //...and when it goes to 0
            {
                xForce = xForce / 2; 
                zForce = zForce / 2;
                yForce = yForce / 2;

                boosted = false;               //...revert the boost
            }
        }
    }

    public void Boost()
    {
        xForce = xForce * 2;
        zForce = zForce * 2;
        yForce = yForce * 2;

        boosted = true;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(whatIsGround))  //If the other gameObject the player hit isn't ground...
        {
            //Debug.Log("I hit floor tag");
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag(whatIsGround))  //If the other gameObject the player hit isn't ground...
        {
            //Debug.Log("Not on the ground");
            isGrounded = false;
        }
    }
}
