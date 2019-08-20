using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_Sliding : MonoBehaviour {

    public Sprite[] cardFace;
    public GameObject[] rawCards;
    private GameObject[,] cards;

    /// <summary>
    /// The number of card spaces total
    /// </summary>
    public int NumberOfCards = 16;
    private int SqrtNumberOfCards;

    private void Update()
    {
        if (checkSuccess) { LoadNextLevel(); }
    }

    // When this gamemanager loads
    private void Start()
    {
        if (TransitionManager.instance.isDarkening) { TransitionManager.instance.ToggleDarkening(); }
        
        // Unpause the timer
        Timer.instance.TogglePause();

        SqrtNumberOfCards = (int)Mathf.Sqrt(NumberOfCards);

        cards = new GameObject[SqrtNumberOfCards, SqrtNumberOfCards];

        // For each row and column of cards, assign it the corresponding card
        for (int column = 0; column < SqrtNumberOfCards; column++)
        {
            for (int row = 0; row < SqrtNumberOfCards; row++)
            {
                cards[column, row] = rawCards[column * 4 + row];
            }
        }

        List<int> usedSpritesIndices = new List<int>();

        // For each column of cards
        for (int column = 0; column < SqrtNumberOfCards; column++)
        {
            // For each row of cards
            for (int row = 0; row < SqrtNumberOfCards; row++)
            {
                // disable the card image
                cards[column, row].GetComponent<Image>().enabled = false;

                // if it's not the bottom right card
                if (column != SqrtNumberOfCards - 1 || row != SqrtNumberOfCards - 1)
                {
                    bool check = false;
                    int randomIndex = 0;

                    // Generate a random integer, and check it hasn't already been used
                    while (!check)
                    {
                        // Generate an int between 0 and 14 (including 0 and 14)
                        randomIndex = Random.Range(0, NumberOfCards - 1);

                        // If it's not in the list of used ints
                        if (!usedSpritesIndices.Contains(randomIndex))
                        {
                            // Add it to the list of used indices, and break the while loop
                            usedSpritesIndices.Add(randomIndex);
                            check = true;
                        }
                    }

                    // set the card at (column, row)'s sprite
                    cards[column, row].GetComponent<Image>().sprite = cardFace[randomIndex];
                    cards[column, row].GetComponent<CardFront>().cardValue = randomIndex;
                    cards[column, row].GetComponent<Image>().enabled = true;
                    cards[column, row].GetComponent<CardFront>()._slidingPosition = new Vector2(column, row );
                }
                else
                {
                    // If it's the last card in the set, hide the image. Set the current blank card to this card, for game logic.
                    cards[column, row].GetComponent<Image>().enabled = false;
                    currentBlankCard = new Vector2( SqrtNumberOfCards - 1, SqrtNumberOfCards - 1 );
                    cards[column, row].GetComponent<CardFront>()._slidingPosition = currentBlankCard;
                }
            }
        }
    }

    private bool checkSuccess;
    private float transitionTimer = 0.0f;
    private float transitionDelay = 1.0f;

    // Loads the next level
    private void LoadNextLevel()
    {
        // After the time delay, 
        if (Time.time >= (transitionTimer + transitionDelay)) { SceneManager.LoadScene("Leaderboard"); }

    }

    private Vector2 currentBlankCard;

    // Runs sliding puzzle game logic
    public void RunGameLogic(Vector2 clickedPosition)
    {
        // If the clicked card is horizontally or vertically adjacent to the blank card
        if ((clickedPosition.y == currentBlankCard.y - 1 && clickedPosition.x == currentBlankCard.x) ||
            (clickedPosition.y == currentBlankCard.y + 1 && clickedPosition.x == currentBlankCard.x) ||
            (clickedPosition.x == currentBlankCard.x - 1 && clickedPosition.y == currentBlankCard.y) ||
            (clickedPosition.x == currentBlankCard.x + 1 && clickedPosition.y == currentBlankCard.y))
        {
            // Set the currentblankcard's sprite to the clicked card's sprite, pass it the value of the clicked card, then make that card visible
            cards[(int)currentBlankCard.x, (int)currentBlankCard.y].GetComponent<Image>().sprite = cards[(int)clickedPosition.x, (int)clickedPosition.y].GetComponent<Image>().sprite;
            cards[(int)currentBlankCard.x, (int)currentBlankCard.y].GetComponent<CardFront>().cardValue = cards[(int)clickedPosition.x, (int)clickedPosition.y].GetComponent<CardFront>().cardValue;
            cards[(int)currentBlankCard.x, (int)currentBlankCard.y].GetComponent<Image>().enabled = true;

            // Hide the card at clickedposition, and set the currentBlankCard's position to that card's position 
            cards[(int)clickedPosition.x, (int)clickedPosition.y].GetComponent<Image>().enabled = false;
            currentBlankCard = clickedPosition;
        }

        // Check if the correct solution is in place
        int correctPlaces = 0;
        // For each card other than the last card, if it's cardvalue is equal to it's position in the rawCards set, increment correct places
        for (int card = 0; card < NumberOfCards; card++)
        {
            // Not the last card
            if (!(card == NumberOfCards - 1))
            {
                // If it's value equals it's position
                if (rawCards[card].GetComponent<CardFront>().cardValue == card)
                {
                    correctPlaces++;
                }
            }
        }

        // If the puzzle is solved
        if (correctPlaces == NumberOfCards - 1 || isCheating)
        {
            checkSuccess = true;
            transitionTimer = Time.time;
            Timer.instance.StopAndScore();
            TransitionManager.instance.ToggleDarkening();
        }
    }

    public bool isCheating;

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

}
