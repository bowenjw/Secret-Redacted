using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using System.Linq;

public class Voting : MonoBehaviour {
    /*
     * This script is on every player obj!
    */

    [SerializeField] public bool result = false;
    [SerializeField] public List<bool> voteHistory;
    [SerializeField] public int failedVotes = 0;
    public bool setHist = false;

    //Buttons 
    [SerializeField] public Button yesVote;
    [SerializeField] public Button noVote;
    [SerializeField] public Button selectPlayerBtn;

    //The player index that this script is attached to
    public int playerIndex;

    //The player this script is attached to
    public customLobby.RoomPlayer roomPlayer;

    void Start() {
        //Generate a list
        voteHistory = new List<bool>();
    }

    public void setUpBtns(int x) {
        //Called from RoomPlayer to set up the voting buttons,
        //but the initial call is from the server

        //Set the player index
        playerIndex = x; 

        //Set the player
        Mirror.NetworkRoomPlayer y = GameObject.Find("NetworkManager").GetComponent<customLobby.NetworkManagerLobby>().roomSlots[playerIndex];
        roomPlayer = (customLobby.RoomPlayer)y;

        //Get the num of players
        int players = GameObject.Find("RolesHolder").GetComponent<Roles>().players;

        //Move every player's select button offscreen
        for (int i = 0; i < players; i++ ) {
            GameObject.Find("Player " + (i+1)+"/Select").transform.localPosition = new Vector3(0,-2000,0);
        }

        //Move the vote buttons offscreen
        yesVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        noVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
    }

    public void addFuncs() {
        //Make sure this is called after setUpBtns
        //Add the funcs to the buttons 
        yesVote.onClick.AddListener(voteYes);
        noVote.onClick.AddListener(voteNo);
    }

    public void loadObjs(int index) {
        //Called from RoomPlayer when a player is selected,
        //but the initial call is from the server
        //Also this is only initially called on the first RoomPlayer

        int players = GameObject.Find("RolesHolder").GetComponent<Roles>().players;

        //Move the player's select buttons onscreen if they're not the president
        if (players > 0) {
            for (int i = 0;i < players; i++) {
                if (i == index) continue;
                GameObject.Find("Player "+(i+1)+"/Select").transform.localPosition = Vector3.zero;
            }
        }
    }

    public void selectPlayer(GameObject player) {
        //Called from selecting a player using the select button

        int players = GameObject.Find("RolesHolder").GetComponent<Roles>().players;

        //This only happens on the client that started the vote
        for (int i = 0;i < players; i++) {
            GameObject.Find("Player "+(i+1)+"/Select").transform.localPosition = new Vector3(0,-2000,0);
        }

        //Calling the roomPlayer to let other clients know we selected someone
        roomPlayer.CmdSelectPlayer();
    }


    public void callVote() {
        //Called from RoomPlayer when there needs to be a vote

        yesVote.gameObject.transform.localPosition = new Vector3(180, -280, 0);
        noVote.gameObject.transform.localPosition = new Vector3(446, -280, 0);
        setHist = false;
    }

    public void endVote() {
        yesVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        noVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        if (!setHist) { voteHistory.Add(result);setHist = true;}

        //Calling RoomPlayer to change the vote sync var 
        roomPlayer.CmdVote(result);
        //Calling RoomPlayer to let other clients know we are done voting
        roomPlayer.CmdEndVote();
    }

    //********Button Functions***********

    public void voteYes() {
        result = true;
        endVote();
    }

    public void voteNo() {
        result = false;
        endVote();
    }


}



