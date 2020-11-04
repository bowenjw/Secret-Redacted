using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using Mirror;
using System.Linq;

namespace customLobby {
    public class NetworkManagerLobby : NetworkRoomManager {

        [Scene] [SerializeField] private string menuScene = string.Empty;

        [SerializeField] public Roles roles;

        [SerializeField] public static CustomGame settings;

        // Actions used for calling in Lobby
        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action OnLobbyClientConnected;

        [SerializeField] public int amtPlayers;

        public List<bool> votes = null;

        // Index for the possible chancellor
        private int possChanc;

        public override void OnClientConnect(NetworkConnection conn) {
            //Called on each client when they connect initially 
            if (!clientLoadedScene) {
                if (!ClientScene.ready) ClientScene.Ready(conn);
                ClientScene.AddPlayer(conn);
            }
            //Invoking the action for Lobby
            OnClientConnected?.Invoke();
        }

        public override void OnClientDisconnect(NetworkConnection conn) {
            //Called on each client when they disconnect
            base.OnClientDisconnect(conn);

            //Invoking the action for Lobby
            OnClientDisconnected?.Invoke();
        }

        public override void OnRoomStartServer() {
            //Called on the server when the room is started

            //Get the custom settings
            settings = GameObject.Find("Settings").GetComponent<CustomGame>();
        }

        public override void OnServerConnect(NetworkConnection conn) {
            //Called on the server when any client connects 

            //If there are too many players disconnect the client
            if (numPlayers >= settings.amtPlayers) {
                conn.Disconnect();
                return;
            }

            if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" != menuScene) {
                Debug.Log(conn);
                conn.Disconnect();
                return;
            }
            Debug.Log("Connected:" + conn);
        }

        public override void OnServerDisconnect(NetworkConnection conn) {
            //Called on the server when a client disconnects
            base.OnServerDisconnect(conn);
            Debug.Log("Disconnected:" + conn);
        }

        public override void OnRoomClientEnter() {
            //Called on the server when a client enters the room

            //Invoking the action for Lobby
            OnLobbyClientConnected?.Invoke();
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnection conn, GameObject roomPlayer, GameObject gamePlayer) {
            //Default implementation returns true and replaces roomPlayer with gamePlayer
            return false;
        }

        public override void OnClientSceneChanged(NetworkConnection conn) {
            //Called on every client when the scene has changed 

            Debug.Log("ClientSceneChanged:" + conn);
            if (!ClientScene.ready) ClientScene.Ready(conn);

            if (ClientScene.localPlayer == null) {
                //ClientScene.AddPlayer(conn);
            }

            //Set up voting buttons
            //TODO: WHy is it only fucking using conn 0 why tf is the local connection the only one this stupid func gets called on

            //GameObject.Find("DEBUG").GetComponent<TMP_Text>().text = "Conn Id: "+conn.connectionId;
            //GameObject.Find("Player " + (conn.connectionId + 1)).GetComponent<Voting>().setUpBtns(conn.connectionId);
            if (conn.connectionId == 0) {
                //First player so set up their select buttons
                //GameObject.Find("Player " + (conn.connectionId + 1)).GetComponent<Voting>().loadObjs(conn.connectionId);
            }


        }

        public override void OnRoomServerSceneChanged(string scene) {
            //Called on the server when the scene has loaded for the room

            if (scene == "Assets/Scenes/5 Players.unity") {
                roles = GameObject.Find("RolesHolder").GetComponent<Roles>();
                roles.generateRoles();

                amtPlayers = roomSlots.Count();

                for (int i = 0; i < amtPlayers; i++) {

                    //Assign each player to their respective party
                    ((RoomPlayer)roomSlots[i]).party = roles.playerRoles[i];

                    //Give each player a votesHolder
                    NetworkServer.Spawn(GameObject.Find("votesHolder"),roomSlots[i].connectionToClient);

                    //Set up voting buttons
                    ((RoomPlayer)roomSlots[i]).CmdSetUpBtns();


                }

                //First player in the lobby is assigned president
                ((RoomPlayer)roomSlots[0]).role = "President";

                ((RoomPlayer)roomSlots[0]).CmdLoadSelectBtns();

                GameRenderer usernameHolder = GameObject.Find("UsernameHolder").GetComponent<GameRenderer>();

                usernameHolder.setPlayerCards(roomSlots.Count());

                usernameHolder.loadUsernames();
            }
        }

        /*
         ***********************************************************
         *********************CUSTOM FUNCTIONS**********************
         ***********************************************************
        */

        public void playerChangedReadyState(bool state, int index) {
            //Called from RoomPlayer when a player's readyState changes 
            //ATTN: This is called by every RoomPlayer

            //Update the readyState text for the changed player
            TMP_Text rT = GameObject.Find("Player"+(index+1)+"/"+"readyText").GetComponent<TMP_Text>();
            rT.text = state? "Ready":"Not Ready";
        }

        public void updateUsername(string username, int index, string scene) {
            //Called from RoomPlayer to update the usernames from usernameChanged
            //Also called from OnClientEnterRoom in game scene
            //ATTN: This is called by every RoomPlayer

            //TODO: Can we use amtPlayers here??
            int playerCnt = (int)(roomSlots.Count() / 2);

            if (scene == "Lobby"){
                //In the Lobby scene

                TMP_Text uT = GameObject.Find("Player"+(index+1)).GetComponentInChildren<TMP_Text>();
                uT.text = username;
            }
            else if (scene == "5 Players" && index < playerCnt){
                //In the game scene 

                //Set the username text to the correct usernames 
                for (int i = 0; i < playerCnt; i++) {
                    TMP_Text uT = GameObject.Find("Username"+(i+1)).GetComponent<TMP_Text>();
                    uT.text = ((RoomPlayer)roomSlots[i]).username; 
                }

                //Get the UsernameRenderer obj
                UsernameRenderer usernameHolder = GameObject.Find("UsernameHolder").GetComponent<UsernameRenderer>();

                //Move the player cards onto the board 
                usernameHolder.setPlayerCards(playerCnt);
                
                //Move the usernames onto the board
                usernameHolder.loadUsernames();
            }
        }

        public void setLobbyUsernames(int index) {
            //Called from RoomPlayer to update the usernames
            //ATTN: This is called by every RoomPlayer

            for (int i = index-1; i >= 0; i--) {
                TMP_Text uT = GameObject.Find("Player"+(i+1)).GetComponentInChildren<TMP_Text>();
                uT.text = ((RoomPlayer)roomSlots[i]).username; 
            }
        }

        public void checkIfAllVoted() {
            //Called from RoomPlayer when a player has voted
            //ATTN: This is called by every RoomPlayer

            for (int i = 0;i < amtPlayers;i++) {

                if (!GameObject.Find("Player "+(i+1)).GetComponent<Voting>().setHist){
                    // Someone still needs to vote
                    return;
                }

            }
            // Everyone is done voting 

            List<bool> yesVotes = new List<bool>();
            foreach (bool vote in votes) {
                if (vote) yesVotes.Add(vote);
            }

            Debug.Log("Everyone voted! Yes votes: " + yesVotes.Count());

            if (yesVotes.Count() > (int)(amtPlayers / 2)) {
                //Majority, so elect chancellor
                ((RoomPlayer)roomSlots[possChanc]).role = "Chancellor";
            }
            else {
                //TODO: Add functionality for when vote fails 

            }
        }

        public void callVote(int playerIndex) {
            //Called from RoomPlayer when a player has been selected
            //ATTN: This is called by every RoomPlayer

            //Make sure we don't overwrite the votes 
            if (votes == null || votes.Count() > 0)
                votes = new List<bool>();

            possChanc = playerIndex; 

            for (int i = 0;i < amtPlayers;i++) {
                //Call a vote on each player

                //Don't call vote if player is the selected player
                if (i == playerIndex) continue;

                //TODO: Make this called on every player somehow
                ((RoomPlayer)roomSlots[i]).CmdCallVote();
            }

            //Calls the timer
            GameObject.Find("Timer").GetComponent<Timer>().startTimer(delegate {
                for (int i = 0;i < amtPlayers;i++) {

                    votes.Add(GameObject.Find("Player "+(i+1)).GetComponent<Voting>().result);

                }

                Debug.Log("Finished Voting, size of votes: " + votes.Count());
                
            });

        }

    }
}
