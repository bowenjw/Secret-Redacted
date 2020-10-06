using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Execution : MonoBehaviour
{
    public int playerCount = 5;
    public int maxDead = 3;
    private Sprite deathSprite;
    private Button[] buttons;
    private int[] deadPlayers;

    void Start()
    {
        //Grab death sprite
        deathSprite = Resources.Load<Sprite>("deadDefaultBack");

        //Finds buttons and places them in array
        buttons = new Button[playerCount];

        for(int i = 0; i < playerCount; i++)
        {
            buttons[i] = (GameObject.Find("Button " + (i+1)).GetComponent<Button>());
        }

        //Turns off Buttons
        ButtonsOff();

        //Seed dead player with 99, I would use a list but it was giving me hassles so fooey
        deadPlayers = new int[maxDead];
        for(int i = 0; i < maxDead; i++)
        {
            deadPlayers[i] = 99;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Figures out which button is pressed and kills that player
    public void ExecutePlayer(GameObject player)
    {
        //Turns off all buttons
        ButtonsOff();

        //Get player number that is killed
        int playerKilledNumber = Int32.Parse(player.name.Remove(0, 7));

        //Add player to deadPlayers
        HasDied(playerKilledNumber);

        //Change sprite
        SpriteRenderer x = player.GetComponent<SpriteRenderer>();
        x.sprite = deathSprite;




    }

    //Only turns on buttons of alive players
    public void ButtonsOn()
    {
        for(int i = 0; i < playerCount; i++) 
        {
            //Dead players are not turned on
            if (isDead(i))
            {
                buttons[i].interactable = false;
            }
            else
            {
                buttons[i].interactable = true;
            }
        }
    }

    //Turns off buttons, so that you cannot execute whenever
    void ButtonsOff()
    {
        for(int i = 0; i < playerCount; i++) 
        {
            buttons[i].interactable = false;
        }
    }

    //Checks to see if a player index number is dead
    bool isDead(int playerIndex)
    {
        for(int i = 0; i < maxDead; i++)
        {
            if (playerIndex == deadPlayers[i])
                return true;
        }
        return false;
    }

    void HasDied (int playerNumber)
    {
        for(int i = 0; i < maxDead; i++)
        {
            if (deadPlayers[i] == 99)
            {
                deadPlayers[i] = playerNumber - 1;
                return;
            }
        }
    }



}
