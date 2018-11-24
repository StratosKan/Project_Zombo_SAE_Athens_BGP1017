using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ZomboMovement))]

public class ZomboHealth : MonoBehaviour
{
    private float zomboHealth = 300;

    private bool roidRage = false; //roidRage = berserk

    //TODO: add different types of zomboHealth/reaction

    private NavMeshAgent navAgent;
    private AI_Manager aiManager;
    private ZomboMovement zomboMov;


	void Start ()
    {
        this.navAgent = this.GetComponent<NavMeshAgent>();
        this.zomboMov = this.GetComponent<ZomboMovement>();
        this.aiManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<AI_Manager>();
	}

    public void ApplyDamage (float amount,int bodyPart) //TODO: add gunType (1 for AR, 2 for pistol and so on).
    {                                                    //FUTURE TODO: add player ID for multiplayer.
        if (!roidRage) //first hit awakes the zombie
        {
            zomboHealth -= amount * bodyPart; //2x Headshot damage 1x Normal damage

            zomboMov.OnAware();
            roidRage = true;

            if (zomboHealth <= 0)
            {
                zomboHealth = 0;
                Die(bodyPart);
            }            
        }
        else
        {
            zomboHealth -= amount * bodyPart;//2x Headshot damage 1x Normal damage

            zomboMov.OnAware();

            if (zomboHealth <= 0)
            {
                zomboHealth = 0;
                Die(bodyPart);
            }
            else if (zomboHealth <= 50) //ROID RAGE BOIS
            {
                navAgent.acceleration = 10.0f;  //TODO: TESTS 
                navAgent.speed = 4.0f;
                //TODO: Attack speed ++
            }
        }
        //if (bodyPart == 2)
        //{
        //    UI_Manager.Headshot();
        //}
    }

    private void Die(int bodyPart)
    {
        aiManager.ZomboDeath(bodyPart);
        Destroy(gameObject);
    }

    //private void OnDestroy()
    //{
    //    //aimanager -1
    //}
    //private void HealthRegen()
    //{
    //    MAYBE ?
    //}
}
