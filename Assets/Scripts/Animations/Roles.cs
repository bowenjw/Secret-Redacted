using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Roles : MonoBehaviour {

    public int players = 5; 
    public string[] playerRoles;
    public int[] amtFash = {2,2,3,3,4,4};
    private Sprite[] liberalSprites;
    private Sprite[] facistSprites;
    private Sprite hitlerSprite;
    private Sprite defaultBack;
    private GameObject[] children;

    void Start()
    {
        liberalSprites = Resources.LoadAll<Sprite>("Roles/Liberal");
        facistSprites = Resources.LoadAll<Sprite>("Roles/Fascist");
        hitlerSprite = Resources.Load<Sprite>("Roles/Hitler/Hitler");
        defaultBack = Resources.Load<Sprite>("defaultBack");

        //TODO: get # of players
        playerRoles = new string[players];
        var rand = new System.Random();

        int maxFash = amtFash[players % 5];
        int[] fashIndex = new int[maxFash];

        for (int i = 0; i < maxFash; i++) {
            fashIndex[i] = rand.Next(players); // rand goes between 0 - players 
        }

        children = new GameObject[players];

        for (int i = 0; i < players; i++) {

            children[i] = GameObject.Find("Player " + (i+1));

            if ( fashIndex.Contains(i) ) {
                // player is fash 
                playerRoles[i] = "Fascist";
            }
            else {
                playerRoles[i] = "Liberal";
            }
        }
        playerRoles[fashIndex[0]] = "Hitler"; // first fash becomes hitler

    }

    public void showRoles() {

        int facistCnt = 0;

        for (int i = 0; i < players; i++) {
            SpriteRenderer x = children[i].GetComponent<SpriteRenderer>();
            if (playerRoles[i] == "Liberal") {
                x.sprite = liberalSprites[i]; 
            }
            else if (playerRoles[i] == "Fascist") {
                x.sprite = facistSprites[facistCnt];
                facistCnt++;
            }
            else {
                // is hitler
                x.sprite = hitlerSprite;
            }
            children[i].SetActive(false);
        }
        for (int i = 0; i < players; i++) {
            children[i].SetActive(true);
        }
        
    }

    public void hideRoles() {

        for (int i = 0; i < players; i++) {
            SpriteRenderer x = children[i].GetComponent<SpriteRenderer>();
            x.sprite = defaultBack; 
            children[i].SetActive(false);
            }
        for (int i = 0; i < players; i++) {
            children[i].SetActive(true);
        }

    }
        


}





