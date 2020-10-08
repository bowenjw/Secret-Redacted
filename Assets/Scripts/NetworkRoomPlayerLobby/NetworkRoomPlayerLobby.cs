using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Mirror;

namespace customLobby {
    public class NetworkRoomPlayerLobby : NetworkRoomPlayer {

        [SyncVar(hook = nameof(usernameChanged))]
        public string username;

        [SerializeField] public TMP_Text readyText;

        [SerializeField] public TMP_Text usernameText;

        [SerializeField] public Button readyButton;

        public override void OnClientEnterRoom() {
            if (string.IsNullOrEmpty(username)) { usernameChanged("",Lobby.displayName);}
            if (usernameText == null){
              usernameText = GameObject.Find("Player"+(index+1)).GetComponentInChildren<TMP_Text>();
              usernameChanged(username,"");
            }
            if (readyText == null){
              readyText = GameObject.Find("Player"+(index+1)+"/"+"readyText").GetComponent<TMP_Text>();
            }
            if (readyButton == null){
              readyButton = GameObject.Find("readyUpButton").GetComponent<Button>();
              readyButton.onClick.AddListener(delegate {CmdChangeReadyState(true);});
              readyButton.onClick.AddListener(delegate {changeReadyButton(true);});
            }
        }

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

        public void usernameChanged(string prevName, string userName) {
            if (string.IsNullOrEmpty(userName)){
                if (!string.IsNullOrEmpty(prevName)){
                  username = prevName;
                }
                else {
                  usernameChanged("",Lobby.displayName);
                }
            }
            else {
                username = userName;
            }
            if (usernameText != null) {usernameText.text = username;}
        }

        public override void ReadyStateChanged(bool _, bool state){
          if (readyText != null) {readyText.text = state? "Ready":"Not Ready";}
        }

        public override void OnClientExitRoom() {
          usernameChanged("","Player"+(index+1));
          ReadyStateChanged(false,false);
        }
    }
}
