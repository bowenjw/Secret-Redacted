using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CustomGame : MonoBehaviour {
    public Slider liberalCardsSlider;
    public Slider amtPlayersSlider;
    public Slider policiesReqSlider;
    public Toggle policyReportingToggle;

    public TMP_Text liberalCardsText;
    public TMP_Text amtPlayersText;
    public TMP_Text policiesReqText;

    [SerializeField] public int liberalCards {get;set;}
    [SerializeField] public int amtPlayers {get;set;}
    [SerializeField] public int policiesReq {get;set;}
    [SerializeField] public bool isPolicyReportingReq {get;set;}

    [SerializeField] public GameObject settings;

    public void setLiberalCardsText(float amt){
        liberalCardsText.text = string.Format("{0}",amt);
    }

    public void setAmtPlayersText(float amt){
        amtPlayersText.text = string.Format("{0}",amt);
    }

    public void setPoliciesReqText(float amt){
        policiesReqText.text = string.Format("{0}",amt);
    }

    public void loadCustomGame(){
        settings = GameObject.Find("Settings");
        CustomGame x = settings.GetComponent<CustomGame>();
        x.liberalCards = ((int)liberalCardsSlider.value);
        x.amtPlayers = ((int)amtPlayersSlider.value);
        x.policiesReq = ((int)policiesReqSlider.value);
        x.isPolicyReportingReq = (policyReportingToggle.isOn);
        DontDestroyOnLoad(settings.gameObject);
        //customSettings.testVals();
    } 
}












