using UnityEngine;
using UnityEngine.UI;
//v3
[RequireComponent(typeof(AI_Manager))]
public class Stats_Manager : MonoBehaviour
{
    private Vector3 playerPos;
    private int playerHealth = 100;  //This comes from save file.
    private int MAX_ALLOWED_PLAYER_HEALTH = 100; //This is default. Will be able to change in future update.
    private int UI_Health;

    private int playerArmor = 2;
    private float playerStamina = 30f;
    private float MAX_ALLOWED_PLAYER_STAMINA = 30f;
    private float UI_Stamina;

    private int stimpacksUsed; //HEALS
    //private int stimpacksOnPlayer; //INVENTORY!!!

    private int weaponType;         //weapon currently equipped
    private int inventoryGrenadesCount; //How many? TODO: grenade types if any
    private int bulletsInMagazine;  //Magazine bullets
    private int bulletsRemaining;  //Overall bullets

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

    //private string dummyString;

    //MOVE TO UI MANAGER ONCE CREATED
    Text locationText;
    Slider healthSlider;
    Slider staminaSlider;

    void Start ()
    {
        this.aiManager = this.GetComponent<AI_Manager>(); //Do this for other managers as well.
        this.UI_Health = this.playerHealth;
        this.UI_Stamina = this.playerStamina;
        //MOVE TO UI MANAGER AFTER THIS POINT ONCE CREATED
        this.locationText = GameObject.Find("LocationText").GetComponent<Text>();
        this.locationText.text = currentGameState;
        this.healthSlider = GameObject.Find("Health").GetComponent<Slider>();
        this.healthSlider.value = UI_Health;
        this.staminaSlider = GameObject.Find("Stamina").GetComponent<Slider>();
        this.staminaSlider.value = UI_Stamina;
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
    public void UI_Update_Health(int newPlayerHealth)
    {
        //UI manager TODODO
        if (UI_Health != newPlayerHealth)
        {
            //PLAY_UI_ANIM();
            UI_Health = newPlayerHealth;
            this.healthSlider.value = UI_Health;
        }
    }
    public void UI_Update_Stamina(float newPlayerStamina)
    {
        //UI manager TODODODODO
        if (UI_Stamina != newPlayerStamina)
        {
            UI_Stamina = newPlayerStamina;
            this.staminaSlider.value = UI_Stamina;
        }
    }
    public void UI_Update_Game_State(string newGameState)
    {
        if (this.currentGameState != newGameState)
        {
            this.currentGameState = newGameState;
            this.locationText.text = currentGameState;
        }
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
        UI_Update_Health(playerHealth); //HERE THEMIS-SAN HERE
    }
    public void SetPlayerStamina(float newPlayerStamina)
    {
        this.playerStamina = newPlayerStamina;
        UI_Update_Stamina(playerStamina);        
    }
    public void OneMoreOnTheHouse()
    {
        this.stimpacksUsed++;
        //PLAY_AUDIO();
        //PLAY_ANIM();
        //UPDATE_UI();
    }

    public void Player_Update()
    {

    }

    public void Player_Ammo_Update()
    {

    }
}
