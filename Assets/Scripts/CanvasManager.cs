using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    public static event System.Action onUIStart;
    public static event System.Action onUIFinish;

    public static event System.Action onPauseStart;
    public static event System.Action onPauseEnd;


    [SerializeField] private GameObject gameLostUI;
    [SerializeField] private GameObject gameWonUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject levelObjectiveUI;
    bool gameIsOver;

    public float timeToAppear = 5f;
    float timeWhenDisappear;


    public static bool GameIsPaused = false;

    public PanelManager panelManager;
    public Animator mainPanel;


    void Start()
    {
        // Mostramos Level Objective
        showLevelObjectiveUI();
        Drone.OnPlayerSpotted += showGameLostUI;
        PlayerController.onReachedFinish += showGameWonUI;

    }

    void Update()
    {
        if (!gameIsOver)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();

                }
                else
                {
                    Pause();

                }

            }
            if (levelObjectiveUI.activeSelf && (Time.time >= timeWhenDisappear))
            {
                levelObjectiveUI.SetActive(false);

                if (!GameIsPaused){
                    onUIFinish();
                }
            }

        }


        if (gameIsOver)
        {
            if (levelObjectiveUI.activeSelf)
            {
                levelObjectiveUI.SetActive(false);
            }
            onPauseStart();

            // if (Input.GetKey(KeyCode.Space))
            // {
            //     LevelManager.Instance.RestartScene();

            // }



        }



    }

    public void showLevelObjectiveUI()
    {
        //? Esto hacia que se pausara el juego mientras sale el objetivo
        // if (onUIStart != null){
        //     onUIStart();
        // }

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

    public void Resume()
    {
        panelManager.CloseCurrent();

        onPauseEnd();
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        // panelManager.StopAllCoroutines();



    }

    void Pause()
    {

        if (onPauseStart != null)
        {
            onPauseStart();

        }



        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
        panelManager.OpenPanel(panelManager.initiallyOpen);


    }




    private void OnDestroy()
    {
        Drone.OnPlayerSpotted -= showGameLostUI;
        PlayerController.onReachedFinish -= showGameWonUI;
    }
}
