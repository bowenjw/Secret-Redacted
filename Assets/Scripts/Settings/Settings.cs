using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using System.Linq;

public class Settings : MonoBehaviour { 

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;

    void Start() {
        resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + " x " + resolutions[i].height;
            /*
            if (options.Contains(option)){
                continue;
            }
            */
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height ) {
                currentResolutionIndex = i;
            }
        }


        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void setResolution (int resolutionIndex) {

        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void setQuality( int qualityIndex ) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void setFullscreen ( bool isFullscreen ) {
        Screen.fullScreen = isFullscreen;
    }

}

