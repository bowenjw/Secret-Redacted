using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class Username : MonoBehaviour {
    public string userName;
    public TMP_InputField inputField;
    public TMP_Text usernameText;

    public string[] bannedUsernames = {"fuck","shit"};

    public void getUsername(){
        userName = inputField.text;
        if (bannedUsernames.Contains(userName)){
            inputField.text = "Banned Name!!!";
            userName = "";
        }

        usernameText.text = "Username: " + userName;

    }

}
