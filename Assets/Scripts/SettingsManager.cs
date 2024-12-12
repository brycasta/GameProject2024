using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




//Fanial W. (SettingsManager)


public class SettingsManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Volume Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    private void Awake()
    {
        // Load saved volumes or set default values
        masterSlider.value = PlayerPrefs.GetFloat(MasterVolumeKey, 0.75f); // Default 75%
        musicSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey, 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat(SFXVolumeKey, 0.75f);

        // Apply volumes to the Audio Mixer
        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        // Add listeners to sliders
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20); // Convert linear to logarithmic scale
        PlayerPrefs.SetFloat(MasterVolumeKey, volume); // Save to PlayerPrefs
    }

    private void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    private void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
    }

    private void OnDestroy()
    {
        // Save preferences when the settings screen is destroyed
        PlayerPrefs.Save();
    }

    public void BackToMainMenu()
    {
        // Logic to navigate back to the main menu

        SceneManager.LoadSceneAsync(0);
        Debug.Log("Returning to main menu...");
    }
}
