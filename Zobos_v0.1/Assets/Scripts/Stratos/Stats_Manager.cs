using UnityEngine;

[RequireComponent(typeof(AI_Manager))]
public class Stats_Manager : MonoBehaviour
{
    //v1
    private Vector3 playerPos;
    private float playerHealth;

    private int weaponType;         //weapon currently equipped
    private int inventoryGrenadesCount; //How many? TODO: grenade types if any
    private int bulletsInMagazine;  //Magazine bullets
    private int bulletsRemaining;  //Overall bullets

    private string currentGameState; // TODO: name them.
    private string currentAIState;

    private bool tutorialEnabled; //TODO: make tutorial?
    private int tutorialProgress;

    private float updateTimer = 2.0f; // Stats is updating every 2 secs 

    private int zombosAlive;
    private int zombosKilledInSession;
    private int zombosKilledTotal; //only on stats for save file or scene load.
    private int zombosKilledWithHeadshot;

    private AI_Manager aiManager;

    private string dummyString;

    void Start ()
    {
        this.aiManager = this.GetComponent<AI_Manager>(); //Do this for other managers as well.
    }

	void Update ()
    {
        updateTimer -= Time.deltaTime;

        if (updateTimer <= 0)
        {
            aiManager.StatsUpdate(); //Stats Update is a function on all manager script that returns the most useful information.

            dummyString = "STATS UPDATE: ZombosKilledInSession: " + zombosKilledInSession + " ZombosAlive: " + zombosAlive + " ZombosHS: " + zombosKilledWithHeadshot;
            Debug.Log(dummyString);

            //Player_Update(); TODO
            //Level_Update();
            //UI_Update();

            updateTimer = 2.0f; //frequency will change on future update. Required optimization.
        }
	}

    //TODO: When making save file make sure save is loaded before this method!!!!! SOS
    public void AI_Update(                                                           //We can also make this a constructor instead of method.
        int zombosAlive,int zombosKilledInSession,int zombosKilledWithHeadshot
        )
    {
        //TODO: Maybe add previousZombosAlive to compare updates.
        this.zombosAlive = zombosAlive;
        this.zombosKilledInSession = zombosKilledInSession;
        this.zombosKilledWithHeadshot = zombosKilledWithHeadshot;
    }
}
