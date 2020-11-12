using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Overview : MonoBehaviour {

    private string liberalScene = "liberalRole";
    private string fashScene = "fashRole";
    private string hitlerScene = "hitlerRole";
    private string overviewScene = "overview";
    private string executiveActionScene = "executiveAction";
    private string legistlativeSessionScene = "legistlativeSession";
    private string electionScene = "election";

    public void loadOverview() {
        SceneManager.LoadScene(overviewScene);
    }

    public void loadLiberal() {
        SceneManager.LoadScene(liberalScene);
    }

    public void loadFascist() {
        SceneManager.LoadScene(fashScene);
    }

    public void loadHitler() {
        SceneManager.LoadScene(hitlerScene);
    }

    public void loadElection() {
        SceneManager.LoadScene(electionScene);
    }

    public void loadExecutive() {
        SceneManager.LoadScene(executiveActionScene);
    }

    public void loadLegistlative() {
        SceneManager.LoadScene(legistlativeSessionScene);
    }
}
