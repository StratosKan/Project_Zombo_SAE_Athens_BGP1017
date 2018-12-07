using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{


    public void NewGame()
    {
        SceneManager.LoadScene("PrototypeF01-Themis Latest Merged");
    }

    public void ToMainMenu()
    {

        SceneManager.LoadScene("MainMenu");


    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

}
