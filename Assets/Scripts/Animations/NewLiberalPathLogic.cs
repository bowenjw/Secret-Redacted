using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using customLobby;
using Mirror;



public class NewLiberalPathLogic : MonoBehaviour
{
    private GameObject[] cards;
    private GameObject[] electionTokens;
    private int numCards = 5;
    private int numElectionTokens = 4;
    //private int pathwayTracker = 0;
    //private int electionTracker = 0;
    private NetworkRoomPlayerLobby[] players;
    private int numPlayers = 2;

    //Grabs cards and hides them all. Then does same for tracker.
    void Start()
    {

        //Grab all cards and place into array and hides them all
        cards = new GameObject[numCards];
        for(int i = 0; i < numCards; i++)
        {
            cards[i] = GameObject.Find("Policy Card Liberal #" + (i+1));
            cards[i].GetComponent<Renderer>().enabled = false;
        }

        //Same as above but election trackers
        electionTokens = new GameObject[numElectionTokens];
        for(int i = 0; i < numElectionTokens; i++)
        {
            electionTokens[i] = GameObject.Find("Election Tracker #" + (i+1));
            electionTokens[i].GetComponent<Renderer>().enabled = false;
        }

        //Gets players from Network Manager
        players = new NetworkRoomPlayerLobby[numPlayers];

        for(int i = 0; i < numPlayers; i++) 
        {
            players[i] = (NetworkRoomPlayerLobby)GameObject.Find("NetworkManager").GetComponent<NetworkManagerLobby>().roomSlots[i];
        }


    }

    //Sets board based on pathwayTracker and electionTracker
    void Update()
    {
        //Liberal Cards, shows them
        for (int i = 0; i < players[0].pathwayTracker; i++)
            cards[i].GetComponent<Renderer>().enabled = true;

        //Hides remaining cards
        for (int i = players[0].pathwayTracker; i < numCards; i++)
            cards[i].GetComponent<Renderer>().enabled = false;

        //Election Tracker, shows them 
        for (int i = 0; i < players[0].electionTracker; i++)
            electionTokens[i].GetComponent<Renderer>().enabled = true;

        //Hides remaining tokens
        for (int i = players[0].electionTracker; i < numElectionTokens; i++)
            electionTokens[i].GetComponent<Renderer>().enabled = false;
    }
 
    //Increments liberal path
    public void IncrementLiberalPath()
    {
       if (players[0].pathwayTracker < 5) 
           for (int i = 0; i < numPlayers; i++)
                players[i].pathwayTracker++;
           
    }

    //Decrements liberal path
    public void DecrementLiberalPath()
    {
       if (players[0].pathwayTracker > 0)
           for (int i = 0; i < numPlayers; i++)
                players[i].pathwayTracker--;
           
    }

    //Resets liberal path
    public void ResetLiberalPath()
    {
        for (int i = 0; i < numPlayers; i++)
                players[i].pathwayTracker = 0;
    }

    //Increments tracker path
    public void IncrementTracker()
    {
       if (players[0].electionTracker < 4) 
           for (int i = 0; i < numPlayers; i++)
                players[i].electionTracker++;
    }

    //Decrements tracker path
    public void DecrementTracker()
    {
       if (players[0].electionTracker > 0)
           for (int i = 0; i < numPlayers; i++)
                players[i].electionTracker--;
           
    }

    //Resets trackers
    public void ResetTracker()
    {
        for (int i = 0; i < numPlayers; i++)
                players[i].electionTracker = 0;
    }

    

 }


