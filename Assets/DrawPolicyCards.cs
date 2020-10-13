using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawPolicyCards : MonoBehaviour
{
    public GameObject FascistCard;
    public GameObject LiberalCard;
    public GameObject DrawCardsArea;
    public GameObject DrawCardsButton;
    public GameObject DiscardText;

    public static List<GameObject> deck = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        DiscardText.SetActive(false);
        deckManager();
    }

    public void OnClick()
    {
        if (deck.Count < 2)
        {
            deckManager();
            Debug.Log("DECKMANAGER");
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject policyCard = Instantiate(deck[0], new Vector3(0, 0, 0), Quaternion.identity);
            policyCard.transform.SetParent(DrawCardsArea.transform, false);
            deck.RemoveAt(0);
        }
        DrawCardsButton.SetActive(false);
        DiscardText.SetActive(true);
    }

   


    //instantiates and randomizes deck
    public void deckManager()
    {
        //adds cards to deck
        for (int i = 0; i < 6; i++)
            deck.Add(LiberalCard);
        for (int i = 0; i < 11; i++)
            deck.Add(FascistCard);

        //shuffles cards in deck
        var shuffledcards = deck.OrderBy(a => Guid.NewGuid()).ToList();
        deck = shuffledcards;
    }

    

}

//Make a button for drawing cards
//Make transparent image for grid
//Make gridLayout
//ms farzhan daddy