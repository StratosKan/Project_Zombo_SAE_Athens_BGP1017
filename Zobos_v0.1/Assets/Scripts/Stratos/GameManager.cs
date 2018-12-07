using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//v1
public class GameManager : MonoBehaviour
{
    private List<GameEventListener> listeners = new List<GameEventListener>(); //Creates a new list of game event listeners.

    // ABORTED FOR NOW!
    // private GameStateMachine gameStateMachine = new GameStateMachine(); //Creates a new state machine for the game manager.

    private Stats_Manager statsManager;

    private static GameManager inst = null; //Called before Awake because its static.

    //private List<string> gameStateNames = new List<string> { "Parking", "Outside" };
    //private enum GameStateNames {Parking,Outside,FirstFloor,SecondFloor,ThirdFloor,FourthFloor,Roof};
    private readonly string[] gameStateNames = new string[8] {"Outside","Parking","FirstFloor","SecondFloor", "SecretLab", "ThirdFloor","FourthFloor","Roof"}; //with order of appearance
    private string currentlyActiveStateName;

    //We use a hidden score variable which feeds through events in the game like killing zombies ...
    //or looting special items to procc special events like extra content.
    //This requires a lot of thinking and resources so it's now placeholder.
    private int hiddenScore = 0;

    public void Awake()
    {
        inst = this;
    }
    public void Start () //MAYBE AWAKE TODO:SCRIPT EXEC ORDER
    {
        this.statsManager = this.GetComponent<Stats_Manager>();
        currentlyActiveStateName = statsManager.GetGameState();
        if (currentlyActiveStateName == null)
        {
            Debug.Log("ERROR: FAILED TO LOAD CURRENTLY ACTIVE STATE NAME");
        }
	}
	public void Update ()
    {
		if (hiddenScore>= 100)
        {
            Debug.Log("Hidden Score is 100. Initiating special event.");
        }
	}
    public static GameManager GetGameManager()
    {
        return inst;
    }

    protected void FireNewGameEvent()
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.OnNewGame();
        }
    }
    protected void FireOnPlayerDeathEvent()
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.OnPlayerDeath();
        }
    }
    public void AddGameEventListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveGameEventListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void AddToHiddenScore(int amount)
    {
        hiddenScore += amount;
    }
    public void ChangeGameState(string newStateName)
    {
        this.currentlyActiveStateName = newStateName;
    }
}
