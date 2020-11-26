using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using System.Linq;

namespace customLobby {
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
        public static RoomPlayer roomPlayer;

        void Start() {
            //Generate a list
            voteHistory = new List<bool>();
        }

        public void setUpVotingBtns() {
            //Called from RoomPlayer to set up the voting buttons,
            //but the initial call is from the server

            yesVote = GameObject.Find("yesVote").GetComponent<Button>();
            noVote = GameObject.Find("noVote").GetComponent<Button>();

            //Move the vote buttons offscreen
            yesVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
            noVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        }

        public void setUpSelectBtn(RoomPlayer iPlayer)  {
            //Called from RoomPlayer to set up the select button 
            //Set the player
            roomPlayer = iPlayer;

            //Set the player index
            playerIndex = roomPlayer.index; 

            selectPlayerBtn = GameObject.Find("Player "+ (playerIndex+1) + "/Select").GetComponent<Button>();

            //Move player's select button offscreen
            GameObject.Find("Player " + (playerIndex+1)+"/Select").transform.localPosition = new Vector3(0,-2000,0);

            //Add listener
            selectPlayerBtn.onClick.AddListener(selectPlayer);
        }

        public void addFuncs() {
            //Make sure this is called after setUpBtns
            //Add the funcs to the buttons 
            yesVote.onClick.AddListener(voteYes);
            noVote.onClick.AddListener(voteNo);
        }

        public void loadObjs(int index) {
            //Called from RoomPlayer when a player is selected,

            int players = GameObject.Find("RolesHolder").GetComponent<Roles>().players;

            //Move the player's select buttons onscreen if they're not the president
            if (players > 0) {
                for (int i = 0;i < players; i++) {
                    if (i == index) continue;
                    GameObject.Find("Player "+(i+1)+"/Select").transform.localPosition = Vector3.zero;
                }
            }
        }

        public void selectPlayer() {
            //Called from selecting a player using the select button

            int players = GameObject.Find("RolesHolder").GetComponent<Roles>().players;

            //This only happens on the client that started the vote
            for (int i = 0;i < players; i++) {
                GameObject.Find("Player "+(i+1)+"/Select").transform.localPosition = new Vector3(0,-2000,0);
            }

            //Calling the roomPlayer to let other clients know we selected someone
            roomPlayer.isSelected = true;
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
            roomPlayer.castVote(result);
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
}



