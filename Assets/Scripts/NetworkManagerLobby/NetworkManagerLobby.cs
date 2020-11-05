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

        //Index of the chancellor
        private int chancIndex;
        //Index for the room
        private int roomCnt = 1;

        public List<bool> votes = null;

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
            //Setting this to true will cause OnRoomClientSceneChanged to update every client's scene 
            return true;
        }

        public override void OnRoomClientSceneChanged(NetworkConnection conn) {
            //Called on every client when the scene has changed 
            //ATTN: This might be called on the gamePlayers also
            //It doesn't seem to be called on the gamePlayers

            int pIndex = conn.connectionId;
            int cnt = 1;

            //Set up voting buttons
            foreach (RoomPlayer player in roomSlots) {
                GameObject.Find("Player " + cnt).GetComponent<Voting>().setUpBtns(cnt-1);
                cnt++;
            }

            GameObject.Find("Player " + roomCnt).GetComponent<Voting>().addFuncs();
            roomCnt++;

            GameRenderer usernameHolder = GameObject.Find("UsernameHolder").GetComponent<GameRenderer>();

            usernameHolder.setPlayerCards(amtPlayers);

            usernameHolder.loadUsernames();

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

                }

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

                //Get the GameRenderer obj
                GameRenderer usernameHolder = GameObject.Find("UsernameHolder").GetComponent<GameRenderer>();

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
                    // Someone didn't vote 
                    //TODO: Add functionality for when someone didn't vote
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
                ((RoomPlayer)roomSlots[chancIndex]).role = "Chancellor";
            }
            else {
                //TODO: Add functionality for when vote fails 

            }
        }

        public void callVote(int playerIndex, bool isChancellor) {
            //Called from RoomPlayer when a player has been selected
            //ATTN: This is called by every RoomPlayer

            //Make sure we don't overwrite the votes 
            if (votes == null)
                votes = new List<bool>();

            //If the calling RoomPlayer is chancellor set the chancellor index
            if (isChancellor)
                chancIndex = playerIndex;

            //Call for vote on RoomPlayer
            ((RoomPlayer)roomSlots[playerIndex]).CmdCallVote();

            //Calls the timer
            GameObject.Find("Timer").GetComponent<Timer>().startTimer(delegate {
                for (int i = 0;i < amtPlayers;i++) {

                    votes.Add(GameObject.Find("Player "+(i+1)).GetComponent<Voting>().result);

                }

                Debug.Log("Finished Voting, size of votes: " + votes.Count());
                checkIfAllVoted();
                
            });

        }

    }
}
