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

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;

        public static event Action OnRoomClientConnected;

        [SerializeField] public int amtPlayers;

        public List<bool> votes = null;

        private int possChanc;

        public override void OnClientConnect(NetworkConnection conn) {
            if (!clientLoadedScene) {
                if (!ClientScene.ready) ClientScene.Ready(conn);
                ClientScene.AddPlayer(conn);
            }
            OnClientConnected?.Invoke();
        }

        public override void OnClientError(NetworkConnection conn, int err) {
            Debug.Log(conn + " " + err);
        }

        public override void OnServerError(NetworkConnection conn, int err) {
            Debug.Log(conn + " " + err);
        }

        public override void OnClientDisconnect(NetworkConnection conn) {
            base.OnClientDisconnect(conn);
            Debug.Log("Disconnected:" + conn);
            OnClientDisconnected?.Invoke();
        }

        public override void OnStartClient(){
            Debug.Log(numPlayers + " " + networkAddress + " " + isNetworkActive);
        }

        public override void OnRoomStartServer() {
            //Get the custom settings
            settings = GameObject.Find("Settings").GetComponent<CustomGame>();
        }

        public override void OnServerConnect(NetworkConnection conn) {
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
            base.OnServerDisconnect(conn);
            Debug.Log("Disconnected:" + conn);
        }


        public override void OnStopClient() {
            Debug.Log("Stopping client, num players: " + numPlayers);
        }

        public override void OnRoomClientEnter() {
            OnRoomClientConnected?.Invoke();
        }

        public void playerChangedReadyState(bool state, int index) {
            TMP_Text rT = GameObject.Find("Player"+(index+1)+"/"+"readyText").GetComponent<TMP_Text>();
            rT.text = state? "Ready":"Not Ready";
        }

        public void setPlayerUsername(string username, int index, string scene) {
            int playerCnt = (int)(roomSlots.Count() / 2);

            if (scene == "Lobby"){
                TMP_Text uT = GameObject.Find("Player"+(index+1)).GetComponentInChildren<TMP_Text>();
                uT.text = username;
            }
            else if (scene == "5 Players" && index < playerCnt){
                for (int i = 0; i < playerCnt; i++) {
                    TMP_Text uT = GameObject.Find("Username"+(i+1)).GetComponent<TMP_Text>();
                    uT.text = ((NetworkRoomPlayerLobby)roomSlots[i]).username; 
                }

                UsernameRenderer usernameHolder = GameObject.Find("UsernameHolder").GetComponent<UsernameRenderer>();

                usernameHolder.setPlayerCards(playerCnt);
                
                usernameHolder.loadUsernames();
            }
        }

        public void setPreviousUsernames(int index, string scene) {
            if (scene == "Lobby") {
                for (int i = index-1; i >= 0; i--) {
                    TMP_Text uT = GameObject.Find("Player"+(i+1)).GetComponentInChildren<TMP_Text>();
                    uT.text = ((NetworkRoomPlayerLobby)roomSlots[i]).username; 
                }
            }
        }

        public void checkIfAllVoted() {
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
                ((NetworkRoomPlayerLobby)roomSlots[possChanc]).role = "Chancellor";
            }
            else {
                //TODO: Add functionality for when vote fails 

            }
        }



        public void callVote(int playerIndex) {

            //Make sure we don't overwrite the votes 
            if (votes == null || votes.Count() > 0)
                votes = new List<bool>();
            possChanc = playerIndex; 

            for (int i = 0;i < amtPlayers;i++) {
                //Call a vote on each player

                //Don't call vote if player is the selected player
                if (i == playerIndex) continue;

                GameObject.Find("Player " + (i + 1) ).GetComponent<Voting>().callVote();
            }

            //Calls the timer
            GameObject.Find("Timer").GetComponent<Timer>().startTimer(delegate {
                for (int i = 0;i < amtPlayers;i++) {

                    votes.Add(GameObject.Find("Player "+(i+1)).GetComponent<Voting>().result);

                }

                Debug.Log("Finished Voting, size of votes: " + votes.Count());
                checkIfAllVoted();
                
            });

        }
            

        public override void OnRoomServerPlayersReady() {
            ServerChangeScene(GameplayScene);
        }

        public override void OnClientSceneChanged(NetworkConnection conn) {
            Debug.Log("ClientSceneChanged:" + conn);
            if (!ClientScene.ready) ClientScene.Ready(conn);

            if (ClientScene.localPlayer == null) {
                ClientScene.AddPlayer(conn);
            }
        }

        public override void OnRoomServerSceneChanged(string scene) {
            if (scene == "Assets/Scenes/5 Players.unity") {
                roles = GameObject.Find("RolesHolder").GetComponent<Roles>();
                roles.generateRoles();

                amtPlayers = roomSlots.Count();

                for (int i = 0; i < amtPlayers; i++) {

                    //Assign each player to their respective party
                    ((NetworkRoomPlayerLobby)roomSlots[i]).party = roles.playerRoles[i];

                    //Set up voting buttons
                    NetworkServer.Spawn(GameObject.Find("votesHolder"),roomSlots[i].connectionToClient);
                    ((NetworkRoomPlayerLobby)roomSlots[i]).setUpBtns();

                }

                //First player in the lobby is assigned president
                ((NetworkRoomPlayerLobby)roomSlots[0]).role = "President";

                UsernameRenderer usernameHolder = GameObject.Find("UsernameHolder").GetComponent<UsernameRenderer>();

                usernameHolder.setPlayerCards(roomSlots.Count());

                usernameHolder.loadUsernames();
            }
        }

    }
}
