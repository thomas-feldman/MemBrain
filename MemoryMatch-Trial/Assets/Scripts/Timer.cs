using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour {

    #region Timer Instancing
    // The public instance to access the Timer
    public static Timer instance = null;

    private void Awake()
    {
        // Check if a gamemanager exists
        if (instance == null)
        {
            instance = this; // If not, set it to this
        }
        else
        {
            Destroy(gameObject); // Otherwise destroy this
        }
    }
    #endregion


    public Text counterText;
    public float seconds, minutes;

    private bool isPaused;
    private int completedLevelsTime = 0;
    private int totalTime = 0;

    // Use this for initialization
    void Start() {
        counterText = GetComponent<Text>() as Text;
    }

    // Update is called once per frame
    void Update() {
        if (!isPaused && completedLevelsTime == 0)
        {
            minutes = (int)(Time.timeSinceLevelLoad / 60f);
            seconds = (int)(Time.timeSinceLevelLoad % 60f);
            counterText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        // If level one's time has been set and the timer isn't paused, show the total time instead.
        if (!isPaused && completedLevelsTime > 0)
        {
            minutes = (int)((Time.timeSinceLevelLoad + IntTimeToIntSeconds(completedLevelsTime)) / 60f);
            seconds = (int)((Time.timeSinceLevelLoad + IntTimeToIntSeconds(completedLevelsTime)) % 60f);

            totalTime = TimeToIntScore();

            counterText.text = IntToTimeString(totalTime);
        }

        if (SceneManager.GetActiveScene().name == "Level - Memory Match" || 
            SceneManager.GetActiveScene().name == "Level - Sliding Puzzle")
        {
            counterText.enabled = true;
        }
        else
        {
            counterText.enabled = false;
        }
    }

    // Toggle the pause
    public void TogglePause()
    {
        isPaused = !isPaused;

        // When the timer is paused, store the 1st level's time
        completedLevelsTime = TimeToIntScore();
    }

    // Pause the timer, convert and concatenate the minutes and seconds into an int, and score that into the DB.
    public void StopAndScore()
    {
        isPaused = true;

        GetComponent<HighScoreManager>().InsertScore("YOUR_NAME", TimeToIntScore());
    }

    // Convert the current time to a single int, and return it as int MMSS
    private int TimeToIntScore()
    {
        int time;
        if (seconds.ToString().Length == 2)
        {
            int.TryParse(minutes.ToString() + seconds.ToString(), out time);
        }
        else if (seconds.ToString().Length == 1)
        {
            int.TryParse(minutes.ToString() + "0" + seconds.ToString(), out time);
        }
        else
        {
            time = 0;
        }
        return time;
    }

    // Returns the total time in seconds
    private int IntTimeToIntSeconds(int intTime)
    {
        // Get the length of the int
        int scoreTimeLength = intTime.ToString().Length;

        // Parse the number of minutes, and convert them to seconds
        int minutes;
        int.TryParse(intTime.ToString().Remove(scoreTimeLength - 2), out minutes);
        minutes = minutes * 60;

        // Parse the seconds
        int seconds;
        int.TryParse(intTime.ToString().Substring(scoreTimeLength - 2), out seconds);

        // Return the total seconds
        return minutes + seconds;
    }

    // Take a time integer, and return it as a formatted string; MM:SS
    private string IntToTimeString(int intTime)
    {
        // Get the length of the string
        int intTimeLength = intTime.ToString().Length;

        // Insert a ':' in the centre of the score, then convert it to a string.
        return intTime.ToString().Remove(intTimeLength - 2) + ":" + intTime.ToString().Substring(intTimeLength - 2);
    }

    // Reset class variables
    public void Reset()
    {
        isPaused = false;
        minutes = 0;
        seconds = 0;
        totalTime = 0;
        completedLevelsTime = 0;

    }
}
