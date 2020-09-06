using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void playGame(){
        SceneManager.LoadScene(1);
    }
    public void quitGame(){ Application.Quit();}

    public void loadSettings(){
        SceneManager.LoadScene(2);
    }
    
    public void loadMainMenue(){
        SceneManager.LoadScene(0);
    }

    public void startCustomGame(){
        SceneManager.LoadScene(3);
    }
}
