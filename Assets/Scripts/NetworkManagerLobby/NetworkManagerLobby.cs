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

                for (int i = 0; i < roomSlots.Count(); i++) {
                    ((NetworkRoomPlayerLobby)roomSlots[i]).party = roles.playerRoles[i];
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
