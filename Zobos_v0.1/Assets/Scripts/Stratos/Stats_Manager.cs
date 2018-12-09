using UnityEngine;
//v3
[RequireComponent(typeof(AI_Manager))]
public class Stats_Manager : MonoBehaviour
{
    private Vector3 playerPos;
    private int playerHealth = 100;  //This comes from save file.
    private int MAX_ALLOWED_PLAYER_HEALTH = 100; //This is default. Will be able to change in future update.

    private int playerArmor = 2;
    private float playerStamina = 30f;
    private float MAX_ALLOWED_PLAYER_STAMINA = 30f;

    private int stimpacksUsed; //HEALS
    //private int stimpacksOnPlayer; //INVENTORY!!!

    private int weaponType;         //weapon currently equipped
    private int inventoryGrenadesCount; //How many? TODO: grenade types if any
    private int MagazineSize=30;  //Magazine Size
    public int BulletsInMag=30;  //Bullets currently in Magazine

    private string currentGameState = "Parking"; // TODO: name them and make game state manager.
    private string currentAIState;

    private bool tutorialEnabled; //TODO: make tutorial?
    private int tutorialProgress;

    private float updateTimer = 2.0f; // Stats is updating every 2 secs 

    private int zombosAlive;
    private int zombosKilledInSession;
    private int zombosKilledTotal; //only on stats for save file or scene load.
    private int zombosKilledWithHeadshot;

    private AI_Manager aiManager;
    private UI_Manager uiManager;

    //private string dummyString;

    void Start ()
    {
        this.aiManager = this.GetComponent<AI_Manager>(); //Do this for other managers as well.
        this.uiManager = this.GetComponent<UI_Manager>();
    }

	void Update () //UPDATE CAN BE REMOVED IN FUTURE VERSION.
    {
        updateTimer -= Time.deltaTime;

        if (updateTimer <= 0)
        {
            aiManager.StatsUpdate(); //Stats Update is a function on all manager script that returns the most useful information.
            
            //dummyString = "STATS UPDATE: ZombosKilledInSession: " + zombosKilledInSession + " ZombosAlive: " + zombosAlive + " ZombosHS: " + zombosKilledWithHeadshot;
            //Debug.Log(dummyString);
            //UI_Update_Game_State(currentGameState);
            //Player_Update(); TODO
            //Level_Update();
            //UI_Update();

            updateTimer = 2.0f; //frequency will change on future update. Required optimization.
        }
	}

    //TODO: When making save file make sure save is loaded before this method!!!!! SOS
    public void AI_Update(                                                           
        int zombosAlive,int zombosKilledInSession,int zombosKilledWithHeadshot
        )
    {
        //TODO: Maybe add previousZombosAlive to compare updates.
        this.zombosAlive = zombosAlive;
        this.zombosKilledInSession = zombosKilledInSession;
        this.zombosKilledWithHeadshot = zombosKilledWithHeadshot;
    }
    public int GetPlayerHealth()
    {
        return playerHealth;
    }
    public int GetMaxPlayerHealth()
    {
        return MAX_ALLOWED_PLAYER_HEALTH;
    }
    public float GetPlayerStamina()
    {
        return playerStamina;
    }
    public float GetMaxPlayerStamina()
    {
        return MAX_ALLOWED_PLAYER_STAMINA;
    }
    public int GetPlayerArmor()
    {
        return playerArmor;
    }
    public void SetPlayerHealth(int newPlayerHealth)
    {
        this.playerHealth = newPlayerHealth;
        uiManager.UI_Update_Health(playerHealth); //HERE THEMIS-SAN HERE
    }
    public void SetPlayerStamina(float newPlayerStamina)
    {
        this.playerStamina = newPlayerStamina;
        uiManager.UI_Update_Stamina(playerStamina);        
    }

    public int GetKills()
    {
        return zombosKilledInSession;
    }

    public int GetHeadshots()
    {
        return zombosKilledWithHeadshot;
    }

    public int GetMagSize()
    {
        return MagazineSize;
    }

    public int GetBulletsInMag()
    {
        return BulletsInMag;
    }

    public void SetBulletsInMag()
    {
        uiManager.UI_Update_BulletsInMag(GetBulletsInMag());
    }

    public void OneMoreOnTheHouse()
    {
        this.stimpacksUsed++;
        //PLAY_AUDIO();
        //PLAY_ANIM();
        //UPDATE_UI();
    }
    public string GetGameState()
    {
        return currentGameState;
    }
    public void ChangeGameState(string newState)
    {
        this.currentGameState = newState;
        uiManager.UI_Update_Game_State(currentGameState);
    }

    public void Player_Update()
    {

    }

    public void Player_Ammo_Update()
    {

    }
}
