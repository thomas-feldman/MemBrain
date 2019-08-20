using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CardFront : MonoBehaviour {

    public static bool DO_NOT = false;
    [SerializeField]
    private int _state;
    [SerializeField]
    private int _cardValue;
    [SerializeField]
    private bool _initialized = false;

    private Sprite _cardBack;
    private Sprite _cardFace;
    
    private GameObject _manager;


    private void Start()
    {
        //start state
        _state = 1;
        //Get game manager
        _manager = GameObject.FindGameObjectWithTag("Manager");
    }

    public void SetupGraphics()
    {
        //Set graphics for each card
        _cardBack = _manager.GetComponent<GameManager>().getCardBack();
        _cardFace = _manager.GetComponent<GameManager>().getCardFace(_cardValue);
        flipCard();
    }

    public void flipCard()
    {
        //Check state of card selected
        if(_state == 0)
        {
            //if face down, flip face up
            _state = 1;

        //Else
        }else if(_state == 1)
        {
            //flip face down
            _state = 0;
        }
        //If the card is down and not paused
        if(_state == 0 && !DO_NOT)
        {
            //show the card back
            GetComponent<Image>().sprite = _cardBack;
        //else if the card is up and not paused
        }else if (_state ==1 && !DO_NOT)
        {
            //show the card front
            GetComponent<Image>().sprite = _cardFace;
        }
    }

    //Method used to get the value of each card, and set it too local variable
    public int cardValue
    {
        get { return _cardValue; }
        set { _cardValue = value; }
    }

    //Method used to get the state of each card, and set it too local variable
    public int state
    {
        get { return _state; }
        set { _state = value; }
    }

    //Used to get the value of initialised(are card initialized) and set to local variable
    public bool initialized
    {
        get { return _initialized; }
        set { _initialized = value; }
    }

    //Used to run the pause and check function in local
    public void falseCheck()
    {
        StartCoroutine(pause());
    }

    IEnumerator pause()
    {
        //Pause Game to check match
        yield return new WaitForSeconds(0.75f);
        //If No Match Found
        if(_state == 0)
        {
            //Flip back over
            GetComponent<Image>().sprite = _cardBack;
        }else if (_state == 1)
        {
            //If they are a match, leave them face up
            GetComponent<Image>().sprite = _cardFace;
        }
        DO_NOT = false;
    }

    #region Sliding Puzzle

    [SerializeField] public Vector2 _slidingPosition;

    //Used to get the value of initialised(are card initialized) and set to local variable
    public GameObject setManager_Sliding
    {
        get { return _manager; }
        set { _manager = value; }
    }

    // Run the game logic, on click in sliding
    public void RunGameLogic_Sliding()
    {
        _manager.GetComponent<GameManager_Sliding>().RunGameLogic(_slidingPosition);
    }
    #endregion

    /*
     *     SpriteRenderer sr;
    private int spriteGroup;


    // Use this for initialization
    void Start () {
        
        UpdateSprite();
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void UpdateSprite()
    {
        spriteGroup = 1;
        sr = GetComponent<SpriteRenderer>();
        Sprite cardFront = Resources.Load<Sprite>(spriteGroup.ToString());
        sr.sprite = cardFront;
    }
     * */
}
