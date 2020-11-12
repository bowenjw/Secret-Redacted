using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

Desc: Script controls logic for fascist path, 5-6 players
TODO: Implement logic for gameplay changes

*/

public class FascistPathFiveSixLogic : MonoBehaviour
{
    /*
    overflowPrevention keeps track of the current board state as follows
    0: Empty board
    1: One fascist card
    2: Two fascist card
    3: Three fascist card, president peeks the deck, red zone begins
    4: Four fascist card, President executes
    5: Five fascist card, President executes and veto power begins
    6: Six fascist card, facists win

    */
    private SpriteRenderer sprite;
    int overflowPreventation = 0;
    

    //Grabs current sorting order for fascist cards, each are done in a decsending order, 0, -1, -2 etc
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
 
    //Increments all fascist cards, allowing one card at a time to be seen, overflow prevents overpressing the button.
    public void IncrementFascistPath()
    {
       if (overflowPreventation < 6) 
       {
           sprite.sortingOrder++;
           overflowPreventation++;
           CheckGameState();
       }
    }

    //Decrements all fascist cards, getting rid of one card in case it is needed.
    public void DecrementFascistPath()
    {
       if (overflowPreventation > 0)
       {
           sprite.sortingOrder--;
           overflowPreventation--;
       }
           
    }

    //Resets fascist board
    public void ResetFascistPath()
    {
        Debug.Log("");
        while (overflowPreventation != 0)
            DecrementFascistPath();
    }

    //Checks overflowPrevention only after increments to see current gamestate
    private void CheckGameState()
    {
        switch (overflowPreventation)
        {
            case 3:
            //Apply president peek at next 3 cards and red zone beggining here
            Debug.Log("President peeks at the next 3 cards. Red zone also begins!");
            break;

            case 4:
            //Apply president executes
            Debug.Log("President executes someone!");
            break;

            case 5:
            //Apply president executes and veto begins
            Debug.Log("President executes someone! Vetoing is now live!");
            break;

            case 6:
            //Fascists win
            Debug.Log("Fascists Win!");
            break;

            default:
            //Nothing special
            Debug.Log("Just a regular facist card");
            break;
        }

    }


    

 }


