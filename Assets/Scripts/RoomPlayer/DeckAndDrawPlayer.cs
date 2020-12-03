using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using customLobby;
using Mirror;
using TMPro;

//Will handle deck and hand for individual player, helping with authority sense
public class DeckAndDrawPlayer : NetworkBehaviour
{
    DeckAndDraw deck;

 


    //Player's index, it is set from RoomPlayer gameStarted() method
    int index = -1;




    // Start is called before the first frame update
    void Start()
    {
        //Turns off discard buttons
        //ButtonsOff();

        deck = GameObject.Find("DeckAndDraw").GetComponent<DeckAndDraw>();


        index = gameObject.GetComponent<RoomPlayer>().index;

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Client calls this to initiate president and chancellor card draw, discard and play. //STARTS EVERYTHING
    public void GetPolicy() {

        deck.GetPolicy(index, 0 , 1);

    }


   

   



  

    

    


    /************************ClientRPC***********************************/
    
    //Makes all client send their index, president, and chanc index to their deck to see if they can draw as president ////////////////////////////////////////////////////////////////////SET PRESIDENT AND CHANCELLOR FOR REAL
    [ClientRpc]
    private void RpcGetPolicy() {
        deck.GetPolicy(index, 0 , 1);
    }



    /************************COMMANDS**************************/
    
    

}
