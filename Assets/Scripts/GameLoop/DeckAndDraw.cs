using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using customLobby;
using Mirror;
using TMPro;

//Will handle deck and drawing, synced accross all clients and give authority to president and chancellor
public class DeckAndDraw : NetworkBehaviour
{
    //Total number of fascist and liberal cards in game
    int NUM_OF_LIBERALS = 6;
    int NUM_OF_FASCISTS = 11;


    //True means liberal for the SyncList, False means fascist
    [SerializeField] public SyncListBool deck = new SyncListBool();

    //Keeps track of how many cards and of what type are in hand
    [SyncVar] public int liberalsInHand = 0;
    [SyncVar] public int fascistsInHand = 0;

    //Keeps track of how many cards are on the board
    [SyncVar] int liberalsPlayed = 0;
    [SyncVar] int fascistsPlayed = 0;

    //President and chancellor index
    int presidentIndex; /////////////////////////////////////////////////////////////These are temp set to 0 and 1 for testing
    int chancellorIndex; /////////////////////////////////////////////////////////////


    //Player's index, it is set from RoomPlayer gameStarted() method
    int index = -1;
    

    [Header("Buttons")]
    [SerializeField]
    public GameObject discardPresLiberalButton;
    public GameObject discardChancLiberalButton;
    public GameObject discardPresFascistButton;
    public GameObject discardChancFascistButton;    



    // Start is called before the first frame update
    void Start()
    {
        //Turns off discard buttons
        ButtonsOff();

        //Makes sure deck is populated and shuffled
        if(isServer)
            ShuffleDeck();


        
    }

    // Update is called once per frame
    void Update()
    {
        if (index == -1) {
            //Do nothing
        }
        Debug.Log("Index: " + index);
        Debug.Log("Liberals in hand: " + liberalsInHand + " and Fascists in hand: " + fascistsInHand);
    }

    //This sets who the player is, president is, and who the chanc is, allowing the president to draw.
    public void GetPolicy(int playerIndex, int president, int chancellor) {

        
        index = playerIndex;
        presidentIndex = president;
        chancellorIndex = chancellor;
        
        if(index == president)
            PresidentDraws();


        

    }


    //Client calls and the president gets to see cards 
    public void PresidentDraws() {

        
        //Gets three cards, for everyone
        DrawThreeCard();

        //SHOW CARDS IN HAND Brett

        //Turns on needed buttons
        PresButtonsOn();

        //Debug to show the hand TODO: HOW TO SHOW HAND SHOULD BE PLACE HERE/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        Debug.Log("Liberals in hand: " + liberalsInHand + " and Fascists in hand: " + fascistsInHand);



    }

    
    //Client calls and the chancellor draws
    public void ChancellorDraws() {

        
        //If the player is the chancellor they get to see the cards and discard
        if(index == chancellorIndex) {
            ChancButtonsOn();

            //SHOW CARDS IN HAND Brett


            //Debug to show the hand TODO: HOW TO SHOW HAND SHOULD BE PLACE HERE/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Debug.Log("Liberals in hand: " + liberalsInHand + " and Fascists in hand: " + fascistsInHand);
        }
        



    }

    //Client calls and sees last card in hand, plays card to track and discards
    private void PlayLastCard() {
        
        //If the last card is a liberal
        if(liberalsInHand == 1) {
            GameObject.Find("LiberalPathFunctionHolder").GetComponent<NewLiberalPathLogic>().IncrementLiberalPath();
            DiscardLiberal();
        }
        //Otherwise it is a fascist
        else {
            GameObject.Find("FascistPathFunctionHolder").GetComponent<NewFascistPathFiveSixLogic>().CmdIncrementFascistPath();
            DiscardFascist();
        }

        //Script basically ends here/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    }



    //Client turns on their buttons if they are president and need those buttons
    private void PresButtonsOn() {
        if(liberalsInHand != 0) //There are liberal cards to discard
            discardPresLiberalButton.SetActive(true);

        if(fascistsInHand != 0) //There are fascist cards to discard
            discardPresFascistButton.SetActive(true);
    }

    //Client turns on their buttons if they are chancellor and need those buttons
    private void ChancButtonsOn() {
        if(liberalsInHand != 0) //There are liberal cards to discard
            discardChancLiberalButton.SetActive(true);

        if(fascistsInHand != 0) //There are fascist cards to discard
            discardChancFascistButton.SetActive(true);
    }


    //Client turns off buttons
    private void ButtonsOff() {
        discardPresLiberalButton.SetActive(false);
        discardChancLiberalButton.SetActive(false);
        discardPresFascistButton.SetActive(false);
        discardChancFascistButton.SetActive(false);
        //STOP SHOWING ALL CARDS Brett
    }


    //Client calls this to draw three cards
    public void DrawThreeCard() {
        DrawCard();
        DrawCard();
        DrawCard();
    }


    //Client calls this to draw a single card
    private void DrawCard() {
        CmdDraw();
    }

    //Client calls this to shuffle the deck
    private void ShuffleDeck() {
        CmdShuffleDeck();
    }

    //Client calls to discard liberal card from hand
    private void DiscardLiberal() {
        CmdDiscardLiberal();
    }


    //Client calls to discard fascist card from hand
    private void DiscardFascist() {
        CmdDiscardFascist();
    }

    /*************************Button Events******************************/
    //President is discarding a liberal card
    public void DiscardPresLiberalButton() {
            
            //Discard the liberal from hand
            DiscardLiberal();

            //Turn off buttons
            ButtonsOff();

            //Chanc turn to discard from hand, everyone needs to see if they are chanc
            RpcChancellorDraws();

        }

    //President is discarding a fascist card
    public void DiscardPresFascistButton() {
            
            //Discard the fascist from hand
            DiscardFascist();

            //Turn off buttons
            ButtonsOff();

            //Chanc turn to discard from hand, everyone needs to see if they are chanc
            RpcChancellorDraws();

        }

    //Chancellor is discarding a liberal card
    public void DiscardChancLiberalButton() {
            
            //Discard the liberal from hand
            DiscardLiberal();

            //Turn off buttons
            ButtonsOff();

            //Play the last card
            PlayLastCard();

        }

    //Chancellor is discarding a fascist card
    public void DiscardChancFascistButton() {
            
            //Discard the fascist from hand
            DiscardFascist();

            //Turn off buttons
            ButtonsOff();

            //Play the last card
            PlayLastCard();

        }



    /************************ClientRpc***********************************/
    
    //Makes everyone check if they are chancellor
    [ClientRpc]
    private void RpcChancellorDraws() {
        ChancellorDraws();
    }


    /************************COMMANDS**************************/
    
    
    //Repopulates the deck with the correct amount of cards and shuffles it
    [Command(ignoreAuthority=true)]
    private void CmdShuffleDeck() {

        //Makes sure deck is empty
        deck.Clear();

        //Adds liberal cards back to deck, not adding any in hand or any played
        for (int i = 0; i < ((NUM_OF_LIBERALS - liberalsInHand) - liberalsPlayed); i++) {
            deck.Add(true);
        }

        //Adds fascist cards back to deck, not adding any in hand or any played
        for (int i = 0; i < ((NUM_OF_FASCISTS - fascistsInHand) - fascistsPlayed); i++) {
            deck.Add(false);
        }

        //Shuffles the deck
        for(int i = 0; i < deck.Count(); i++) {
            bool temp = deck[i]; //Temp store current value
            int randomIndex = (UnityEngine.Random.Range(0, deck.Count())); //Gets random index for deck
            deck[i] = deck[randomIndex]; //Makes current index the random index
            deck[randomIndex] = temp; //And puts temp, the what used to be the current valuve in the random index
        }



    }

    //Draw and remove card from deck because it has been drawn
    [Command(ignoreAuthority=true)]
    private void CmdDraw() {

        //If the deck is empty, shuffle it so it can be drawn from otherwise get back to drawing
        if (deck.Count() == 0)
            CmdShuffleDeck();

        //If the first card is liberal add a liberal to hand, otherwise add a fascist card to hand
        if(deck[0])
            liberalsInHand++;
        else
            fascistsInHand++;

        //Remove the card drawn from deck
        deck.RemoveAt(0);
    }

    //Discard liberal card from hand
    [Command(ignoreAuthority=true)]
    private void CmdDiscardLiberal() {
        liberalsInHand--;
    }


    //Discard fascist card from hand
    [Command(ignoreAuthority=true)]
    private void CmdDiscardFascist() {
        fascistsInHand--;
    }

}
