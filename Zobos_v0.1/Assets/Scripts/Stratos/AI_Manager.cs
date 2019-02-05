using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

//v3
[RequireComponent(typeof(Stats_Manager))]

public class AI_Manager : MonoBehaviour
{
    //private string currentState = "LEVEL1";
    private int MAX_ALLOWED_ZOMBOS = 10;

    public bool onLevelChange; //Need?

    public GameObject zomboPrefab;
    public Transform dadOFZombos;

    private GameObject[] activeAgents = new GameObject[50];                  //reference to everything AI related so we can use for our needs.
    private Vector3[] activeAgentsPositions = new Vector3[50];

    private ZomboAttack[] activeAgentsAtkScripts = new ZomboAttack[50];
    private ZomboMovement[] activeAgentsMovementScripts = new ZomboMovement[50];
    private ZomboHealth[] activeAgentsHealthScripts = new ZomboHealth[50];
    private NavMeshAgent[] activeAgentsNavMesh = new NavMeshAgent[50];

    private int[] activeAgentsID = new int[50];
    private bool[] activeAgentsEnabled = new bool[50];
    //private bool[] activeAgentsAware = new bool[50]; TODODODODODODO

    //EXPERIMENTAL NOTE: AI_SO to apply modifications on spawn.

    private int zombosSpawned = 0; //This serves as a counter and an ID for zombos.
    private int zombosAlive = 0;
    private int zombosKilledInSession = 0;
    private int zombosKilledWithHeadshot = 0;

    private Transform spawnPointsParkingDad; 
    private int spawnChildrenCount; 
    public Vector3[] spawnPoints; //spawnPoint Control.    
    private int randomSpawnPoint;

    [Header("Experimental Option")]
    public bool endlessSpawn;
    public int howManyToSpawnOnStart;
    public float zomboRespawnTimer;  //TODO: MAKE THIS AN OPTION AND/OR MAKE THIS WORK IN 
    private float defaultZomboRespawnTimer;

    private Stats_Manager stats_manager; //private test

    private string playerTag = "Player";
    private Transform target;
    private float zomboAttackTimer; //TODO: DECIDE HOW MANY CLOCKS SHOULD BE RUNNING.
    private float defaultZomboAttackTimer = 0.7f;

    void Start ()
    {
        //TODO: Get stats_manager stats.

        //TODO: GET GAME STATE!

        this.stats_manager = this.GetComponent<Stats_Manager>();

        if (target == null && GameObject.FindGameObjectWithTag(playerTag))
        {
            target = GameObject.FindGameObjectWithTag(playerTag).transform;  //TODO: optimization so we can get Vector3 and get the .position.
        }

        //TODO: NEST THIS WITH GAME STATE
        //IF STATE PARKING
        spawnPointsParkingDad = GameObject.Find("ZomboSpawnPointsParking").transform; //Finding papa

        CountTheChilden(spawnPointsParkingDad);
        //END IF
        
        if(howManyToSpawnOnStart > 0)
        {
            MassiveZomboRandomSpawn(howManyToSpawnOnStart);
        }

        defaultZomboRespawnTimer = zomboRespawnTimer;

        this.zomboAttackTimer = defaultZomboAttackTimer; //TODO: Tests
    }

    void Update ()
    {
        if (zombosAlive > 0)
        {
            foreach (int ai in activeAgentsID)  //Running all agent updates through this manager.
            {
                if (activeAgentsEnabled[ai]) //Checks if it should do the updates on this AI.
                {
                    if (activeAgentsMovementScripts[ai].AwareOrNot())
                    {
                        activeAgentsMovementScripts[ai].Chase(target);

                        zomboAttackTimer -= Time.deltaTime;

                        if (activeAgentsMovementScripts[ai].IsPlayerInRange())
                        {
                            if (zomboAttackTimer <= 0)
                            {
                                activeAgentsAtkScripts[ai].Attack(target);

                                zomboAttackTimer = defaultZomboAttackTimer;
                            }

                            activeAgentsMovementScripts[ai].ResetPlayerInRange();
                        }

                        activeAgents[ai].GetComponent<MeshRenderer>().material.color = Color.yellow; //REMOVE WHEN MODEL IS ACTIVE.
                                                                                                     //color = yellow
                    }
                    else
                    {
                        activeAgentsMovementScripts[ai].SearchForPlayer();
                        activeAgentsMovementScripts[ai].Wander();

                        activeAgents[ai].GetComponent<MeshRenderer>().material.color = Color.blue;  //REMOVE WHEN MODEL IS ACTIVE.
                                                                                                    //color = blue
                    }
                }                
            }
        }
        if (endlessSpawn)
        {
            //NEST THIS
            //TODO: Something when zombos are over 50. Maybe implement a stack/queue.
            zomboRespawnTimer -= Time.deltaTime;

            if (zomboRespawnTimer <= 0)
            {
                RandomZomboSpawn();
                zomboRespawnTimer = defaultZomboRespawnTimer;
            }
        }
	}

    public void CountTheChilden(Transform spawnPointsDad) //v3 made it working arround whole project. Each level will have a spawnPointsDad and the actual spawnPoints will be his children.
    {
        spawnChildrenCount = spawnPointsDad.childCount;                        //Count the children

        spawnPoints = new Vector3[spawnPointsDad.childCount];

        for (int i = 0; i < spawnChildrenCount; i++)                                //Add each children's position to the V3[].  
        {
            spawnPoints[i] = new Vector3(
                spawnPointsDad.GetChild(i).position.x,
                spawnPointsDad.GetChild(i).position.y,
                spawnPointsDad.GetChild(i).position.z
                );
        }

        //Debug.Log("Found " + spawnChildrenCount + " possible spawn points");
    }

    public void MassiveZomboRandomSpawn(int howManyZombosToSpawn)
    {
        if (howManyZombosToSpawn > 0)
        {
            for (int i = 0; i <= howManyZombosToSpawn; i++)
            {
                RandomZomboSpawn();
            }
        }
    }

    public void RandomZomboSpawn()
    {
        randomSpawnPoint = Random.Range(0, spawnChildrenCount - 1); //its inclusive MIN/MAX
        ZomboSpawn(spawnPoints[randomSpawnPoint]);
        //TODO: spawnPointCooldown;
    }

    public void ZomboSpawnByIndex(int index)  //Use this for trigger events.
    {
        ZomboSpawn(spawnPoints[index]);
    }

    public void ZomboSpawn (Vector3 spawnPoint) //maybe use Transform here but no need for now.
    {
        if (zombosAlive < MAX_ALLOWED_ZOMBOS)
        {
            activeAgents[zombosSpawned] = Instantiate(zomboPrefab, spawnPoint, Quaternion.identity, dadOFZombos);

            AddZomboAsEdge(activeAgents[zombosSpawned]);

            zombosSpawned++;
            zombosAlive++;
        }
        else
        {
            Debug.Log("AI MANAGER: MAX ALLOWED ZOMBOS ALIVE REACHED.");
        }
    } 
    public void AddZomboAsEdge(GameObject zombo)
    {
        //applying references for optimal behaviour/control.
        activeAgentsPositions[zombosSpawned] = zombo.transform.position;
        activeAgentsAtkScripts[zombosSpawned] = zombo.GetComponent<ZomboAttack>();
        activeAgentsHealthScripts[zombosSpawned] = zombo.GetComponent<ZomboHealth>();
        activeAgentsMovementScripts[zombosSpawned] = zombo.GetComponent<ZomboMovement>();
        activeAgentsNavMesh[zombosSpawned] = zombo.GetComponent<NavMeshAgent>();

        activeAgentsID[zombosSpawned] = zombosSpawned; //TEST TEST TEST
        activeAgentsEnabled[zombosSpawned] = true; //Toggled off from ZomboDeath().        
        activeAgentsMovementScripts[zombosSpawned].ChangeMyID(zombosSpawned); //ID is on zombo as well
    }

    public void ZomboDeath(int bodyPart , int zomboID) //1 for anything , 2 for headshot
    {
        if (bodyPart == 2)
        {
            zombosAlive--;
            zombosKilledInSession++;
            zombosKilledWithHeadshot++;
        }
        else if (bodyPart == 1)
        {
            zombosAlive--;
            zombosKilledInSession++;
        }
        else
        {
            Debug.Log("ERROR: ZOMBO DEATH TYPE FAILURE");
        }

        activeAgentsEnabled[zomboID] = false; //Turns down the updates on that zombo.

        StatsUpdate();  //Stat tracking - Event-driven logic
    }

    public void StatsUpdate()
    {
        stats_manager.AI_Update(this.zombosAlive, this.zombosKilledInSession, this.zombosKilledWithHeadshot);
    }
}
