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


    private GameObject chancBtn;
    private GameObject presBtn;
    private bool needVeto = false;


    //Finds buttons and turns them off
    void Start()
    {
        //Find Chancellor and President Buttons
        presBtn = GameObject.Find("President Veto Button");
        chancBtn = GameObject.Find("Chancellor Veto Button");


        ButtonsOff();
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
