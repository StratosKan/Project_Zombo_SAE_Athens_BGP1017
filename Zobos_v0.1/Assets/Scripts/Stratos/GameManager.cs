using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//v2
public class GameManager : MonoBehaviour
{
    private List<GameEventListener> listeners = new List<GameEventListener>(); //Creates a new list of game event listeners.
    private Stats_Manager statsManager;
    private static GameManager inst = null; //Called before Awake because its static. (Singleton pattern)

    //private readonly string[] gameEventNames = new string[7] {"OnNewGame","OnParking","OnCollegeArea","OnSecondFloor", "OnSecretLab", "OnSecretLabTwo","OnEndlessArcadeMode"};
    private bool[] gameEventNotUsed = new bool[7] { true, true, true, true, true, true, true };
    private string currentlyActiveStateName;
    private string currentlyActiveSceneName; //TODO: Add Scene02 when ready + use their actual names + assign on awake/start.
    private string currentlyActiveObjective;

    private int hiddenScore = 0; //testing purpose var

    public void Awake()
    {
        inst = this;
    }
    public void Start () //MAYBE AWAKE TODO:SCRIPT EXEC ORDER
    {
        this.statsManager = this.GetComponent<Stats_Manager>();
        currentlyActiveStateName = statsManager.GetGameState();
        currentlyActiveSceneName = statsManager.GetSceneName();

        if (currentlyActiveStateName == null)
        {
            Debug.Log("ERROR: FAILED TO LOAD CURRENTLY ACTIVE STATE NAME");
        }
        else
        {
            if (currentlyActiveSceneName == "Scene01")
            {
                //FireEvent("Street");
            }
            else if (currentlyActiveSceneName == "Scene02")
            {
                //FireEvent("SecretLab");
            }
        }
	}
    public static GameManager GetGameManager()
    {
        return inst;
    }

    public void ChangeGameState(string newStateName)
    {
        this.currentlyActiveStateName = newStateName;
        this.statsManager.ChangeGameState(newStateName);
        //FireEvent(newStateName);
    }
    public void ChangeObjectiveDisplay(string newObjective)
    {
        this.currentlyActiveObjective = newObjective;
        //TODO: Reference to text on screen + fade effect.
        //this.statsManager.ChangeActiveObjective(newObjective);
    }

    private void FireEvent(string eventName)
    {
        switch (eventName)
        {
            case "Street":
                {
                    if (gameEventNotUsed[0]) //Making sure each event is triggered once.
                    {
                        FireOnNewGameEvent();
                        gameEventNotUsed[0] = false;
                    }
                    break;
                }
            case "Parking":
                {
                    if (gameEventNotUsed[1])
                    {
                        FireOnParkingEvent();
                        gameEventNotUsed[1] = false;
                    }
                    break;
                }
            case "College":
                {
                    if (gameEventNotUsed[2])
                    {
                        FireOnCollegeAreaEvent();
                        gameEventNotUsed[2] = false;
                    }
                    break;
                }
            case "SecondFloor":
                {
                    if (gameEventNotUsed[3])
                    {
                        FireOnSecondFloorEvent();
                        gameEventNotUsed[3] = false;
                    }
                    break;
                }
            case "SecretLab":
                {
                    if (gameEventNotUsed[4])
                    {
                        Debug.Log("Event Not Implemented Yet: " + eventName);
                        gameEventNotUsed[4] = false;
                    }
                    break;
                }
            case "SecretLabTwo":
                {
                    if (gameEventNotUsed[5])
                    {
                        Debug.Log("Event Not Implemented Yet: " + eventName);
                        gameEventNotUsed[5] = false;
                    }
                    break;
                }
            case "EndlessArcade":
                {
                    if (gameEventNotUsed[6])
                    {
                        Debug.Log("Event Not Implemented Yet: " + eventName);
                        gameEventNotUsed[6] = false;
                    }
                    break;
                }
            case "PlayerDeath":
                {
                    FireOnPlayerDeathEvent();
                    break;
                }
        }
    }

    protected void FireOnNewGameEvent()
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.OnNewGame();
        }
    }
    protected void FireOnCollegeAreaEvent()
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.OnCollegeArea();
        }
    }
    protected void FireOnPlayerDeathEvent()
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.OnPlayerDeath();
        }
    }
    protected void FireOnParkingEvent()
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.OnParking();
        }
    }
    protected void FireOnSecondFloorEvent()
    {
        foreach (GameEventListener listener in listeners)
        {
            listener.OnSecondFloor();
        }
    }
    //TODO:SecretLabFireEvents
    //protected void FireArcadeEndlessModeEvent()
    //{
    //    foreach (GameEventListener listener in listeners)
    //    {
    //        listener.OnArcadeEndlessMode();
    //    }
    //}

    public void AddGameEventListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveGameEventListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Update()
    {
        if (hiddenScore >= 150) //+10 on zombo kill
        {
            Debug.Log("Hidden Score is 150. Initiating special event (maybe play some hidden audio clip?)");
        }
        else if (hiddenScore >= 80)
        {
            Debug.Log("Hidden Score is 80. Initiating special event (maybe play some hidden audio clip?)");
        }
    }

    public void AddToHiddenScore(int amount)
    {
        hiddenScore += amount;
    }
}
