using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Mirror;

namespace customLobby {
    public class NetworkRoomPlayerLobby : NetworkRoomPlayer {

        public string username = "";

        public override void OnClientEnterRoom() {
            setUsername(username);
        }

        public void setUsername(string name) {
            if (!string.IsNullOrEmpty(username)){
                if (!string.IsNullOrEmpty(name)) {
                    username = name;
                }
            }
            else {
                username = Lobby.DisplayName;
            }
        }

        public override void OnClientExitRoom() {

        }
    }
}
