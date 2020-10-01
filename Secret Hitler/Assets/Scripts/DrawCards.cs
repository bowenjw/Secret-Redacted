using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class DrawCards : NetworkBehaviour

{
    public PlayerManager PlayerManager;

    public void OnClick()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        
        PlayerManager.CmdDealCards();
    }



    public void ExitGame()
    {
        Application.Quit();
    }
}
