using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ZomboMovement))]
[RequireComponent(typeof(ZomboAttack))]

public class ZomboHealth : MonoBehaviour
{
    private float zomboHealth = 100;

    private bool roidRage = false; //roidRage = berserk

    //TODO: add different types of zomboHealth/reaction

    private NavMeshAgent navAgent;
    private AI_Manager aiManager;
    private ZomboMovement zomboMov;
    private ZomboAttack zomboAtk;

	void Start ()
    {
        this.navAgent = this.GetComponent<NavMeshAgent>();
        this.zomboMov = this.GetComponent<ZomboMovement>();
        this.zomboAtk = this.GetComponent<ZomboAttack>();
        this.aiManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<AI_Manager>();

        if (aiManager == null)
        {
            Debug.Log("ERROR: Can't find AI manager. SOURCE: " + this.transform.name);
        }
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
                int i = 2;
                zomboAtk.MultiplyZomboAtkSpeed(i);
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
