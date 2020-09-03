using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class Settings : MonoBehaviour { 

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;

    void Start() {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
    }

    public void setQuality( int qualityIndex ) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void setFullscreen ( bool isFullscreen ) {
        Screen.fullScreen = isFullscreen;
    }

}

