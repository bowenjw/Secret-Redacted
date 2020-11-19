using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using Mirror;

namespace customLobby {
    public class RoomPlayer : NetworkRoomPlayer {

        //****************SYNC VARS*****************
        [SyncVar(hook = nameof(aliveChanged))]
        public bool isAlive = true;

        [SyncVar(hook = nameof(roleChanged))]
        public string role;

        [SyncVar(hook = nameof(voted))]
        public bool vote;

        [SyncVar(hook = nameof(usernameChanged))]
        public string username;

        [SyncVar(hook = nameof(selectedPlayer))]
        public bool isSelected;

        [SyncVar(hook = nameof(needsToVote))]
        public bool hasToVote;

        [SyncVar(hook = nameof(gameStarted))]
        public bool startGame;

        [SyncVar]
        public int pathwayTracker = 0;

        //**************LOCAL VARS*****************
        //TODO: If these are local maybe try to make them private???
        public int electionTracker = 0;
        public string party;
        private bool inPosition = false;
        private bool loaded = false;

        [SerializeField] public Button readyButton;

        //Server
        public static NetworkManagerLobby lobby;

        //Voting obj
        public Voting voting;

        public override void OnStartClient() {
            //Called when the client starts 
            //Use this as if it's the Start() func

            lobby = GameObject.Find("NetworkManager").GetComponent<NetworkManagerLobby>();
            voting = this.GetComponent<Voting>();

            //username = PlayerPrefs.GetString("Username");
            username = "Player" + (index + 1);

        }

        public override void OnClientEnterRoom() {
            //Called on every RoomPlayer when a RoomPlayer enters the room

            if (SceneManager.GetActiveScene().name == "Lobby" && !inPosition) {
                //Setting up the lobby scene 

                GameObject x = GameObject.Find("Player"+(index+1));
                x.transform.localPosition = new Vector3( (-864 +(index * 192)), -295, 0);
                inPosition = true;
            }
            if (SceneManager.GetActiveScene().name == "Lobby" && hasAuthority){
                // In Lobby Scene

                if (!loaded) {
                    //Loads the readyButton if it hasn't already been loaded 

                    if (readyButton == null){
                      readyButton = GameObject.Find("readyUpButton").GetComponent<Button>();
                      readyButton.onClick.AddListener(delegate {CmdChangeReadyState(true);});
                      readyButton.onClick.AddListener(delegate {changeReadyButton(true);});
                    }
                    loaded = true;
                }

                //Loading the new username because another player joined 
                lobby.setLobbyUsernames(index);
            }
            else if (SceneManager.GetActiveScene().name == "5 Players" && hasAuthority) {
                // In 5 Players
                // This is only called because mirror creates gamePlayers and they "join" the room
                // So keep in mind that the gamePlayers are now calling this

                //TODO: Call this somewhere else
                lobby.updateUsername(username,index,"5 Players");

            }
            //username = PlayerPrefs.GetString("Username");
                
        }

        public override void OnClientExitRoom() {
            //Called on each RoomPlayer when a RoomPlayer leaves the room
        
            if (SceneManager.GetActiveScene().name == "Lobby") {
                loaded = false;
            }
        }

        /*
         ***********************************************************
         ************************HOOKS******************************
         ***********************************************************

         Hooks are called on every roomPlayer when any of their
         sync vars are updated, so if you make one roomPlayer 
         "President" then every roomPlayer would have their 
         roleChanged hook called.

        */


        public void selectedPlayer(bool _, bool selected) {
            //Hook to handle when a player is selected 

            //Player was selected and now we need to vote
            //Calling the server to start a vote
            if (selected) {
                lobby.callVote(index, isSelected); 
            }
        }

        public void aliveChanged(bool prevState, bool state) {
            //Hook to handle when a player has died 

            if (!state) {
                //TODO: Add functionality for what happens when player dies
                Debug.Log(username + " has died!");
            }
        }

        public void roleChanged(string prevRole, string currRole) {
            //Hook to handle when a player's role has changed

            if (currRole == "President") {
                //TODO: Get president text and move it to the current players card

                if (role == "President") {
                    //We are the president and we need to vote on a chancellor
                    voting.loadObjs(index);
                }
                Debug.Log(username + " is president!");
            }
            else if (currRole == "Chancellor") {
                //TODO: Get chancellor text and move it to the current players card

                Debug.Log(username + " is chancellor!");
            }
            else {
                Debug.Log("ERROR:" + username + "'s role was: " + role);
            }
        }

        public void needsToVote(bool _, bool playerHasToVote) {
            //Hook to handle when a player needs to vote 

            //Finds the player obj that represents this roomPlayer and calls it's callVote func
            //TODO: It shouldn't matter what player obj this is called on because all callVote does it set buttons active 
            if (playerHasToVote)
                voting.callVote();
        }

        public void voted(bool _, bool vote) {
            //Hook to handle when a player has voted

            Debug.Log(username + " voted " + (vote? "yes":"no"));
        }

        public override void ReadyStateChanged(bool _, bool state){
            //Hook to handle when the readyState has changed 
            
            //Calling the server to update the ready button and text 
            //It is fine that every player calls this because every player needs to render the updated ready text 
            lobby.playerChangedReadyState(state,index); 
        }

        public void usernameChanged(string prevName, string name) {
            //Hook to handle when a username has changed 
            //ATTN: This hook currently only gets called from OnStartClient 

            //This check is to see if we are the gamePlayer 
            if (SceneManager.GetActiveScene().name == "5 Players" && !readyToBegin) return;

            //Calling the server to set the player usernames
            lobby.updateUsername(name, index, SceneManager.GetActiveScene().name);
        }

        public void gameStarted(bool _, bool canStartGame) {
            //Hook to handle when the game starts  
            if (canStartGame) {
                GameObject.Find("GameLoop").GetComponent<GameLoop>().startBtn.gameObject.SetActive(false);
                GameObject.Find("GameLoop").GetComponent<GameLoop>().readyToStart = true;
            }
        }

        /*
         ***********************************************************
         *******************CUSTOM FUNCTIONS************************
         ***********************************************************
        */


        public void changeReadyButton(bool state){
            GameObject rB = GameObject.Find("readyUpButton");
            if (rB.GetComponent<Button>().onClick.GetPersistentEventCount() != 0) {
                rB.GetComponent<Button>().onClick.RemoveAllListeners();
            }
            if (state){
                rB.GetComponentInChildren<TMP_Text>().text = "Cancel";
            }
            else {
                rB.GetComponentInChildren<TMP_Text>().text = "Ready Up";
            }
            rB.GetComponent<Button>().onClick.AddListener(delegate {CmdChangeReadyState(!state);});
            rB.GetComponent<Button>().onClick.AddListener(delegate {changeReadyButton(!state);});
        }

        public void setUpVoting() {
            voting.setUpBtns();
            voting.addFuncs();
        }

        public bool hasVoted() {
            return voting.setHist;
        }

        /*
         ***********************************************************
         ************************COMMANDS***************************
         ***********************************************************

         Commands are the only way to safely update sync vars,
         they are slow and require the server and client to send
         messages back and forth to update; use them only when neccesary

         If you get authority warning giving the command the ignoreAuthority
         tag will fix the problem, but this is probably just a sign that
         we are not using the command correctly.

        */

        [Command(ignoreAuthority=true)]
        public void CmdStartGame() {
            startGame = true;
        }

        [Command]
        public void CmdIncrementLiberal() {
            //Called to update the pathwayTracker sync var
            pathwayTracker++;
        }

        [Command(ignoreAuthority=true)]
        public void CmdSelectPlayer() {
            //Called to update the isSelected sync var
            isSelected = true;
        }

        [Command(ignoreAuthority=true)]
        public void CmdDeselectPlayer() {
            //Called to update the isSelected sync var
            isSelected = false;
        }

        [Command(ignoreAuthority=true)]
        public void CmdChangeRole(string Role) {
            //Called to update the role sync var
            role = Role;
        }

        [Command(ignoreAuthority=true)]
        public void CmdVote(bool Vote) {
            //Called to update the vote sync var
            vote = Vote;
        }

        [Command(ignoreAuthority=true)]
        public void CmdCallVote() {
            //Called to update the hasToVote sync var
            hasToVote = true;
        }

        [Command(ignoreAuthority=true)]
        public void CmdEndVote() {
            //Called to update the hasToVote sync var
            hasToVote = false;
        }
    }
}
