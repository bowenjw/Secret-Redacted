using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputName : MonoBehaviour {
    public string userName;
    public InputField inputField;

    public void setUsername(string input){
        userName = input;
    }

    public void Awake(){
        userName = PlayerPrefs.GetString("playerName", "Player");
        inputField.text = userName;
    }

}
