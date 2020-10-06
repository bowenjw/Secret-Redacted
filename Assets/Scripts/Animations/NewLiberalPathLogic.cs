using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class NewLiberalPathLogic : MonoBehaviour
{
    private GameObject[] cards;
    private GameObject[] electionTokens;
    private int numCards = 5;
    private int numElectionTokens = 4;
    private int pathwayTracker = 0;
    private int electionTracker = 0;

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


    }

    //Sets board based on pathwayTracker and electionTracker
    void Update()
    {
        //Liberal Cards, shows them
        for (int i = 0; i < pathwayTracker; i++)
            cards[i].GetComponent<Renderer>().enabled = true;

        //Hides remaining cards
        for (int i = pathwayTracker; i < numCards; i++)
            cards[i].GetComponent<Renderer>().enabled = false;

        //Election Tracker, shows them
        for (int i = 0; i < electionTracker; i++)
            electionTokens[i].GetComponent<Renderer>().enabled = true;

        //Hides remaining tokens
        for (int i = electionTracker; i < numElectionTokens; i++)
            electionTokens[i].GetComponent<Renderer>().enabled = false;
    }
 
    //Increments liberal path
    public void IncrementLiberalPath()
    {
       if (pathwayTracker < 5) 
           pathwayTracker++;
           
    }

    //Decrements liberal path
    public void DecrementLiberalPath()
    {
       if (pathwayTracker > 0)
           pathwayTracker--;
           
    }

    //Resets liberal path
    public void ResetLiberalPath()
    {
        pathwayTracker = 0;
    }

    //Increments tracker path
    public void IncrementTracker()
    {
       if (electionTracker < 4) 
           electionTracker++;
    }

    //Decrements tracker path
    public void DecrementTracker()
    {
       if (electionTracker > 0)
           electionTracker--;
           
    }

    //Resets trackers
    public void ResetTracker()
    {
        electionTracker = 0;
    }

    

 }


