using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;

    public string secondLevel;
    public string tutorialLevel;

    public string mainMenu;


    public void StartGame(){
        LevelManager.Instance.LoadScene(firstLevel);

    }

    public void StartTutorial(){
        LevelManager.Instance.LoadScene(tutorialLevel);

    }

    public void SecondLevel(){
        LevelManager.Instance.LoadScene(secondLevel);

    }

    public void RestartLevel(){
        LevelManager.Instance.RestartScene();
    }

    public void SendMainMenu(){
        LevelManager.Instance.LoadScene(mainMenu);
    }

    public void QuitGame(){
        Application.Quit();
    }

}
