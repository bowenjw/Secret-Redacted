using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Linq;


public class PresidentRoles : MonoBehaviour {
  public int numPlayers = 5;
  public int chancellor;
  public int president;
  public GameObject presidentObj;
  public GameObject chancellorObj;
  public List<GameObject> children;

  void Start() {
    presidentObj = GameObject.Find("presRole");
    chancellorObj = GameObject.Find("chancRole");

    children = new List<GameObject>();

    for (int i = 0; i < numPlayers; i++) {
      children.Add(GameObject.Find("Username" + (i+1)));
    }
  }

  public void assignPresident(string username){
    for (int i=0;i<numPlayers;i++){
      if (children[i].GetComponent<TMP_Text>().text == username){
        president = i;
        updateRoles();
        return;
      }
    }
    Debug.Log("ERROR: Username not found:"+username);
  }

  public void assignChancellor(string username){
    for (int i=0;i<numPlayers;i++){
      if (children[i].GetComponent<TMP_Text>().text == username){
        chancellor = i;
        updateRoles();
        return;
      }
    }
    Debug.Log("ERROR: Username not found:"+username);
  }

  public void updateRoles(){
    TMP_Text presidentText = presidentObj.GetComponent<TMP_Text>();
    TMP_Text chancellorText = chancellorObj.GetComponent<TMP_Text>();

    presidentText.text = "President: " + children[president].GetComponent<TMP_Text>().text;
    chancellorText.text = "Chancellor: " + children[chancellor].GetComponent<TMP_Text>().text;

    presidentObj.transform.position = children[president].transform.position + new Vector3(0,-150,0);
    chancellorObj.transform.position = children[chancellor].transform.position + new Vector3(0,-150,0);

    return;
  }
}
