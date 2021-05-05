using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audio_Mixer;

    public Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;

        List<string> options = new List<string>();

        int current_Res_Index = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                current_Res_Index = i;
            }
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = current_Res_Index;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        audio_Mixer.SetFloat(Tags.VOLUME_TAG, volume);
    }

    public void SetQuality(int quality_Index)
    {
        QualitySettings.SetQualityLevel(quality_Index);
    }

    public void SetScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    }

    public void SetRes(int res_Index)
    {
        Resolution resolution = resolutions[res_Index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
