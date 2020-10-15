using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShowRoles : MonoBehaviour
{

    public int playerCount = 5;
    private GameObject[] buttons;
    Roles Roles;

    //Gets ducks in a row
    void Start()
    {
        //Finds buttons and places them in array
        buttons = new GameObject[playerCount];

        for(int i = 0; i < playerCount; i++)
        {
            buttons[i] = GameObject.Find("Show Party Button " + (i+1));
        }

        //Turn off buttons
        ButtonsOff();

        //Gets prepared to see Roles script
        Roles = GameObject.Find("RolesHolder").GetComponent<Roles>();
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    //Button pressed, show player party
    public void ButtonPress(GameObject player) {
    
        //Turns off buttons
        ButtonsOff();
        
        //Show Party of player
        int playerIndex = Int32.Parse(player.name.Remove(0, 7)) - 1;
        Roles.showParty(playerIndex);

        //Waits 5 seconds then hides roles
        Invoke("HideRoles", 5);

    }

    //Turns on buttons
    public void ButtonsOn()
    {
        for(int i = 0; i < playerCount; i++) 
        {
            buttons[i].SetActive(true);
        }
    }


    //Turns off buttons
    public void ButtonsOff()
    {
        for(int i = 0; i < playerCount; i++) 
        {
            buttons[i].SetActive(false);
        }
    }

    //Hides all roles
    private void HideRoles()
    {
        Roles.hideRoles();
    }
}
