using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class UsernameRenderer : MonoBehaviour {
    public GameObject[] children;
    public GameObject[] playersCards;
    public int players = 5;
    public string[] usernames;


    void Start() {
        children = new GameObject[players];
        playersCards = new GameObject[players];
        usernames = new string[players];
        string playerName = PlayerPrefs.GetString("Username");

        for( int i=0; i<players; i++) {
            children[i] = GameObject.Find("Username" + (i+1));
            playersCards[i] = GameObject.Find("Player " + (i+1));

            //TODO: get actual usernames
            usernames[i] = "Username" + (i + 1); 
            children[i].transform.position = playersCards[i].transform.position + new Vector3(0,70,0);
        }
        usernames[0] = playerName;
    }

    public void setUsernames() {
        for (int i=0; i<players; i++) {
            TMP_Text x = children[i].GetComponent<TMP_Text>();

            x.text = usernames[i];
        }
    }

}
