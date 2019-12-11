using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // Attributes
    private Card[] deck;
    private bool[] availableCards;
    private List<InventoryItem> playerInventory;
    private List<InventoryItem> dealerInventory;
    private int playerSum;
    private int dealerSum;
    private bool stay;
    private bool calculate;
    private float targetTime = 2.0f;
    private float compareTime = 3.0f;
    public GameObject[] prefabs;
    public GameObject buttonPanel;
    public GameObject scorePanels;
    public GameObject dealerText;
    public GameObject dealButton;
    public GameObject removeButton;
    public GameObject endPanel;
    public GameObject back;
    public GameObject playingText;

    // Use this for initialization
    void Start () {
        deck = new Card[52];
        availableCards = new bool[52];

        // Initialize available card array
        ResetAvailableCards();

        // Initialize deck array
        InitializeDeck();
	}

    // Initializes the deck of cards
    void InitializeDeck()
    {
        string suit = "";
        for (int i = 1; i <= 52; i++)
        {
            int k = i % 13;
            int value = k;
            string name = "";
            Card c;
            switch (k)
            {
                case 1:
                    if (i == 1)
                        suit = "Club";
                    else if (i == 14)
                        suit = "Spade";
                    else if (i == 27)
                        suit = "Heart";
                    else
                        suit = "Diamond";

                    name = suit + "_A";
                    c = new Card(name, value);
                    deck[i-1] = c;
                    break;
                case 0:
                    name = suit + "_K";
                    c = new Card(name, 10);
                    deck[i-1] = c;
                    break;
                case 2:
                    name = suit + "_2";
                    c = new Card(name, value);
                    deck[i-1] = c;
                    break;
                case 3:
                    name = suit + "_3";
                    c = new Card(name, value);
                    deck[i-1] = c;
                    break;
                case 4:
                    name = suit + "_4";
                    c = new Card(name, value);
                    deck[i-1] = c;
                    break;
                case 5:
                    name = suit + "_5";
                    c = new Card(name, value);
                    deck[i-1] = c;
                    break;
                case 6:
                    name = suit + "_6";
                    c = new Card(name, value);
                    deck[i-1] = c;
                    break;
                case 7:
                    name = suit + "_7";
                    c = new Card(name, value);
                    deck[i-1] = c;
                    break;
                case 8:
                    name = suit + "_8";
                    c = new Card(name, value);
                    deck[i-1] = c;
                    break;
                case 9:
                    name = suit + "_9";
                    c = new Card(name, value);
                    deck[i-1] = c;
                    break;
                case 10:
                    name = suit + "_10";
                    c = new Card(name, value);
                    deck[i-1] = c;
                    break;
                case 11:
                    name = suit + "_J";
                    c = new Card(name, 10);
                    deck[i-1] = c;
                    break;
                case 12:
                    name = suit + "_Q";
                    c = new Card(name, 10);
                    deck[i-1] = c;
                    break;
            }
        }
    }

    // Starts the game 
    public void StartGame()
    {
        // Show Game UI
        buttonPanel.SetActive(true);
        scorePanels.SetActive(true);
        removeButton.SetActive(true);
        dealerText.SetActive(true);
        dealButton.SetActive(false);
        playingText.SetActive(false);
        endPanel.SetActive(false);
        // Deal Cards
        playerInventory = new List<InventoryItem>();
        dealerInventory = new List<InventoryItem>();
        playerSum = 0;
        dealerSum = 0;
        stay = false;
        calculate = false;
        targetTime = 2.0f;
        compareTime = 3.0f;
        for (int i = 0; i < 4; i++)
        {
            if (i % 2 == 0)
                DealToPlayer();
            else
                DealToDealer();
        }
        GameObject.Find("DealerScoreText").GetComponent<Text>().text = dealerInventory[0].Value.ToString();
        Instantiate(back, dealerInventory[1].Prefab.transform.position, Quaternion.identity);
    }

    // Reset the game
    public void ResetGame()
    {
        ResetAvailableCards();
        for(int i = 0; i < dealerInventory.Count; i++)
        {
            Destroy(dealerInventory[i].Prefab);
        }
        for (int i = 0; i < playerInventory.Count; i++)
        {
            Destroy(playerInventory[i].Prefab);
        }
        GameObject back = GameObject.Find("Back(Clone)");
        if (back != null)
            Destroy(back);
        StartGame();
    }

    // Deal a card to the player
    public void DealToPlayer()
    {
        bool available = false;
        // Find a random available card to deal to player
        while(available != true)
        {
            int r = Random.Range(0, 52);
            if(availableCards[r] == true)
            {
                // Use new card for display and sum
                // Deal with aces
                int cardValue;
                if (deck[r].Value == 1)
                {
                    if (playerSum + 11 > 21)
                    {
                        playerSum += 1;
                        cardValue = 1;
                    }
                    else
                    {
                        playerSum += 11;
                        cardValue = 11;
                    }
                }
                else
                {
                    playerSum += deck[r].Value;
                    cardValue = deck[r].Value;
                }
                
                GameObject.Find("PlayerScoreText").GetComponent<Text>().text = playerSum.ToString();
                availableCards[r] = false;
                available = true;
                float spacing = -1.5f;
                int cardCount = playerInventory.Count;
                // Adjust position spacing if there are many cards in the players hand
                if(cardCount >= 5)
                {
                    spacing = -0.5f;
                }
                // If there are no cards, position dealt card at starting spot
                if(cardCount == 0)
                {
                    GameObject card = Instantiate(prefabs[r], new Vector3(.79f, -2.35f, 0.0f), Quaternion.identity);
                    InventoryItem i = new InventoryItem(deck[r].Name, cardValue, card);
                    playerInventory.Add(i);
                }
                // Otherwise, factor in position spacing
                else
                {
                    Vector3 lastPosition = playerInventory[cardCount - 1].Prefab.transform.position;
                    GameObject card = Instantiate(prefabs[r], new Vector3(lastPosition.x + spacing, -2.35f, 0.0f), Quaternion.identity);
                    InventoryItem i = new InventoryItem(deck[r].Name, cardValue, card);
                    playerInventory.Add(i);
                }

                // Check if 21 has been hit
                if (playerSum == 21)
                {
                    endPanel.SetActive(true);
                    GameObject.Find("FinalText").GetComponent<Text>().text = "Player has a Black Jack!";
                }
                // Check other states
                if(playerSum > 21)
                {
                    endPanel.SetActive(true);
                    GameObject.Find("FinalText").GetComponent<Text>().text = "Player has Busted! (" + playerSum + ")";
                }
            }
        }
    }

    // Deal a card to the dealer
    public void DealToDealer()
    {
        bool available = false;
        // Find a random available card to deal to dealer
        while (available != true)
        {
            int r = Random.Range(0, 52);
            if (availableCards[r] == true)
            {
                // Use new card for display and sum
                // Deal with aces
                int cardValue;
                if (deck[r].Value == 1)
                {
                    if (dealerSum + 11 > 21)
                    {
                        dealerSum += 1;
                        cardValue = 1;
                    }
                    else
                    {
                        dealerSum += 11;
                        cardValue = 11;
                    }
                }
                else
                {
                    dealerSum += deck[r].Value;
                    cardValue = deck[r].Value;
                }

                GameObject.Find("DealerScoreText").GetComponent<Text>().text = dealerSum.ToString();
                availableCards[r] = false;
                available = true;
                float spacing = -1.5f;
                int cardCount = dealerInventory.Count;
                // Adjust position spacing if there are many cards in the dealers hand
                if (cardCount >= 5)
                {
                    spacing = -0.5f;
                }
                // If there are no cards, position dealt card at starting spot
                if (cardCount == 0)
                {
                    GameObject card = Instantiate(prefabs[r], new Vector3(.79f, 2.7f, 0.0f), Quaternion.identity);
                    InventoryItem i = new InventoryItem(deck[r].Name, cardValue, card);
                    dealerInventory.Add(i);
                }
                // Otherwise, factor in position spacing
                else
                {
                    Vector3 lastPosition = dealerInventory[cardCount - 1].Prefab.transform.position;
                    GameObject card = Instantiate(prefabs[r], new Vector3(lastPosition.x + spacing, 2.7f, 0.0f), Quaternion.identity);
                    InventoryItem i = new InventoryItem(deck[r].Name, cardValue, card);
                    dealerInventory.Add(i);
                }

                // Check if 21 has been hit
                if(dealerSum == 21)
                {
                    endPanel.SetActive(true);
                    GameObject.Find("FinalText").GetComponent<Text>().text = "Dealer has a Black Jack!";
                }
                // Check other states
                if (dealerSum > 21)
                {
                    endPanel.SetActive(true);
                    GameObject.Find("FinalText").GetComponent<Text>().text = "Dealer has Busted, Player Wins!";
                }

                // start calculating comparison once everything has been displayed
                if(dealerSum >= 17)
                {
                    calculate = true;
                }
            }
        }
    }

    // Start dealer logic after player stays
    public void Stay()
    {
        playingText.SetActive(true);
        GameObject back = GameObject.Find("Back(Clone)");
        if (back != null)
            Destroy(back);
        GameObject.Find("DealerScoreText").GetComponent<Text>().text = dealerSum.ToString();
        stay = true;
        if (GameObject.Find("ButtonPanel") != null)
            GameObject.Find("ButtonPanel").SetActive(false);
        if (GameObject.Find("RemoveButton") != null)
            GameObject.Find("RemoveButton").SetActive(false);
    }

    // Reset the available card array
    void ResetAvailableCards()
    {
        for (int i = 0; i < 52; i++)
            availableCards[i] = true;
    }

    // Reset card gameobjects
    void ResetCards()
    {
        for (int i = 0; i < dealerInventory.Count; i++)
        {
            Destroy(dealerInventory[i].Prefab);
        }
        for (int i = 0; i < playerInventory.Count; i++)
        {
            Destroy(playerInventory[i].Prefab);
        }
        GameObject back = GameObject.Find("Back(Clone)");
        if (back != null)
            Destroy(back);
    }

    // Removes the last card the player drew
    public void RemoveLast()
    {
        int index = playerInventory.Count - 1;
        playerSum -= playerInventory[index].Value;
        GameObject.Find("PlayerScoreText").GetComponent<Text>().text = playerSum.ToString();
        Destroy(playerInventory[index].Prefab);
        playerInventory.RemoveAt(playerInventory.Count - 1);

    }
	
	// Update is called once per frame
	void Update () {
        if (endPanel.activeSelf)
        {
            ResetCards();
            playingText.SetActive(false);
            removeButton.SetActive(false);
        }

        if (stay && dealerSum < 17)
        {
            targetTime -= Time.deltaTime;

            // Keep dealing to dealer until they reach 17
            if (targetTime <= 0.0f)
            {
                DealToDealer();
                targetTime = 2.0f;
            }
        }
        // Do comparison once dealer has reached 17
        if(stay && calculate && !endPanel.activeSelf)
        {
            targetTime -= Time.deltaTime;

            // calculate comparison after reaching 17
            if(targetTime <= 0.0f)
            {
                //playingText.GetComponent<Text>().text = "Calculating Hand Comparison...";
                compareTime -= Time.deltaTime;

                // Keep dealing to dealer until they reach 17
                if (compareTime <= 0.0f)
                {
                    if (playerSum > dealerSum && dealerSum < 21)
                    {
                        endPanel.SetActive(true);
                        GameObject.Find("FinalText").GetComponent<Text>().text = "Player has Won!";
                    }
                    if (playerSum < dealerSum && dealerSum < 21)
                    {
                        endPanel.SetActive(true);
                        GameObject.Find("FinalText").GetComponent<Text>().text = "Dealer has Won!";
                    }
                    if (playerSum == dealerSum)
                    {
                        endPanel.SetActive(true);
                        GameObject.Find("FinalText").GetComponent<Text>().text = "Dealer and Player Have Tied!";
                    }
                }
            }
        }
    }
}
