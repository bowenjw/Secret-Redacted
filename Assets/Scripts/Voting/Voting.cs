using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
//using System.Linq;

public class Voting : MonoBehaviour {

    public bool result = false;
    public bool setHist = false;
    public List<bool> voteHistory;

    public Button yesVote;
    public Button noVote;

    void Start() {
        yesVote.onClick.AddListener(voteYes);
        noVote.onClick.AddListener(voteNo);
        voteHistory = new List<bool>();
    }

    public void callVote() {
        yesVote.gameObject.SetActive(true);
        noVote.gameObject.SetActive(true);
        setHist = false;
    }

    public void endVote() {
        yesVote.gameObject.SetActive(false);
        noVote.gameObject.SetActive(false);
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



