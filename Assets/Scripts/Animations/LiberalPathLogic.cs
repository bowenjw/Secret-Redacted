using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

Desc: Script controls logic for liberal path, should work for any player game. Similar logic will be used on facist path.
TODO: Implement logic to Election Tracker

*/

public class LiberalPathLogic : MonoBehaviour
{
    //overflowPreventation handles the main board, trackeroverflowPreventation handles the election tracker
    private SpriteRenderer sprite;
    int overflowPreventation = 0;
    //int trackerOverflowPreventation = 0;

    //Grabs current sorting order for liberal cards, each are done in a decsending order, 0, -1, -2 etc
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
 
    //Increments all liberal cards, allowing one card at a time to be seen, overflow prevents overpressing the button.
    public void IncrementLiberalPath()
    {
       if (overflowPreventation < 5) 
       {
           sprite.sortingOrder++;
           overflowPreventation++;
       }
    }

    //Decrements all liberal cards, getting rid of one card in case it is needed.
    public void DecrementLiberalPath()
    {
       if (overflowPreventation > 0)
       {
           sprite.sortingOrder--;
           overflowPreventation--;
       }
           
    }

    //Resets liberal board
    public void ResetLiberalPath()
    {
        while (overflowPreventation != 0)
            DecrementLiberalPath();
    }


    

 }


