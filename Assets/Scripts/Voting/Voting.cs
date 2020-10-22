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

    void Start() {
        voteHistory = new List<bool>();
        yesVote.onClick.AddListener(voteYes);
        noVote.onClick.AddListener(voteNo);
    }

    public void setUpBtns() {
        selectPlayerBtn.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        yesVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        noVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
    }

    public void loadObjs() {
        int players = GameObject.Find("RolesHolder").GetComponent<Roles>().players;

        if (players > 0) {
            for (int i = 0;i < players; i++) {
                GameObject.Find("Player "+(i+1)+"/Select").transform.localPosition = Vector3.zero;
            }
        }
    }

    public void selectPlayer(GameObject player) {
        int players = GameObject.Find("RolesHolder").GetComponent<Roles>().players;

        for (int i = 0;i < players; i++) {
            GameObject.Find("Player "+(i+1)+"/Select").transform.localPosition = new Vector3(0,-2000,0);
        }
        
        int playerIndex;
        if (player.name.Length == 8) { playerIndex = player.name[7];}
        else { playerIndex = Int32.Parse(player.name[7] + "" + player.name[8]);}
        
        if (!GameObject.Find("NetworkManager").GetComponent<customLobby.NetworkManagerLobby>().callVote(playerIndex - 1)) {
            failedVotes++;
            loadObjs();
        }
    }


    public void callVote() {
        yesVote.gameObject.transform.localPosition = new Vector3(551, -180, 0);
        noVote.gameObject.transform.localPosition = new Vector3(828, -180, 0);
        setHist = false;
    }

    public void endVote() {
        yesVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        noVote.gameObject.transform.localPosition = new Vector3(0,-2000,0);
        if (!setHist) { voteHistory.Add(result);setHist = true;}
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



