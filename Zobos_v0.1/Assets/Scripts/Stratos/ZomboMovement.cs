using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZomboMovement : MonoBehaviour
{
    Transform target;

    RaycastHit hit;

    NavMeshHit navHit;

    NavMeshAgent agent;

    public float fov = 120f; //field of view

    public float viewDistance = 10f;

    public float wanderRadius = 7.0f;

    private Vector3 wanderPoint; //the wandering point our AI generates to wander arround and ACT like a zombo.
    
    private string playerTag = "Player";

    private bool playerInRange = false;

    private bool isAware = false;

    private Renderer renderer; // for testing purposes


    //private Transform dummyTargetTest; //for testing purposes

    void Start ()
    {
        //setting up refs
		if (target == null && GameObject.FindGameObjectWithTag(playerTag)) 
        {
            target = GameObject.FindGameObjectWithTag(playerTag).transform;  //For optimization we can use Vector3 and get the .position.
        }

        agent = this.GetComponent<NavMeshAgent>();

        //myTargetTest = GameObject.Find("ZomboTargetTest").transform;

        wanderPoint = RandomWanderPoint();

        renderer = this.GetComponent<MeshRenderer>();
	}
	
	void Update ()
    {
        if (isAware)
        {
            this.agent.SetDestination(target.position);
            renderer.material.color = Color.yellow;
        }
        else
        {
            //this.agent.SetDestination(dummyTargetTest.position);
            SearchForPlayer();
            Wander();
            renderer.material.color = Color.blue;
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

    public Vector3 RandomWanderPoint() //TODO: comments , optimization
    {
        Vector3 randomPoint = (Random.insideUnitSphere * wanderRadius) + transform.position;

        NavMesh.SamplePosition(randomPoint, out navHit, wanderRadius, -1);

        return new Vector3(navHit.position.x, this.transform.position.y, navHit.position.z);
    }




    //void OnNavMeshPreUpdate() // use for callbacks to encounter before nav mesh calcs.
}
