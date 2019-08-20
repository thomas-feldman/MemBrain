using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public Sprite[] cardFace;
    public Sprite cardBack;
    public GameObject[] cards;
    public Text matchText;

    /// <summary>
    /// The number of cards before they are copied into pairs, plus one.
    /// </summary>
    public int NumberOfCards = 13;
    private int _matches = 13;
    private const int FACEUP = 1;
    private const int FACEDOWN = 0;
    private const int MATCHED = 2;

    private bool _init = false;

    private List<string> retrievedInformation;
    private string levelToLoad;

    private void Start()
    {
        if (TransitionManager.instance.isDarkening) { TransitionManager.instance.ToggleDarkening(); }
        
        // Grab and interpret the information from the cart
        retrievedInformation = InformationCartScript.instance.RetrieveInformation("key_MemoryMatch");
        levelToLoad = retrievedInformation[0];

        // Reset the timer, just in case
        Timer.instance.Reset();
    }

    private void Update()
    {
        // Initialise the cards at the start of the game
        if (!_init)
        {
            initializeCards();
        }
        // Check if cards need to be compared or not
        if (Input.GetMouseButtonUp(0))
        {
            checkCards();
        }
        // Check if the player has matched all cards, and deal with success
        if (!checkSuccess)
        {
            CheckGameOver();
        }
        else
        {
            LoadNextLevel();
        }
    }

    void initializeCards()
    {
        // For each pair
        for (int id = 0; id < 2; id++)
        {
            // For each card face
            for (int i = 0; i < NumberOfCards; i++)
            {
                bool test = false;
                int choice = 0;
                while (!test)
                {
                    // Get a random int
                    choice = Random.Range(0, cards.Length);
                    // If that card in the set isn't initialised
                    test = !(cards[choice].GetComponent<CardFront>().initialized);
                }
                // Initialise that card with a value
                cards[choice].GetComponent<CardFront>().cardValue = i;
                cards[choice].GetComponent<CardFront>().initialized = true;
            }
        }
        foreach (GameObject c in cards)
        {
            // For each card, get it it's card face and back
            c.GetComponent<CardFront>().SetupGraphics();
        }

        // LATER check if this is for debugging
        if (!_init)
        {
            _init = true;
        }
    }

    public Sprite getCardBack()
    {
        return cardBack;
    }

    public Sprite getCardFace(int i)
    {
        return cardFace[i];
    }

    void checkCards()
    {
        List<int> c = new List<int>();
        // For each card in the list of cards TO COMPARE AGAINST EACH OTHER
        for (int i = 0; i < cards.Length; i++)
        {
            // If that card has been flipped, add it to the list
            if (cards[i].GetComponent<CardFront>().state == FACEUP)
            {
                c.Add(i);
            }
        }
        // If this is the second card selected, compare them
        if (c.Count == 2)
        {
            cardComparison(c);
        }
    }

    void cardComparison(List<int> c)
    {
        // Pause selection of cards
        CardFront.DO_NOT = true;
        int x = FACEDOWN;
        // If the value of the first card matches the second card
        if (cards[c[0]].GetComponent<CardFront>().cardValue == cards[c[1]].GetComponent<CardFront>().cardValue)
        {
            x = MATCHED;
            _matches--;
            matchText.text = "Matches Left: " + _matches;
            if(_matches == 0)
            {
                matchText.text = "Game Complete";
            }
        }
        // For each selected card, set their state to X 
        for (int i = 0; i < c.Count; i++)
        {
            cards[c[i]].GetComponent<CardFront>().state = x;

            // Check and undo the pause
            cards[c[i]].GetComponent<CardFront>().falseCheck();
        }
    }

    public void triggerMenuBehaviour(int i)
    {
        switch (i)
        {
            default:
            //If the start button was clicked, load the menu
            case (0):
                SceneManager.LoadScene("Menu");
                break;
        }
    }

    #region Level Transition
    [SerializeField] private bool checkSuccess = false;
    private float transitionTimer = 0.0f;
    private float transitionDelay = 1.1f;

    // Checks if all cards have been matched, starts a timer to load the sliding puzzle
    private void CheckGameOver()
    {
        // Check if all cards have been matched
        int matchedCards = 0;
        for (int card = 0; card < cards.Length; card++)
        {
            // For each card, if it's matched, increment the matchedCards count
            if (cards[card].GetComponent<CardFront>().state == 2) { matchedCards++; }
        }
        // If all cards have been matched, set success to true, and start the timer to transition to the next level.
        if (matchedCards == cards.Length)
        {
            checkSuccess = true;
            transitionTimer = Time.time;
            TransitionManager.instance.ToggleDarkening();

            if (levelToLoad == "Level - Sliding Puzzle")
            {
                Timer.instance.TogglePause();
            }
            else
            {
                Timer.instance.StopAndScore(); // STOP THE TIMER, STORE SCORE
            }
        }
    }

    // Loads the next level
    private void LoadNextLevel()
    {
            // After the time delay, 
            if (Time.time >= (transitionTimer + transitionDelay)) { SceneManager.LoadScene(levelToLoad); }
        
    }
    #endregion
}
