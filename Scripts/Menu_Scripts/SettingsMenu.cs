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
        } // Create a list with every supported resolution available on the current system

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        // Load the list "options" in the resolution dropdown

        resolutionDropdown.value = current_Res_Index;
        resolutionDropdown.RefreshShownValue();
        // Make the current resolution to be shown in the dropdown
    }

    public void SetVolume(float volume)
    {
        audio_Mixer.SetFloat(Tags.VOLUME_TAG, volume);
    } // Sets the value for volume in the master of the game mixer
    // Needs a mixer and all autio sources to have the output device set to it

    public void SetQuality(int quality_Index)
    {
        QualitySettings.SetQualityLevel(quality_Index);
    } // Sets the quality for the game
    // Needs various quality levels to be set from Project Settings -> Quality

    public void SetScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
    } // Sets screen to full screen

    public void SetRes(int res_Index)
    {
        Resolution resolution = resolutions[res_Index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    } // Sets the resolution of the aplication
}
