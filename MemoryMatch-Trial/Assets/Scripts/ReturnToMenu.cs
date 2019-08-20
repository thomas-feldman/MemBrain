using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour {

    private float timeDelay = 1.1f;
    private float timer;
    private int levelToLoad;

    private void Start()
    {
        if (TransitionManager.instance.isDarkening) { TransitionManager.instance.ToggleDarkening(); }
    }

    private void Update()
    {
        if (Time.time > timer && timer != 0) { DoReturn(); }
    }

    // Starts the menu transition
    public void Return()
    {
        timer = Time.time + timeDelay;
        TransitionManager.instance.ToggleDarkening();
    }

    // Actually loads the menu
    private void DoReturn()
    {
        SceneManager.LoadScene("Menu");
    }
}
