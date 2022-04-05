using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
	public static  event System.Action onUIStart;
	public static event System.Action onUIFinish;
    public GameObject gameLostUI;
    public GameObject gameWonUI;

    public GameObject levelObjectiveUI;
    bool gameIsOver;

    float timeToAppear = 5f;
    float timeWhenDisappear;

    void Start()
    {
		showLevelObjectiveUI();
        Drone.OnPlayerSpotted += showGameLostUI;
        PlayerController.onReachedFinish += showGameWonUI;
    }

    void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }

        if (levelObjectiveUI.activeSelf && (Time.time >= timeWhenDisappear))
        {
            levelObjectiveUI.SetActive(false);
			onUIFinish();
        }
    }

    public void showLevelObjectiveUI()
    {
		onUIStart();
        levelObjectiveUI.SetActive(true);
        timeWhenDisappear = Time.time + timeToAppear;
    }

    void showGameWonUI()
    {
        onGameOver(gameWonUI);
    }

    void showGameLostUI()
    {
        onGameOver(gameLostUI);
    }

    void onGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);
        gameIsOver = true;
        Drone.OnPlayerSpotted -= showGameLostUI;
        PlayerController.onReachedFinish -= showGameWonUI;
    }
}
