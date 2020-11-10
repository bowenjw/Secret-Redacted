using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetVictoryScreen : MonoBehaviour
{

    //The win screens
    private GameObject liberalsWin;
    private GameObject fascistsWin;

    //These pathways get in the way
    private GameObject liberalPath;
    private GameObject fascistPath;

    private Roles Roles;


    //Find game objects
    void Start()
    {
        liberalsWin = GameObject.Find("LiberalsWin");
        fascistsWin = GameObject.Find("FascistsWin");

        liberalPath = GameObject.Find("Liberal Path");
        fascistPath = GameObject.Find("Fascist Path");

        Roles = GameObject.Find("RolesHolder").GetComponent<Roles>();


    }

    //Call this if Liberals Win
    public void LiberalVictory() {

        //Brings Win Screen
        liberalsWin.transform.localPosition = new Vector3(0, 0, 0);

        //Moves Pathways out of way
        MovePaths();

        //Shows roles
        Roles.showRoles(); 

    }

    //Call this if Fascists Win
    public void FascistVictory() {

        //Brings Win Screen
        fascistsWin.transform.localPosition = new Vector3(0, 0, 0);

        //Moves Pathways out of way
        MovePaths();

        //Shows roles
        Roles.showRoles(); 

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

}
