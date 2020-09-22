using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class UsernameRenderer : MonoBehaviour {
    public GameObject[] children;
    public int players = 5;
    public string[] usernames;


    void Start() {
        children = new GameObject[players];
        usernames = new string[players];
        string playerName = PlayerPrefs.GetString("Username");

        for( int i=0; i<players; i++) {
            children[i] = GameObject.Find("Username" + (i+1));

            //TODO: get actual usernames
            usernames[i] = "Username" + (i + 1); 
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
