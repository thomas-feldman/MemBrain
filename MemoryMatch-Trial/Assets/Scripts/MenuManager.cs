using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public static bool standAlone;
    private float timeDelay = 1.1f;
    private float timer;
    private int levelToLoad;

    private void Start()
    {
        if (TransitionManager.instance.isDarkening) { TransitionManager.instance.ToggleDarkening(); }
    }

    private void Update()
    {
        if (Time.time > timer && timer != 0) { DoLoadLevel(levelToLoad); }
    }

    public void triggerMenuBehaviour(int i)
    {
        timer = Time.time + timeDelay;
        levelToLoad = i;
        TransitionManager.instance.ToggleDarkening();
    }

    private void DoLoadLevel(int i)
    {
        switch (i)
        {
            default:
            //If the start button was clicked, load the level
            case (0):
                SceneManager.LoadScene("GameModeSelect");
                break;
            // If the quit button was clicked, quit the game
            case (1):
                Application.Quit();
                break;
            // Show Leaderboard
            case (2):
                SceneManager.LoadScene("Leaderboard");
                break;
            //Show Instructions
            case (3):
                SceneManager.LoadScene("Instructions");
                break;
            //load stand alone memory match
            case (4):
                SceneManager.LoadScene("Level - Memory Match");
                // Pass the cart information; return to menu on completion
                InformationCartScript.instance.StoreInformation("key_MemoryMatch", new List<string> { "Leaderboard" });
                break;
            //load sliding puzzle double game
            case (5):
                SceneManager.LoadScene("Level - Memory Match");
                //Pass the cart information; go to sliding puzzle on completion
                InformationCartScript.instance.StoreInformation("key_MemoryMatch", new List<string> { "Level - Sliding Puzzle" });
                break;
            //return to menu
            case (6):
                SceneManager.LoadScene("Menu");
                break;
        }
    }
}
