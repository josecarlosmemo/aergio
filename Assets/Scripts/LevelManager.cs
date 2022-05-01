using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;


    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image progressBar;

    private float progress;
    void  Awake() {

        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else{
            Destroy(gameObject);
        }
        
    }


    public async void LoadScene(string sceneName){
        Debug.Log("Loading " + sceneName);
        progressBar.fillAmount = 0;
        progress = 0;
        var scene = SceneManager.LoadSceneAsync(sceneName);

        scene.allowSceneActivation = false;

        loadingScreen.SetActive(true);


        do
        {
            await Task.Delay(100); //artifical wait time
            progress = scene.progress;
        
            
        } while (scene.progress  < 0.9f);
        
        await Task.Delay(1000); //artifical wait time


        scene.allowSceneActivation = true;

        loadingScreen.SetActive(false);


    }

  public async void RestartScene(){

        progressBar.fillAmount = 0;
        progress = 0;
        var scene = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        scene.allowSceneActivation = false;

        loadingScreen.SetActive(true);


        do
        {
            await Task.Delay(100); //artifical wait time
            progress = scene.progress;
        
            
        } while (scene.progress  < 0.9f);
        
        await Task.Delay(1000); //artifical wait time


        scene.allowSceneActivation = true;

        loadingScreen.SetActive(false);

    }





    void Update() {
        progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, progress,3*Time.deltaTime);
        
    }

    
}
