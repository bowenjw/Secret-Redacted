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

        [SyncVar(hook = nameof(needsToVote))]
        public bool hasToVote;

        [SyncVar]
        public int pathwayTracker = 0;

        public int electionTracker = 0;

        public string party;

        public bool inPosition = false;

        private bool loaded = false;

        [SerializeField] public Button readyButton;

        public static NetworkManagerLobby lobby;

        public override void OnStartClient() {
            lobby = GameObject.Find("NetworkManager").GetComponent<NetworkManagerLobby>();

            //username = PlayerPrefs.GetString("Username");
            username = "Player" + (index + 1);

        }
        public override void OnClientEnterRoom() {
            if (SceneManager.GetActiveScene().name == "Lobby" && !inPosition) {
                GameObject x = GameObject.Find("Player"+(index+1));
                x.transform.localPosition = new Vector3( (-864 +(index * 192)), -295, 0);
                inPosition = true;
            }
            if (SceneManager.GetActiveScene().name == "Lobby" && hasAuthority){
                // In Lobby Scene

                if (!loaded) {
                    if (readyButton == null){
                      readyButton = GameObject.Find("readyUpButton").GetComponent<Button>();
                      readyButton.onClick.AddListener(delegate {CmdChangeReadyState(true);});
                      readyButton.onClick.AddListener(delegate {changeReadyButton(true);});
                    }
                    loaded = true;
                }

                lobby.setPreviousUsernames(index,"Lobby");
            }
            else if (SceneManager.GetActiveScene().name == "5 Players" && hasAuthority) {
                // In 5 Players

                //TODO: Call this somewhere else
                lobby.setPlayerUsername(username,index,"5 Players");
                //setUpBtns();

            }
            //username = PlayerPrefs.GetString("Username");
                
        }

        [Command]
        public void CmdIncrementLiberal() {
            pathwayTracker++;
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

        [Command(ignoreAuthority=true)]
        public void CmdSetUpBtns() {
            GameObject.Find("Player " + (index + 1) ).GetComponent<Voting>().setUpBtns(index);
        }

        [Command(ignoreAuthority=true)]
        public void CmdSelectPlayer() {
            isSelected = true;
        }

        [Command]
        public void CmdDeselectPlayer() {
            isSelected = false;
        }

        [Command]
        public void CmdVote(bool Vote) {
            vote = Vote;
        }

        public void selectedPlayer(bool _, bool selected) {
            //Player was selected and now we need to vote

            if (selected)
                lobby.callVote(index); 
        }

        public void aliveChanged(bool prevState, bool state) {
            if (!state) {
                //TODO: Add functionality for what happens when player dies
                Debug.Log(username + " has died!");
            }
        }

        public void roleChanged(string prevRole, string currRole) {
            if (currRole == "President") {
                //TODO: Get president text and move it to the current players card

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

        [Command(ignoreAuthority=false)]
        public void CmdLoadSelectBtns() {
            GameObject.Find("Player " + (index + 1)).GetComponent<Voting>().loadObjs(index);
        }


        [Command]
        public void CmdCallVote() {
            hasToVote = true;
        }

        [Command]
        public void CmdEndVote() {
            hasToVote = false;
        }

        public void needsToVote(bool _, bool playerHasToVote) {
            GameObject.Find("Player " + (index + 1) ).GetComponent<Voting>().callVote();
        }

        public void voted(bool _, bool vote) {
            Debug.Log(username + " voted " + (vote? "yes":"no"));
            lobby.checkIfAllVoted();
        }

        public override void ReadyStateChanged(bool _, bool state){
          lobby.playerChangedReadyState(state,index); 
        }

        public void usernameChanged(string prevName, string name) {
            if (SceneManager.GetActiveScene().name == "5 Players" && !readyToBegin) return;
            lobby.setPlayerUsername(name, index, SceneManager.GetActiveScene().name);
        }

        public override void OnClientExitRoom() {
          if (SceneManager.GetActiveScene().name == "Lobby") {
              loaded = false;
          }
        }
    }
}
