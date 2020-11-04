using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
//using System.Linq;

public class Voting : MonoBehaviour {

    [SerializeField] public bool result = false;
    [SerializeField] public List<bool> voteHistory;
    [SerializeField] public int failedVotes = 0;
    public bool setHist = false;

    [SerializeField] public Button yesVote;
    [SerializeField] public Button noVote;
    [SerializeField] public Button selectPlayerBtn;
    public int playerIndex;

    public customLobby.RoomPlayer roomPlayer;

    void Start() {
        voteHistory = new List<bool>();
    }

    public void setUpBtns(int x) {
        playerIndex = x; 
        Mirror.NetworkRoomPlayer y = GameObject.Find("NetworkManager").GetComponent<customLobby.NetworkManagerLobby>().roomSlots[playerIndex];
        roomPlayer = (customLobby.RoomPlayer)y;
        int players = GameObject.Find("RolesHolder").GetComponent<Roles>().players;
        for (int i = 0; i < players; i++ ) {
            GameObject.Find("Player " + (i+1)+"/Select").transform.localPosition = new Vector3(0,-2000,0);
        }
        yesVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        noVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        yesVote.onClick.AddListener(voteYes);
        noVote.onClick.AddListener(voteNo);
    }

    public void loadObjs(int index) {
        int players = GameObject.Find("RolesHolder").GetComponent<Roles>().players;

        if (players > 0) {
            for (int i = 0;i < players; i++) {
                if (i == index) continue;
                GameObject.Find("Player "+(i+1)+"/Select").transform.localPosition = Vector3.zero;
            }
        }
    }

    public void selectPlayer(GameObject player) {
        int players = GameObject.Find("RolesHolder").GetComponent<Roles>().players;

        //This only happens on the client that started the vote
        for (int i = 0;i < players; i++) {
            GameObject.Find("Player "+(i+1)+"/Select").transform.localPosition = new Vector3(0,-2000,0);
        }

        roomPlayer.CmdSelectPlayer();
    }


    public void callVote() {
        yesVote.gameObject.transform.localPosition = new Vector3(180, -280, 0);
        noVote.gameObject.transform.localPosition = new Vector3(446, -280, 0);
        setHist = false;
    }

    public void endVote() {
        yesVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        noVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        if (!setHist) { voteHistory.Add(result);setHist = true;}
        roomPlayer.CmdVote(result);
        roomPlayer.CmdEndVote();
    }

    public void voteYes() {
        result = true;
        endVote();
    }

    public void voteNo() {
        result = false;
        endVote();
    }


}



