using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutiveLookup : MonoBehaviour
{
    public GameObject FascistCard;
    public GameObject LiberalCard;
    public GameObject DrawCardsArea;
    public GameObject LookupText;

    void Start()
    {
        LookupText.SetActive(false);
    }
    public void Lookup()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject policyCard = Instantiate(DrawPolicyCards.deck[0], new Vector3(0, 0, 0), Quaternion.identity);
            policyCard.transform.SetParent(DrawCardsArea.transform, false);
        }

        LookupText.SetActive(true);
    }
}
