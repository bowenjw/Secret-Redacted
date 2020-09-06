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

    public CustomGameSettings customSettings = new CustomGameSettings();

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
        customSettings.setLiberalCards((int)liberalCardsSlider.value);
        customSettings.setAmtPlayers((int)amtPlayersSlider.value);
        customSettings.setPoliciesReq((int)policiesReqSlider.value);
        customSettings.setPolicyReporting(policyReportingToggle.isOn);
        //customSettings.testVals();
    } 
}

public class CustomGameSettings {
        private int liberalCards;
        private int amtPlayers;
        private int policiesReq;
        private bool isPolicyReportingReq;

        public void setLiberalCards(int x){
            liberalCards = x;
        }
        
        public void setAmtPlayers(int x){
            amtPlayers = x;
        }
        
        public void setPoliciesReq(int x){
            policiesReq = x;
        }

        public void setPolicyReporting(bool x){
            isPolicyReportingReq = x;
        }

        public void testVals(){
            Debug.Log("liberalCards:" + liberalCards + "\nPlayers" + amtPlayers + "\nPolicies req:" + policiesReq + "\npolicyReporting:" + isPolicyReportingReq);
        }
}












