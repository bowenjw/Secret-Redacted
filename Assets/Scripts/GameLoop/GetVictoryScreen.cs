using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetVictoryScreen : MonoBehaviour
{

    private GameObject liberalsWin;

    //These pathways get in the way
    private GameObject liberalPath;
    private GameObject fascistPath;

    // Start is called before the first frame update
    void Start()
    {
        liberalsWin = GameObject.Find("LiberalsWin");

        liberalPath = GameObject.Find("Liberal Path");
        fascistPath = GameObject.Find("Fascist Path");



    }

    //Call this if Liberals Win
    public void LiberalVictory() {

        //Brings Win Screen
        liberalsWin.transform.localPosition = new Vector3(0, 0, 0);

        //Moves Pathways out of way
        MovePaths();

    }

    //Loads Menu Scene (Does not do complete reset)
    public void BackToMenu() {
        SceneManager.LoadScene("Main Menue");
    }

    //Moves Pathways out of way
    private void MovePaths() {
        liberalPath.transform.localPosition = new Vector3(2000, 0, 0);
        fascistPath.transform.localPosition = new Vector3(2000, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
