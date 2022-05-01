using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;

    public Text resolutionLabel;

    public AudioMixer mixer;

    public Text masterVolumeLabel, musicVolumeLabel, sfxVolumeLabel,sensibilityLabel;
    public Slider masterVolumeSlider, musicVolumeSlider, sfxVolumeSlider, sensibilitySlider;

    private int currentResolution;

    
    void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;

        vsyncToggle.isOn = (QualitySettings.vSyncCount == 0)? false : true;
        
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if ((Screen.currentResolution.height == Screen.resolutions[i].height) && (Screen.currentResolution.width == Screen.resolutions[i].width) )
            {
                currentResolution = i;
                UpdateResolutionLabel();
                
                
            }
        }
        float vol = 0f;
        mixer.GetFloat("MasterVolume",out vol);
        masterVolumeSlider.value = vol;
        masterVolumeLabel.text = Mathf.RoundToInt(masterVolumeSlider.value + 80).ToString() + " %";

        mixer.GetFloat("MusicVolume",out vol);
        musicVolumeSlider.value = vol;
        musicVolumeLabel.text = Mathf.RoundToInt(musicVolumeSlider.value + 80).ToString() + " %";

        mixer.GetFloat("SFXVolume",out vol);
        sfxVolumeSlider.value = vol;
        sfxVolumeLabel.text = Mathf.RoundToInt(sfxVolumeSlider.value + 80).ToString() + " %";


       float sensibility = PlayerPrefs.GetFloat("Sensibility");

       sensibilitySlider.value = sensibility;

       sensibilityLabel.text = Mathf.RoundToInt(sensibility).ToString();

    }

    
    public void UpdateResolutionLabel(){
        resolutionLabel.text = Screen.resolutions[currentResolution].width.ToString() + " x " + Screen.resolutions[currentResolution].height.ToString();
    }

    public void LeftResolution(){
        if (currentResolution > 0)
        {
            currentResolution--;
        }
        UpdateResolutionLabel();
    }
    public void RightResolution(){
        if (currentResolution < Screen.resolutions.Length -1)
        {
            currentResolution++;
        }
        UpdateResolutionLabel();

    }
    public void ApplyGraphicChanges(){
        // Screen.fullScreen = fullscreenToggle.isOn;
        QualitySettings.vSyncCount = (vsyncToggle.isOn)? 1 : 0;
        Screen.SetResolution(Screen.resolutions[currentResolution].width,Screen.resolutions[currentResolution].height,fullscreenToggle.isOn);
        
    }

    public void SetMasterVolume(){
        masterVolumeLabel.text = Mathf.RoundToInt(masterVolumeSlider.value + 80).ToString() + " %";

        mixer.SetFloat("MasterVolume",masterVolumeSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);

    }

     public void SetMusicVolume(){
        musicVolumeLabel.text = Mathf.RoundToInt(musicVolumeSlider.value + 80).ToString() + " %";

        mixer.SetFloat("MusicVolume",musicVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);


    }

     public void SetSFXVolume(){
        sfxVolumeLabel.text = Mathf.RoundToInt(sfxVolumeSlider.value + 80).ToString() + " %";

        mixer.SetFloat("SFXVolume",sfxVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);

    }

    public void SetSensibility(){

       sensibilityLabel.text = Mathf.RoundToInt(sensibilitySlider.value).ToString();

        PlayerPrefs.SetFloat("Sensibility",sensibilitySlider.value);

    }
    

}

[System.Serializable]
public class Resolution {
    public int width, height;
    
}


