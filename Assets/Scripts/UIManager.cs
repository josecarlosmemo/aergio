using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject gameLostUI;
	public GameObject gameWonUI;
	bool gameIsOver;


	void Start () {
        Drone.OnPlayerSpotted += showGameLostUI;
        PlayerController.onReachedFinish +=showGameWonUI;
	}
	

	void Update () {
		if (gameIsOver) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				SceneManager.LoadScene (0);
			}
		}
	}

	void showGameWonUI() {
		onGameOver (gameWonUI);
	}

	void showGameLostUI() {
		onGameOver (gameLostUI);
	}

	void onGameOver(GameObject gameOverUI) {
		gameOverUI.SetActive (true);
		gameIsOver = true;
        Drone.OnPlayerSpotted-=showGameLostUI;
        PlayerController.onReachedFinish -=showGameWonUI;

	}
}
