using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider SFXSlider;
    public Slider gunSlider;

    public void Start()
    {
        audioMixer.GetFloat("Music", out float musicValueForSlider);
        musicSlider.value = musicValueForSlider;

        audioMixer.GetFloat("SFX", out float SFXValueForSlider);
        SFXSlider.value = SFXValueForSlider;

        audioMixer.GetFloat("Gun", out float gunValueForSlider);
        gunSlider.value = gunValueForSlider;

    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", volume);
    }

    public void SetGunVolume(float volume)
    {
        audioMixer.SetFloat("Gun", volume);
    }

    public void ClearSavedData()
    {
        PlayerPrefs.DeleteAll();
    }
}