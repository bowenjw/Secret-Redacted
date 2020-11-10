using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButtons : MonoBehaviour
{
    private GameObject[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        buttons = new GameObject[100];

        buttons[0] = GameObject.Find("IncrementLiberalButton");
        buttons[1] = GameObject.Find("DecrementLiberalButton");
        buttons[2] = GameObject.Find("ResetLiberalButton");
        buttons[3] = GameObject.Find("IncrementTrackerButton");
        buttons[4] = GameObject.Find("DecrementTrackerButton");
        buttons[5] = GameObject.Find("ResetTrackerButton");
        buttons[6] = GameObject.Find("IncrementFascistButton");
        buttons[7] = GameObject.Find("DecrementFascistButton");
        buttons[8] = GameObject.Find("DecrementFascistButton");
        buttons[9] = GameObject.Find("Execute Button");
        buttons[10] = GameObject.Find("Show Roles Button");
        buttons[11] = GameObject.Find("LookupButton");
        buttons[12] = GameObject.Find("setUsernames");
        buttons[13] = GameObject.Find("callVote");
        buttons[14] = GameObject.Find("hideRoles");
        buttons[15] = GameObject.Find("showRoles");
        buttons[16] = GameObject.Find("DrawPoliciesButton");
        buttons[17] = GameObject.Find("ResetFascistButton");
        buttons[18] = GameObject.Find("presInput");
        buttons[19] = GameObject.Find("chancInput");
        buttons[20] = GameObject.Find("LiberalVictoryButton");

        ButtonsOff();


    }

    public void ButtonsOff() 
    {
        for(int i = 0; i < 21; i++) 
        {
            buttons[i].SetActive(false);
        }
    }

    
    public void ButtonsOn() 
    {
        for(int i = 0; i < 21; i++) 
        {
            buttons[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {       
                   
                ButtonsOn();
                
            }

            if (Input.GetKeyDown(KeyCode.H))
            {       
                   
                ButtonsOff();
                
            }
        }

    }
}
