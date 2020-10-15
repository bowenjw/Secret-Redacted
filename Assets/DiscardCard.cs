using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardCard : MonoBehaviour
{
    public GameObject DiscardText;
    public static bool alreadyDiscarded = false;

    public void OnMouseDown()
    {
        GameObject text = GameObject.Find("DiscardInstructions");
        if (alreadyDiscarded == false)
        {
            Destroy(gameObject);
            alreadyDiscarded = true;
        }
        text.SetActive(false);

    }
}

