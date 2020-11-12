using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameLoop : MonoBehaviour {
    /*
     * Main game loop for the game
     *
    */

    [Header("Buttons")]
    [SerializeField]
    public Button startBtn;

    [Header("Server")]
    [SerializeField]
    public static customLobby.NetworkManagerLobby server;
    
    [Header("Vars")]
    [SerializeField]
    public List<int> prevPresidents;
    public System.Random rand;
    public bool readyToStart = false;
    

    void Start() {
        //Set up all the buttons

        //Set up the server
        server = GameObject.Find("NetworkManager").GetComponent<customLobby.NetworkManagerLobby>();

        //Set up the lists
        prevPresidents = new List<int>();

        //Set up rand 
        rand = new System.Random();

    }

    void Update() {
        mainLoop();
    }

    private void mainLoop() {

        //Check if we are ready
        if (!readyToStart) return;

        //We are now ready and in the main game loop
        
        //Check if we have a president
        if (!server.havePres()) {
            //If we don't have a president get one

            int nextPresIndex = getNewPres();

            //We have found a suitable president

            Mirror.NetworkRoomPlayer x = server.roomSlots[nextPresIndex]; 
            customLobby.RoomPlayer nextPres = (customLobby.RoomPlayer)x;
            nextPres.CmdChangeRole("President");

            //Check if president was assigned 
            if (nextPres.role != "President") return;

            //Add the new president to the prevPresidents list
            prevPresidents.Add(nextPresIndex);
            //Remove the old president so that they can be president again
            if (prevPresidents.Count > 1 ) prevPresidents.RemoveAt(0);

        }

        //Wait for president to select a chancellor

        int cnt = 0;
        foreach ( customLobby.RoomPlayer player in server.roomSlots) {
            if (player.role == "Chancellor") cnt++;
        }
        //No chancellor so return
        if (cnt == 0) return;

        //We have a chancellor
        //TODO: Implement our card draw functions here
            
        Debug.Log("GAMELOOP");
        
    }

    public int getNewPres() {
        int x = rand.Next(server.amtPlayers);
        if (prevPresidents.Contains(x)) {
            return getNewPres();
        }
        else {
            return x;
        }
    }

    public void startGameLoop() {
        //TODO: send message to server to remove everyone's start button
        readyToStart = true;
        startBtn.gameObject.SetActive(false);
        server.playerStarted();
    }

}
