using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using customLobby;
using Mirror;

//isAlive tells the opposite, it is like isDead.

public class Execution : MonoBehaviour
{
    //public int playerCount = 2;
    public int numPlayers = 2;
    //public int maxDead = 3;
    private Sprite deathSprite;
    private GameObject[] buttons;
    private int[] deadPlayers;
    private NetworkRoomPlayerLobby[] players;
    private GameObject[] playersCards;

    void Start()
    {
        //Grab death sprite
        deathSprite = Resources.Load<Sprite>("deadDefaultBack");

        //Finds buttons and places them in array
        buttons = new GameObject[numPlayers];

        for(int i = 0; i < numPlayers; i++)
        {
            buttons[i] = GameObject.Find("Execution Button " + (i+1));
        }

        //Finds player cards
        playersCards = new GameObject[numPlayers];

        for(int i = 0; i < numPlayers; i++)
        {
            playersCards[i] = GameObject.Find("Player " + (i+1));
        }

        //Turns off Buttons
        ButtonsOff();

        //Gets players from Network Manager
        players = new NetworkRoomPlayerLobby[numPlayers];

        for(int i = 0; i < numPlayers; i++) 
        {
            players[i] = (NetworkRoomPlayerLobby)GameObject.Find("NetworkManager").GetComponent<NetworkManagerLobby>().roomSlots[i];
        }

        
        /*
        //Seed dead player with 99, I would use a list but it was giving me hassles so fooey
        deadPlayers = new int[maxDead];
        for(int i = 0; i < maxDead; i++)
        {
            deadPlayers[i] = 99;
        }
        */

    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < numPlayers; i++)
        {
            Debug.Log("Player count is " + numPlayers + " but i is " + i + " and they are " + isDead(i));

            if(isDead(i))
            {
                SpriteRenderer x = playersCards[i].GetComponent<SpriteRenderer>();
                x.sprite = deathSprite;
            }

        }
    }

    //Figures out which button is pressed and kills that player
    public void ExecutePlayer(GameObject player)
    {
        //Turns off all buttons
        ButtonsOff();

        //Get player number that is killed
        int playerKilledNumber = Int32.Parse(player.name.Remove(0, 7));

        //Add player to deadPlayers
        HasDied(playerKilledNumber);

        //Change sprite
        //SpriteRenderer x = player.GetComponent<SpriteRenderer>();
        //x.sprite = deathSprite;




    }

    //Only turns on buttons of alive players
    public void ButtonsOn()
    {
        for(int i = 0; i < numPlayers; i++) 
        {
            //Dead players are not turned on
            if (isDead(i))
            {
                buttons[i].SetActive(false);
            }
            else
            {
                buttons[i].SetActive(true);
            }
        }
    }

    //Turns off buttons, so that you cannot execute whenever
    void ButtonsOff()
    {
        for(int i = 0; i < numPlayers; i++) 
        {
            buttons[i].SetActive(false);
        }
    }

    //Checks to see if a player index number is dead
    bool isDead(int playerIndex)
    {
        Debug.Log("Entered isDead");
        Debug.Log(playerIndex);
        Debug.Log(players[playerIndex].isAlive);
        return players[playerIndex].isAlive;
    
        /*
        for(int i = 0; i < maxDead; i++)
        {
            if (playerIndex == deadPlayers[i])
                return true;
        }
        return false;
        */
    }


    //Kills player
    void HasDied (int playerNumber)
    {

        int playerIndex = playerNumber - 1;


        players[playerIndex].isAlive = true;


        /*
        for(int i = 0; i < maxDead; i++)
        {
            if (deadPlayers[i] == 99)
            {
                deadPlayers[i] = playerNumber - 1;
                return;
            }
        }
        */
    }



}
