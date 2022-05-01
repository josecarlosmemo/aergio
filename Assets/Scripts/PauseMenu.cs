using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    public static bool GameIsPaused = false;
    [SerializeField]  private GameObject pauseMenuUI;

    void Awake() {
         if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else{
            Destroy(gameObject);
        }
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();

            } else{
                Pause();
                
            }
        }
        
    }

    void Resume(){
        pauseMenuUI.SetActive(false);
        // Time.timeScale = 1f;
        GameIsPaused = false;


    }

    void Pause(){
        pauseMenuUI.SetActive(true);
        // Time.timeScale = 0f;
        GameIsPaused = true;

    }


}
