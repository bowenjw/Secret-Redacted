using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : NetworkBehaviour
{
    public GameObject FascistCard;
    public GameObject LiberalCard;
    public GameObject FascistArea;
    public GameObject LiberalArea;
    public GameObject PlayerArea;
    public GameObject drawButton;
    public GameObject HiddenCard;
    public GameObject CardDraw;

    private List<GameObject> cards = new List<GameObject>();
    private List<GameObject> presidential = new List<GameObject>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        PlayerArea = GameObject.Find("PlayerArea");
        FascistArea = GameObject.Find("FascistArea");
        LiberalArea = GameObject.Find("LiberalArea");
        drawButton = GameObject.Find("DrawButton");
        CardDraw = GameObject.Find("CardDraw");
    }

    [Server]
    public override void OnStartServer()
    {
        deckManager();
    }


    void deckManager()
    {
        cards.Clear();
        for (int i = 0; i < 11; i++)
            cards.Add(FascistCard);
        for (int i = 0; i < 6; i++)
            cards.Add(LiberalCard);

        var shuffled = cards.OrderBy(x => Guid.NewGuid()).ToList();
        cards = shuffled;

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i] == FascistCard)
                Debug.Log("Fascist");
            if (cards[i] == LiberalCard)
                Debug.Log("Liberal");
        }

    }

    [Command]
    public void CmdDealCards()
    {
        if (cards.Count < 3)
            deckManager();

        for (int i = 0; i < 3; i++)
        {
            presidential.Add(cards[i]);
        }

        for (int i = 0; i < 3; i++)
        {
            GameObject playerCard = Instantiate(cards[0], new Vector2(0, 0), Quaternion.identity);
            GameObject HiddenCard = Instantiate(cards[0], new Vector2(0, 0), Quaternion.identity);
            NetworkServer.Spawn(playerCard, connectionToClient);
            RpcShowCard(playerCard, "Dealt");

            cards.RemoveAt(0);
        }
        drawButton.gameObject.SetActive(false);
        NetworkServer.UnSpawn(drawButton);


    }

    [ClientRpc]
    void RpcShowCard(GameObject playerCard, string type)
    {
        SceneManager.LoadScene("CardDraw");
        if (type == "Dealt")
        {
            if (hasAuthority)
            {
                playerCard.transform.SetParent(CardDraw.transform, false);
                playerCard.GetComponent<cardFlipper>().Flip();
            }
            else
            {
                playerCard.transform.SetParent(CardDraw.transform, false);
                playerCard.GetComponent<cardFlipper>().Flip();
                playerCard.GetComponent<cardFlipper>().Flip();
            }
        }
    }

}
