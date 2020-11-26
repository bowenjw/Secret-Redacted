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
        public bool isAlive;

        [SyncVar(hook = nameof(roleChanged))]
        public string role;

        [SyncVar(hook = nameof(voted))]
        public bool vote;

        [SyncVar(hook = nameof(usernameChanged))]
        public string username;

        [SyncVar(hook = nameof(selectedPlayer))]
        public bool isSelected;

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
        public List<bool> voteCount;

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

            if (SceneManager.GetActiveScene().name == "Lobby") {
                // In Lobby Scene

                if (!inPosition) {
                    //Setting up the lobby scene 

                    GameObject x = GameObject.Find("Player"+(index+1));
                    x.transform.localPosition = new Vector3( (-864 +(index * 192)), -295, 0);
                    inPosition = true;
                }

                if (!loaded) {
                    //Loads the readyButton if it hasn't already been loaded 

                    if (readyButton == null){
                      readyButton = GameObject.Find("readyUpButton").GetComponent<Button>();
                      readyButton.onClick.AddListener(delegate {CmdChangeReadyState(true);});
                      readyButton.onClick.AddListener(delegate {changeReadyButton(true);});
                    }
                    loaded = true;
                }

                if (hasAuthority) {
                    //Loading the new username because another player joined 
                    setLobbyUsernames();
                }
            }
            else if (SceneManager.GetActiveScene().name == "5 Players" && hasAuthority && !isLocalPlayer) {
                loadUsernames();
                setUpVotingBtns();
            }
                
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

            voteCount = new List<bool>();

            if (!selected) return;

            //Player was selected and now we need to vote
            //Calling the server to start a vote
            foreach (RoomPlayer player in lobby.roomSlots) {
                if (player.voting.selectPlayerBtn == null) continue;
                player.callVote();
            }

            selectPlayer();
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

        public void voted(bool _, bool vote) {
            //Hook to handle when a player has voted

            //TODO: This only works when the vote val is being changed, maybe find a way for it to add to the list either way
            voteCount.Add(vote);

            Debug.Log(username + " voted " + (vote? "yes":"no"));
        }

        public override void ReadyStateChanged(bool _, bool state){
            //Hook to handle when the readyState has changed 
            
            //Calling the server to update the ready button and text 
            setReadyState(); 
        }

        public void usernameChanged(string prevName, string name) {
            //Hook to handle when a username has changed 
            //ATTN: This hook currently only gets called from OnStartClient 

            //This check is to see if we are the gamePlayer 
            if (SceneManager.GetActiveScene().name == "5 Players" && !readyToBegin) return;

            setLobbyUsernames();
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
         ************************Custom Functions*******************
         ***********************************************************
        */

        private void deselectPlayers() {
            foreach (RoomPlayer player in lobby.roomSlots) {
                player.deselectPlayer();
            }
        }

        private bool hasVoted() {
            return voting.setHist;
        }

        /*
         ***********************************************************
         ************************Client Functions*******************
         ***********************************************************

         Functions that are only called by the client

        */

        [Client]
        public void setLobbyUsernames() {
            CmdSetUsernames();
        }

        [Client]
        public void loadUsernames() {
            CmdLoadUsernames();
        }

        [Client]
        public void setReadyState() {
            CmdSetReadyState();
        }

        [Client]
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

        [Client]
        public void setUpVoting(RoomPlayer iPlayer) {
            voting.setUpSelectBtn(iPlayer);
        }

        [Client]
        public void setUpVotingBtns() {
            voting.setUpVotingBtns();
            voting.addFuncs();
        }

        [Client]
        public void callStartGame() {
            CmdStartGame();
        }

        [Client]
        public void selectPlayer() {
            CmdSelectPlayer(true);
        }

        [Client]
        public void deselectPlayer() {
            CmdSelectPlayer(false);
        }

        [Client]
        public void callVote() {
            //We have to vote
            CmdCallVote();

            if (isSelected) {
                GameObject.Find("Timer").GetComponent<Timer>().startTimer(delegate {
                    checkIfAllVoted();
                });
            }
            
        }

        [Client]
        public void castVote(bool Vote) {
            CmdCastVote(Vote);
        }

        [Client]
        public void setRole(string role) {
            CmdChangeRole(role);
        }

        [Client]
        public void checkIfAllVoted() {
            //Called to see if everyone has voted 
            List<bool> yesVotes = new List<bool>();

            foreach (RoomPlayer player in lobby.roomSlots) {

                if (player.voting.selectPlayerBtn == null) continue;

                //TODO:This isnt working 
                if (voteCount.Count != lobby.amtPlayers){
                    // Someone didn't vote 
                    Debug.Log("Someone didn't vote");

                    //Vote failed
                    voting.failedVotes++;

                    //Find the president
                    if (voting.failedVotes < 3) {
                        //Start another selection for chancellor
                        voting.loadObjs(index);
                    }
                    else {
                        //We have already failed 3 votes so select new pres
                        //Our GameLoop will handle it
                        setRole("");
                    }
                    deselectPlayers();
                    return;
                }
            }

            // Everyone is done voting 
            // Count all the votes
            foreach (bool Vote in voteCount) {

                if (Vote == true) {
                    yesVotes.Add(vote);
                }
            }

            Debug.Log("Everyone voted! Yes votes: " + yesVotes.Count);

            if (yesVotes.Count > (int)(lobby.amtPlayers / 2)) {
                //Majority, so elect chancellor
                //TODO: This might not be working, it is setting the local player chancellor every time for some reason
                foreach (RoomPlayer player in lobby.roomSlots) {
                    if (player.isSelected) {
                        player.setRole("Chancellor");
                    }
                }
            }
            else {
                //Vote failed
                voting.failedVotes++;

                if (voting.failedVotes < 3) {
                    //Start another selection for chancellor
                    voting.loadObjs(index);
                }
                else {
                    //We have already failed 3 votes so select new pres
                    //Our GameLoop will handle it
                    setRole("");
                }
            }
            deselectPlayers();
        }

        /*
         ***********************************************************
         ************************COMMANDS***************************
         ***********************************************************

         Commands are the only way to safely update sync vars,
         Commands are called from the server, they have all the authority
         and values from the server.

         If you get authority warning giving the command the ignoreAuthority
         tag will fix the problem, but this is probably just a sign that
         we are not using the command correctly.

        */
        [Command]
        public void CmdSetUsernames() {
            //Update the usernames for the players
            RpcSetUsernames();
        }

        [Command]
        public void CmdLoadUsernames() {
            //Loads the usernames for the players
            int playerCnt = (int)(lobby.roomSlots.Count / 2);

            if (index < playerCnt){

                //Set the username text to the correct usernames 
                RpcSetGameUsernames();

            }
        }

        [Command]
        public void CmdSetReadyState() {
            //Update the readyState text for the changed player
            RpcSetReadyStates();
        }

        [Command]
        public void CmdStartGame() {
            //Called to start the game 
            RpcStartGame();
        }
        [Command]
        public void CmdIncrementLiberal() {
            //Called to update the pathwayTracker sync var
            pathwayTracker++;
        }

        [Command(ignoreAuthority=true)]
        public void CmdSelectPlayer(bool state) {
            //Called to update the isSelected sync var
            RpcSelectPlayer(state);
        }

        [Command(ignoreAuthority=true)]
        public void CmdChangeRole(string Role) {
            //Called to update the role sync var
            RpcChangeRole(Role);
        }

        [Command(ignoreAuthority=true)]
        public void CmdCallVote() {
            //Called to update the hasToVote sync var
            //TODO: dont call this on the game players
            if (!isLocalPlayer) {
                RpcCallVote();
            }
        }

        [Command(ignoreAuthority=true)]
        public void CmdCastVote(bool Vote) {
            //Called to cast vote
            RpcCastVote(Vote);
        }

        /*
         ***********************************************************
         ************************ClientRpc**************************
         ***********************************************************

         ClientRpc is called on every client from the server.
         Use them when you want every client to do something.
         Treat them as similtanious calls.

        */

        [ClientRpc]
        public void RpcSetUsernames() {
            TMP_Text uT = GameObject.Find("Player"+(index+1)).GetComponentInChildren<TMP_Text>();
            uT.text = username;
        }

        [ClientRpc]
        public void RpcSetReadyStates() {
            TMP_Text rT = GameObject.Find("Player"+(index+1)+"/"+"readyText").GetComponent<TMP_Text>();
            rT.text = readyToBegin? "Ready":"Not Ready";
        }

        [ClientRpc]
        public void RpcCallVote() {
            //Called on every client to initiate vote
            GameObject.Find("Timer").GetComponent<Timer>().startTimer();
            voting.callVote();
        }

        [ClientRpc]
        public void RpcCastVote(bool Vote) {
            //Called to cast vote
            vote = Vote;
        }

        [ClientRpc]
        public void RpcSetGameUsernames() {
            int playerCnt = (int)(lobby.roomSlots.Count / 2);

            TMP_Text uT = GameObject.Find("Username"+(index+1)).GetComponent<TMP_Text>();
            uT.text = username; 

            //Get the GameRenderer obj
            GameRenderer usernameHolder = GameObject.Find("UsernameHolder").GetComponent<GameRenderer>();

            //Move the player cards onto the board 
            usernameHolder.setPlayerCards(playerCnt);
            
            //Move the usernames onto the board
            usernameHolder.loadUsernames();
        }

        [ClientRpc]
        public void RpcChangeRole(string Role) {
            role = Role;
        }

        [ClientRpc]
        public void RpcStartGame() {
            //Called on every client to start the game 
            startGame = true;
            GameObject.Find("GameLoop").GetComponent<GameLoop>().readyToStart = true;
            GameObject.Find("GameLoop").GetComponent<GameLoop>().startBtn.gameObject.SetActive(false);
        }

        [ClientRpc]
        public void RpcSelectPlayer(bool state) {
            //Called on every client to select a player
            isSelected = state;
        }

        [ClientRpc]
        public void RpcDeselectPlayers() {
            //Called on every client to deselect players
            isSelected = false;
        }

    }
}
