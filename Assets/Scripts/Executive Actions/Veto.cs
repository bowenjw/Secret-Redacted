using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//TODO: Place StartVeto() as Chancellor gets their cards, it will check itself to see if vetoing is needed now.
//TODO: Place ButtonsOff() after Chancellor discards their card and card is played
//TODO: After successful veto need to implement discarding both cards, advancing election tracker, and continuing president title
    //Implement this into PresVetos()

public class Veto : MonoBehaviour
{

    //private int numPlayers = 5;
    private GameObject chancBtn;
    private GameObject presBtn;
    private bool needVeto = false;

    //customLobby.NetworkRoomPlayerLobby[] players; //TODO
    


    //Finds buttons and turns them off
    void Start()
    {
        //Find Chancellor and President Buttons
        presBtn = GameObject.Find("President Veto Button");
        chancBtn = GameObject.Find("Chancellor Veto Button");


        ButtonsOff();

        //for(int i = 0; i < 5; i++) //TODOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
        //{
        //    players[i] = GameObject.Find("NetworkManager").GetComponent<customLobby.NetworkManagerLobby>().roomSlots[i];
        //}
    }

    //Makes sure only Chancellor and President can press the button
    void Update()
    {
        
    }

    //Starts the veto process permanently keeps it enabled
    public void SetVetoLive()
    {
        needVeto = true;
    }


    //Comes into effect once vetoing is live and this should be ran once Chancellor gets their cards
    public void StartVeto()
    {
        if (needVeto)
        {
            chancBtn.SetActive(true);
        }
    }


    //The Chancellor agreed to Veto, ball is in President's court
    public void ChancVetos()
    {
        Debug.Log("Chancellor wants to veto!");
        ButtonsOff();
        presBtn.SetActive(true);
    }

    //Turns off the buttons and need to implement discarding both cards, advancing election tracker, and continuing president title
    public void PresVetos()
    {
        Debug.Log("President wants to veto!");
        ButtonsOff();
    }

    //Turns off buttons
    public void ButtonsOff()
    {
        chancBtn.SetActive(false);
        presBtn.SetActive(false);

    }
}
