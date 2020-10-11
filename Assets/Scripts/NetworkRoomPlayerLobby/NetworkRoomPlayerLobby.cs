using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using Mirror;

namespace customLobby {
    public class NetworkRoomPlayerLobby : NetworkRoomPlayer {

        [SyncVar(hook = nameof(usernameChanged))]
        public string username;

        [SyncVar(hook = nameof(aliveChanged))]
        public bool isAlive;

        [SyncVar(hook = nameof(roleChanged))]
        public string role;

        [SyncVar(hook = nameof(voted))]
        public bool vote;

        public string party;

        [SerializeField] public TMP_Text readyText;

        [SerializeField] public TMP_Text usernameText;

        [SerializeField] public Button readyButton;

        private bool inPosition = false;

        public override void OnClientEnterRoom() {

            if (string.IsNullOrEmpty(username)) { usernameChanged("",Lobby.displayName);}

            if (SceneManager.GetActiveScene().name == "Lobby" && isLocalPlayer){
                // In Lobby Scene

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
                if (!inPosition) {
                    GameObject x = GameObject.Find("Player"+(index+1));
                    x.transform.localPosition = new Vector3( (-864 +(index * 192)), -295, 0);
                    inPosition = true;
                }

            }
            else if(SceneManager.GetActiveScene().name == "5 Players" && isLocalPlayer) {
                // In 5 Players
            
                if (usernameText == null) {
                  usernameText = GameObject.Find("Username"+(index+1)).GetComponent<TMP_Text>();
                  usernameChanged(username,"");
                }

                if (string.IsNullOrEmpty(party)) {
                    party = GameObject.Find("RolesHolder").GetComponent<Roles>().playerRoles[index];
                }

                if (index == 0) {
                    roleChanged("","President");
                }

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

        public void aliveChanged(bool prevState, bool state) {
            if (!state) {
                //TODO: Add functionality for what happens when player dies
                Debug.Log(username + " has died!");
            }
        }

        public void roleChanged(string prevRole, string role) {
            if (role == "President") {
                //TODO: Get president text and move it to the current players card
                Debug.Log(username + " is president!");
            }
            else if (role == "Chancellor") {
                //TODO: Get chancellor text and move it to the current players card
                Debug.Log(username + " is chancellor!");
            }
            else {
                Debug.Log("ERROR:" + username + "'s role was: " + role);
            }
        }

        public void voted(bool _, bool vote) {
            Debug.Log(username + " voted " + (vote? "yes":"no"));
        }

        public override void ReadyStateChanged(bool _, bool state){
          if (readyText != null) {readyText.text = state? "Ready":"Not Ready";}
        }

        public override void OnClientExitRoom() {
          if (SceneManager.GetActiveScene().name == "Lobby") {
              usernameChanged("","Player"+(index+1));
              ReadyStateChanged(false,false);
          }
        }
    }
}
