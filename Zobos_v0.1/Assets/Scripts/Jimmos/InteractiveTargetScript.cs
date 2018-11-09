using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTargetScript : MonoBehaviour
{
    //Add this script to an object you want to take damage

    public float health = 20f;

    //TEMP
    private Renderer tempMatHealth;
    private Color32 color;
    //TEMP

    void Awake()
    {
        //TEMP
        ProtoHealth();
    }

    private void ProtoHealth()
    {
        tempMatHealth = GetComponent<Renderer>();
        color = new Color32((byte)(health), 0, 0, 255);
        tempMatHealth.material.color = color;
    }

    public void TakeDamage (float amount)
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

    private void Die()
    {
        Destroy(gameObject);
    }
}