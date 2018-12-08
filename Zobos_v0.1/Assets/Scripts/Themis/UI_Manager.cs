using UnityEngine;
using UnityEngine.UI;
//v1
public class UI_Manager : MonoBehaviour,GameEventListener
{
    Text locationText;
    Slider healthSlider;
    Slider staminaSlider;

    Text MagCapacityText;
    Text BulletsInMagText;

    private string currentGameState;
    private int UI_Health;
    private float UI_Stamina;

    private int MagCapacity;
    private int BulletsInMag;

    private Stats_Manager statsManager;

    public void Start()
    {
        GameManager.GetGameManager().AddGameEventListener(this);

        this.statsManager = this.GetComponent<Stats_Manager>();

        if (statsManager != null)
        {
            this.currentGameState = statsManager.GetGameState();
            this.UI_Health = statsManager.GetPlayerHealth();
            this.UI_Stamina = statsManager.GetPlayerStamina();
            this.MagCapacity = statsManager.GetMagSize();
            this.BulletsInMag = statsManager.GetBulletsInMag();
            Debug.Log("UI BUGFIX" + currentGameState +" "+ UI_Stamina +" "+ UI_Health);
        }
        else
        {
            Debug.Log("ERROR: UI_m can't find stats manager.");
        }        
        this.locationText = GameObject.Find("LocationText").GetComponent<Text>();
        this.locationText.text = currentGameState;
        this.healthSlider = GameObject.Find("Health").GetComponent<Slider>();
        this.healthSlider.value = UI_Health;
        this.staminaSlider = GameObject.Find("Stamina").GetComponent<Slider>();
        this.staminaSlider.value = UI_Stamina;
        this.BulletsInMagText = GameObject.Find("BulletsInMagText").GetComponent<Text>();
        this.BulletsInMagText.text = BulletsInMag.ToString();
        this.MagCapacityText = GameObject.Find("MagText").GetComponent<Text>();
        this.MagCapacityText.text = MagCapacity.ToString();
    }
    public void UI_Update_Health(int newPlayerHealth)
    {
        if (UI_Health != newPlayerHealth)
        {
            //PLAY_UI_ANIM();
            UI_Health = newPlayerHealth;
            this.healthSlider.value = UI_Health;
        }
    }
    public void UI_Update_Stamina(float newPlayerStamina)
    {
        if (UI_Stamina != newPlayerStamina)
        {
            UI_Stamina = newPlayerStamina;
            this.staminaSlider.value = UI_Stamina;
        }
    }

    public void UI_Update_Ammo(int newMagCapacity , int newBulletsInMag)
    {
        if (MagCapacity != newMagCapacity && BulletsInMag!= newBulletsInMag)
        {
            MagCapacity = newMagCapacity;
            this.MagCapacityText.text = MagCapacity.ToString();

            BulletsInMag = newBulletsInMag;
            this.BulletsInMagText.text = BulletsInMag.ToString();
        }
    }

    public void UI_Update_BulletsInMag(int newBulletsInMag)
    {
        if (BulletsInMag!= newBulletsInMag)
        {
            if (BulletsInMag <= 10)
            {
                BulletsInMagText.color = Color.red;
            }
            else BulletsInMagText.color = Color.white;

            BulletsInMag = newBulletsInMag;
            this.BulletsInMagText.text = BulletsInMag.ToString();
            
        }
       
    }

    private void Update()
    {
        UpdateBulletsInMagUI();
    }

    public void UpdateBulletsInMagUI()
    {
        BulletsInMagText.text = statsManager.GetBulletsInMag().ToString();
    }

    public void UI_Update_Game_State(string newGameState)
    {
        if (this.currentGameState != newGameState)
        {
            this.currentGameState = newGameState;
            this.locationText.text = currentGameState;
        }
    }
    public void OnGameExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnGamePause()
    {
        throw new System.NotImplementedException();
    }

    public void OnGameStart()
    {
        throw new System.NotImplementedException();
    }

    public void OnGameUnpause()
    {
        throw new System.NotImplementedException();
    }

    public void OnLevelChange()
    {
        throw new System.NotImplementedException();
    }

    public void OnLevelOne()
    {
        throw new System.NotImplementedException();
    }

    public void OnLevelTwo()
    {
        throw new System.NotImplementedException();
    }

    public void OnNewGame()
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerDeath()
    {
        throw new System.NotImplementedException();
    }
}
