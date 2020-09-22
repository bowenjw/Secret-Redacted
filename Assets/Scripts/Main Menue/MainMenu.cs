using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    private string overviewScene = "overview";

    public void Start(){
        int qualityIndex = PlayerPrefs.GetInt("Quality",0);
        QualitySettings.SetQualityLevel(qualityIndex);

        int isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1);
        if (isFullscreen == 1) {
            Screen.fullScreen = true;
        }
        else {
            Screen.fullScreen = false;
        }
    }

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

    public void loadOverview() {
        SceneManager.LoadScene(overviewScene);
    }

    public void startCustomGame(){
        SceneManager.LoadScene(3);
    }
}
