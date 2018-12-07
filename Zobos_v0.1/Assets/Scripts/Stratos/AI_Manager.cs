using UnityEngine;

[RequireComponent(typeof(Stats_Manager))]
public class AI_Manager : MonoBehaviour
{
    //v1
    //private string currentState = "LEVEL1";
    //private int MAX_ALLOWED_ZOMBOS = 10;

    public GameObject zomboPrefab;
    public Transform dadOFZombos;
    public int howManyZombosToSpawn;

    private int zombosAlive = 0;
    private int zombosKilledInSession = 0;
    private int zombosKilledWithHeadshot = 0;

    private Transform spawnPointsParkingDad;
    private int spawnChildrenCount; 
    public Vector3[] spawnPoints; 
    private float spawnPoint_Y = 0.5f; //default height of spawning zombos
    private int randomSpawnPoint;

    public float zomboRespawnTimer;
    private float defaultZomboRespawnTimer;

    private Stats_Manager stats_manager; //private test

    void Start ()
    {
        //TODO: Get stats_manager stats.

        this.stats_manager = this.GetComponent<Stats_Manager>();

        spawnPointsParkingDad = GameObject.Find("ZomboSpawnPointsParking").transform; //Finding papa

        spawnChildrenCount = spawnPointsParkingDad.childCount;                        //Count the children

        spawnPoints = new Vector3[spawnPointsParkingDad.childCount];

        for (int i = 0; i < spawnChildrenCount; i++)                                //Add each children's position to the V3[].  
        {            
            spawnPoints[i] = new Vector3(
                spawnPointsParkingDad.GetChild(i).position.x,
                spawnPoint_Y,
                spawnPointsParkingDad.GetChild(i).position.z
                );                          
        }

        Debug.Log("Found " + spawnChildrenCount + " possible spawn points");

        if (howManyZombosToSpawn > 0)
        {
            for (int i = 0; i <= howManyZombosToSpawn; i++)
            {
                RandomZomboSpawn();
            }
        }
        defaultZomboRespawnTimer = zomboRespawnTimer;
    }

    void Update ()
    {
        zomboRespawnTimer -= Time.deltaTime;
        if(zomboRespawnTimer <= 0)
        {
            RandomZomboSpawn();
            zomboRespawnTimer = defaultZomboRespawnTimer;
        }

	}

    public void RandomZomboSpawn()
    {
        randomSpawnPoint = Random.Range(0, spawnChildrenCount - 1); //its inclusive MIN/MAX
        ZomboSpawn(spawnPoints[randomSpawnPoint]);
        //TODO: spawnPointCooldown;
    }

    public void ZomboSpawn (Vector3 spawnPoint) //maybe use Transform here but no need for now.
    {
        Instantiate(zomboPrefab,spawnPoint,Quaternion.identity,dadOFZombos);

        zombosAlive++;
    } 

    public void ZomboDeath(int bodyPart) //1 for anything , 2 for headshot
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

        StatsUpdate();
    }

    public void StatsUpdate()
    {
        stats_manager.AI_Update(this.zombosAlive, this.zombosKilledInSession, this.zombosKilledWithHeadshot);
    }
}
