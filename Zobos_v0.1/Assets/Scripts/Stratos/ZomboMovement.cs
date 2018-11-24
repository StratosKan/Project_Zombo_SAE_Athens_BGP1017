using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//v1
public class ZomboMovement : MonoBehaviour
{
    private Transform target; //player TODO: Aggro system on multiplayer
    private RaycastHit hit;
    private NavMeshHit navHit;
    private NavMeshAgent agent;

    private float fov = 120f; //field of view
    private float viewDistance = 10f;
    private float wanderRadius = 7.0f;
    private Vector3 wanderPoint; //the wandering point our AI generates to wander arround and ACT like a zombo.
    
    private string playerTag = "Player";
    //private bool playerInRange = false;

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
	}
	
	void Update ()
    {
        if (isAware)
        {
            //TODO: Distance checker agent.stoppingDistance = 1;
            this.agent.SetDestination(target.position);
            //TODO: Chase, Attack(coroutine)
            //TODO: roadRage = navAgent.acceleration = xxx;
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
