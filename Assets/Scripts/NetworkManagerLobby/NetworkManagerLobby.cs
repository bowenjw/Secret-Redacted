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

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;

        //public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

        /*
        public override void OnStartClient(){

            var spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            Debug.Log(spawnPrefabs);

            foreach (var prefab in spawnPrefabs) {
                Debug.Log("Registering prefab:" + prefab);
                ClientScene.RegisterPrefab(prefab);
            }
        }
        */

        public override void OnClientConnect(NetworkConnection conn) {
            base.OnClientConnect(conn);
            Debug.Log("Connected" + conn);
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

        public override void OnServerConnect(NetworkConnection conn) {
            if (numPlayers >= maxConnections) {
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
            /*int cnt = 1;
            foreach ( NetworkRoomPlayerLobby player in roomSlots) {
                GameObject.Find("Player"+cnt).GetComponentInChildren<TMP_Text>().text = player.username;
                cnt++;
            }*/
        }



        /*
        public override void OnServerAddPlayer(NetworkConnection conn) {
            if ("Assets/Scenes/" + SceneManager.GetActiveScene().name + ".unity" == menuScene) {
                NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(playerPrefab);

                NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
            }
        }
        */
    }
}
