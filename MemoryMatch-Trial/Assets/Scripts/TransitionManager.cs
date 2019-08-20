using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    #region TransitionManager Instancing
    // The public instance to access the information cart
    public static TransitionManager instance = null;

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
    
    // Use this for initialization
	void Start () {
        // Reference the image component
        Image = GetComponent<Image>();

        Image.color = new Color(0, 0, 0, 0);

        // reenable the image layer
        Image.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        DoTransition();
	}

    private Image Image;
    public bool isDarkening { get; private set; }
    [SerializeField] private float alpha = 0.0f;

    // Fade to black
    private void DoTransition()
    {
        if (alpha <= 1 && isDarkening)
        {
            alpha += Time.deltaTime * 4;
            Image.color = new Color(0, 0, 0, alpha);
        }
        else if (alpha > 0 && !isDarkening)
        {
            alpha -= Time.deltaTime * 4;
            Image.color = new Color(0, 0, 0, alpha);
        }
    }

    public void ToggleDarkening()
    {
        isDarkening = !isDarkening;
    }
}
