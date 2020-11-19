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

    //Kirt's additions
    private Sprite facistPartySprite;
    private Sprite liberalPartySprite;
    private Sprite churchillSprite;

    void Start()
    {
        liberalSprites = Resources.LoadAll<Sprite>("Roles/Liberal");
        facistSprites = Resources.LoadAll<Sprite>("Roles/Fascist");
        hitlerSprite = Resources.Load<Sprite>("Roles/Hitler/Hitler");
        churchillSprite = Resources.Load<Sprite>("Roles/Churchill/ChurchillPrototype");
        defaultBack = Resources.Load<Sprite>("defaultBack");
        facistPartySprite = Resources.Load<Sprite>("PartyMembershipFascist");
        liberalPartySprite = Resources.Load<Sprite>("PartyMebershipLiberal");

        children = new GameObject[players];

        for (int i = 0; i < players; i++) {

            children[i] = GameObject.Find("Player " + (i+1));

        }

    }

    public void generateRoles() {
        players = (int)(GameObject.Find("NetworkManager").GetComponent<customLobby.NetworkManagerLobby>().roomSlots.Count());
        playerRoles = new string[players];
        var rand = new System.Random();

        int maxFash = amtFash[players % 5];
        int[] fashIndex = new int[maxFash];

        if (players < 5) maxFash = 2;

        for (int i = 0; i < maxFash; i++) {
            fashIndex[i] = rand.Next(players); // rand goes between 0 - players 
        }

        //Loop gets random player, makes sure they are not fash and makes them Churchill
        bool churchillAssigned = false;
        int churchill = -1;

        while(!churchillAssigned) {
            
            churchill = rand.Next(players);

            if(fashIndex.Contains(churchill)) {

            }
            
            else {
                churchillAssigned = true;
            }
        }

        for (int i = 0; i < players; i++) {

            if ( fashIndex.Contains(i) ) {
                // player is fash 
                playerRoles[i] = "Fascist";
            }

            else if (i == churchill) {
                playerRoles[i] = "Churchill";
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
            else if (playerRoles[i] == "Churchill") {
                x.sprite = churchillSprite;
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

    //shows complete roles given player index (0 to 4)
    public void showRole(int playerIndex) {



            SpriteRenderer x = children[playerIndex].GetComponent<SpriteRenderer>();
            
            if (playerRoles[playerIndex] == "Liberal") { //Is Liberal
                x.sprite = liberalSprites[playerIndex]; 
            }

            else if (playerRoles[playerIndex] == "Fascist") { //Is Fascist
                x.sprite = facistSprites[1];
            }

            else if (playerRoles[playerIndex] == "Churchill") { //Is Churchill
                x.sprite = churchillSprite;
            }
            else { //Is Hitler
                x.sprite = hitlerSprite;
            }
            children[playerIndex].SetActive(false);
        
            children[playerIndex].SetActive(true);
        
    }


    //Shows all liberal player Roles, including Churchill
    public void showLiberalRoles() {

            for (int i = 0; i < players; i++) {
                if ((playerRoles[i] == "Liberal") || (playerRoles[i] == "Churchill"))
                    showRole(i);
            }

    }

    //Shows all facist player Roles, including Hitler
    public void showFascistRoles() {

            for (int i = 0; i < players; i++) {
                if ((playerRoles[i] == "Fascist") || (playerRoles[i] == "Hitler"))
                    showRole(i);
            }

    }

    

    
    //Shows Party (Facist or Liberal) for show role executive action, protects which facist is hitler
    //and which liberal is churchill
     public void showParty(int playerIndex) {

            SpriteRenderer x = children[playerIndex].GetComponent<SpriteRenderer>();

            //Is liberal
            if ((playerRoles[playerIndex] == "Liberal") || (playerRoles[playerIndex] == "Churchill")) {
                x.sprite = liberalPartySprite; 
            }
            
            //Is facist
            else {
                x.sprite = facistPartySprite;
            }
            
            children[playerIndex].SetActive(false);


            children[playerIndex].SetActive(true);
        
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

    //Hides role of the player index passed
    public void hideRole(int playerIndex) {

        
        SpriteRenderer x = children[playerIndex].GetComponent<SpriteRenderer>();
        x.sprite = defaultBack; 


        children[playerIndex].SetActive(false);
        children[playerIndex].SetActive(true);
        

    }
        


}




