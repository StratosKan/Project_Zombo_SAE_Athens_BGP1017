using UnityEngine;
using UnityEngine.AI;
//v2
[RequireComponent(typeof(ZomboAttack))]
public class ZomboMovement : MonoBehaviour
{
    private Transform target; //player TODO: Aggro system on multiplayer
    private RaycastHit hit;
    private NavMeshHit navHit;
    private NavMeshAgent agent;
    private ZomboAttack zomboAtk;

    private float fov = 120f; //field of view
    private float viewDistance = 10f;
    private float wanderRadius = 7.0f;
    private Vector3 wanderPoint; //the wandering point our AI generates to wander arround and ACT like a zombo.
    
    private string playerTag = "Player";
    private bool playerInRange = false;
    private float zomboAttackRange = 1.3f; //TODO: TESTS AND GAMEPLAY
    private float zomboAttackSpeed = 0.7f;
    private float zomboAttackTimer;

    private bool isAware = false;
    private Renderer zomboRenderer; // for testing purposes
    
    void Start ()
    {
        //setting up refs
		if (target == null && GameObject.FindGameObjectWithTag(playerTag)) 
        {
            target = GameObject.FindGameObjectWithTag(playerTag).transform;  //TODO: optimization so we can get Vector3 and get the .position.
        }

        this.agent = this.GetComponent<NavMeshAgent>();
        this.wanderPoint = RandomWanderPoint();
        this.zomboRenderer = this.GetComponent<MeshRenderer>();
        this.zomboAtk = this.GetComponent<ZomboAttack>();

        this.zomboAttackTimer = zomboAttackSpeed * 3; //x3 is gameplay factor.
	}
	
	void Update ()
    {
        if (isAware)
        {
            Chase(target);
            zomboAttackTimer -= Time.deltaTime; //todo: gameplay test if it should be inside playerInRange bool.

            if (playerInRange)
            {
                if (zomboAttackTimer <= 0) //in this version attack speed is on playerMovement because update runs on zomboMovement.
                {
                    zomboAtk.Attack(target);
                    zomboAttackTimer = zomboAttackSpeed;
                }
                playerInRange = false;
            }

            //TODO: Evasive maneuvers            
            zomboRenderer.material.color = Color.yellow;
        }
        else
        {
            //this.agent.SetDestination(dummyTargetTest.position);
            SearchForPlayer();
            Wander();
            zomboRenderer.material.color = Color.blue;
        }
	}

    public void Chase(Transform target)
    {
        this.agent.SetDestination(target.position);

        float distance = (target.position - this.transform.position).magnitude;

        if (distance <= zomboAttackRange)
        {
            playerInRange = true;
        }
        // :))))))))))) For science
        //float distance = (target.position - this.transform.position).magnitude;
        //float distance2 = Vector3.Distance(this.transform.position, target.position);
        //if (distance == distance2)
        //{
        //    Debug.Log("SUCK IT");
        //}
        //else
        //{
        //    Debug.Log("magnitude " + distance);
        //    Debug.Log("v3distance " + distance2);
        //}
    }
    public void OnAware()          //This can and will also be handled by AI Manager in later version.
    {
        isAware = true;
    }

    public void SearchForPlayer()
    {
        if(Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(target.position)) < fov /2)  //Checks if player is within zombo fov...
        {
            float distance = (target.position - this.transform.position).magnitude;     // The distance between Zombo and Player.     

            if (distance < viewDistance)
            {
                if (Physics.Linecast(this.transform.position,target.position,out hit, -1))  //Checks if player is behind an object.
                {
                    if (hit.transform.CompareTag(playerTag))
                    {
                        OnAware();
                    }
                }
            }
        }
    }

    public void Wander()
    {
        if (Vector3.Distance(this.transform.position, wanderPoint) < 1.0f)
        {
            wanderPoint = RandomWanderPoint();
        }
        else
        {
            agent.SetDestination(wanderPoint);
        }
    }

    public Vector3 RandomWanderPoint() //TODO: optimization.
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position; //Creates a random point in wanderRadius

        NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, -1);                  //...then returns a hit on nav mesh (Careful with nav mesh on other floors)

        return new Vector3(navHit.position.x, this.transform.position.y, navHit.position.z); //... and finally sends back the wanderPoint vector.
    }

    //void OnNavMeshPreUpdate() // use for callbacks to encounter before nav mesh calcs.
}
