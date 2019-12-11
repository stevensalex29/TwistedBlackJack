using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour {

    // Attributes
    public GameManager gm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Start the game
    public void StartGame()
    {
        gm.GetComponent<GameManager>().StartGame();
    }

    // Hit for the player
    public void HitMe()
    {
        gm.GetComponent<GameManager>().DealToPlayer();
    }

    // Player stays
    public void Stand()
    {
        gm.GetComponent<GameManager>().Stay();
    }

    // Play Game
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Rules
    public void Rules()
    {
        SceneManager.LoadScene("Rules");
    }

    // Main Menu
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // Exit Game
    public void ExitGame()
    {
        Application.Quit();
    }

    // Removes the last card the player drew
    public void RemoveLast()
    {
        gm.GetComponent<GameManager>().RemoveLast();
        GameObject.Find("RemoveButton").SetActive(false);
    }

    // Play Again
    public void PlayAgain()
    {
        gm.GetComponent<GameManager>().ResetGame();
    }
}
