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

            //Set up voting buttons
            foreach (RoomPlayer player in roomSlots) {
                player.setUpVoting();
                player.loadUsernames();
            }


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

                }

            }
        }

        /*
         ***********************************************************
         *********************CUSTOM FUNCTIONS**********************
         ***********************************************************
        */

        public bool havePres() {
            foreach (RoomPlayer player in roomSlots) {
                if (player.role == "President"){
                    return true;
                }
            }
            return false;
        }

    }
}
