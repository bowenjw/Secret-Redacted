using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using customLobby;
using Mirror;
/*

Desc: Script controls logic for fascist path, 5-6 players
TODO: Implement logic for gameplay changes

*/

public class NewFascistPathFiveSixLogic : NetworkBehaviour
{
    /*
    overflowPrevention keeps track of the current board state as follows
    0: Empty board
    1: One fascist card
    2: Two fascist card
    3: Three fascist card, president peeks the deck, red zone begins
    4: Four fascist card, President executes
    5: Five fascist card, President executes and veto power begins
    6: Six fascist card, facists win

    */
    private GameObject[] cards;
    private int numCards = 6;
    
    [SyncVar (hook = nameof(CheckGameState))]
    private int pathwayTracker = 0;

    //Other scripts in project
    Veto Veto;

    //Grabs cards and hides them all.
    void Start()
    {
        //Grab all cards and place into array and hides them all
        cards = new GameObject[numCards];
        for(int i = 0; i < numCards; i++)
        {
            cards[i] = GameObject.Find("Policy Card Fascist #" + (i+1));
            cards[i].GetComponent<Renderer>().enabled = false;
        }

        //Allows access to Functions/Methods from these Scripts
        Veto = GameObject.Find("VetoHolder").GetComponent<Veto>();
    }

    //Keeps current game state, includes where to implement gamestate
    void Update()
    {

        //Fascist Cards, shows them
        for (int i = 0; i < pathwayTracker; i++)
            cards[i].GetComponent<Renderer>().enabled = true;

        //Hides remaining cards
        for (int i = pathwayTracker; i < numCards; i++)
            cards[i].GetComponent<Renderer>().enabled = false;

    }
 
    //Increments fascist cards, then checks gamestate
    [Command(ignoreAuthority = true)]
    public void CmdIncrementFascistPath()
    {
       if (pathwayTracker < 6){
           pathwayTracker++;
       }
    }

    //Decrements fascist cards
    [Command(ignoreAuthority = true)]
    public void CmdDecrementFascistPath()
    {
       if (pathwayTracker > 0)
            pathwayTracker--;
           
    }

    //Resets fascist board
    [Command(ignoreAuthority = true)]
    public void CmdResetFascistPath()
    {
        Debug.Log("");
        pathwayTracker = 0;
    }

    //Checks gamestate
    private void CheckGameState(int prev, int current)
    {
        switch (pathwayTracker)
        {
            case 3:
            //Apply president peek at next 3 cards and red zone beggining here
            Debug.Log("President peeks at the next 3 cards. Red zone also begins!");
            break;

            case 4:
            //Apply president executes
            Debug.Log("President executes someone!");
            break;

            case 5:
            //Apply president executes and veto begins TODO: GET RID OF VETO START HERE, REFER TO VETO SCRIPT
            Debug.Log("President executes someone! Vetoing is now live!");
            Veto.SetVetoLive();
            Veto.StartVeto();
            break;

            case 6:
            //Fascists win
            Debug.Log("Fascists Win!");
            break;

            default:
            //Nothing special
            Debug.Log("Just a regular facist card");
            break;
        }

    }

    

 }


