using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using customLobby;
using Mirror;
using TMPro;


//Handles Execution
public class NewExecution : NetworkBehaviour
{

    //True means alive for the SyncList, False means dead
    [SerializeField] public SyncListBool playersStatus = new SyncListBool();
    int numPlayers = 5;

    //This will hold the player sprites
    public GameObject[] players;

    // Start is called before the first frame update
    void Start()
    {
        //Adds themselves to player array
        CmdAddPlayer();

        players = new GameObject[numPlayers];

        //Gets player sprites to hold
        for (int i = 0; i < numPlayers; i++) {
            players[i] = GameObject.Find("Player " + (i+1));
        }


    }

    // I have turned the update into a quasi-second start, since it must wait till all players connect!
    void Update()
    {
        if (numPlayers == playersStatus.Count()) {
            //If the player is dead gray out their card no matter what sprite is present
            for (int i = 0; i < numPlayers; i++) {
                if (IsDead(i))
                {
                    GrayOutCard(i);
                }
            }
        }
    }


    //Grays out the sprite so no matter the card present it is grayed out
    public void GrayOutCard (int playerIndex) {

        //Gets SpriteRenderer for the given player
        SpriteRenderer x = players[playerIndex].GetComponent<SpriteRenderer>();

        //Darkens Sprite to show they are dead
        x.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);


    }

    //Client calls this to kill a player
    public void KillPlayer (int playerIndex) {
        CmdPlayerKilled(playerIndex);
    }

    //Returns true if player is alive, false if player is dead
    public bool IsAlive (int playerIndex) {
        return playersStatus[playerIndex];
    } 


    //Returns true if player is dead, false if player is alive
    public bool IsDead (int playerIndex) {
        return !playersStatus[playerIndex];
    } 



    //******COMMANDS***********//
    
    //Each player calls this on start, adding a player to the playersStatus
    [Command(ignoreAuthority=true)]
    void CmdAddPlayer() {
        playersStatus.Add(true);
    }


    //Updates playerStatus to show this player is dead
    [Command(ignoreAuthority=true)]
    void CmdPlayerKilled(int playerIndex) {
        playersStatus[playerIndex] = false;
    }


}
