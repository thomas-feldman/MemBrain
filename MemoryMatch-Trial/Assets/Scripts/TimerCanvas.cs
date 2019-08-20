using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCanvas : MonoBehaviour {

    #region TimerCanvas Instancing
    // The public instance to access the Timer
    public static TimerCanvas instance = null;

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

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
    #endregion
}
