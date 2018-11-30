using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {


    public void NewGame()
    {
        SceneManager.LoadScene("PrototypeF01-Themis");
    }

    public void ToMainMenu()
    {
        
        SceneManager.LoadScene("MainMenu");
        
        
    }

    public void Quit()
    {
        Application.Quit();
    }

}
