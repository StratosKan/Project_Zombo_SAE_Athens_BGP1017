using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTargetScript : MonoBehaviour
{
    //Add this script to an object you want to take damage

    private float health = 100f;

    //v2
    private string enemyTag = "Enemy";
    private ZomboHealth zomboHealth;
    private int bodyPart; // 1 for body, 2 for head


    //TEMP
    private Renderer tempMatHealth;
    private Color32 color;
    //TEMP

    void Awake()
    {
        //TEMP
        ProtoHealth();

        if (this.CompareTag(enemyTag) && this.transform.name == "Head")
        {
            this.zomboHealth = this.transform.parent.GetComponent<ZomboHealth>(); 
            this.bodyPart = 2;
        }
        else if (this.CompareTag(enemyTag))  //checking if attached gameObject is a Zombo and then ref.
        {
            this.zomboHealth = this.GetComponent<ZomboHealth>();
            this.bodyPart = 1;
        }
    }

    private void ProtoHealth()
    {
        tempMatHealth = GetComponent<Renderer>();
        color = new Color32((byte)(health), 0, 0, 255);
        tempMatHealth.material.color = color;
    }

    public void TakeDamage (float amount) 
    {
        if (this.CompareTag(enemyTag))
        {
            zomboHealth.ApplyDamage(amount,bodyPart);
        }
        else
        {
            health -= amount;

            //TEMP
            color = new Color32((byte)(health), 0, 0, 255);
            tempMatHealth.material.color = color;
            //TEMP

            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}