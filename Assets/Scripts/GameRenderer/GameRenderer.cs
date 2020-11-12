using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class GameRenderer : MonoBehaviour {
    public GameObject[] children;
    public GameObject[] playersCards;
    public int players = 5;
    public string[] usernames;

    public void setPlayerCards(int amtPlayers) {
        players = amtPlayers;
        playersCards = new GameObject[players];
        
        for( int i=0; i<players; i++) {
            playersCards[i] = GameObject.Find("Player " + (i+1));
            playersCards[i].transform.localPosition = new Vector3(-864 + (i * 192) , -310, 0);
        }
    }

    public void loadUsernames() {
        children = new GameObject[players];
        usernames = new string[players];
        string playerName = PlayerPrefs.GetString("Username");

        for( int i=0; i<players; i++) {
            children[i] = GameObject.Find("Username" + (i+1));

            usernames[i] = "Username" + (i + 1); 
            children[i].transform.position = playersCards[i].transform.position + new Vector3(0,90,0);
        }
    }


    public void setUsernames() {
        for (int i=0; i<players; i++) {
            TMP_Text x = children[i].GetComponent<TMP_Text>();

            x.text = usernames[i];
        }
    }

}
