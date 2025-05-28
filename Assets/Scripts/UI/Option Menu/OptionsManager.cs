using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System.Collections.Generic;

public class OptionsManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject graphicsPanel;
    public GameObject controlsPanel;
    public GameObject AudioPanel;

    [Header("Audio Mixers")]
    // Audio settings
    public AudioMixer audioMixer;

    [Header("UI Elements")]
    // Graphics settings
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    // Volume Sliders
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private PlayerVals playerThatPaused;
    Resolution[] resolutions;

    private void Start()
    {
        // Initialize resolution options
        InitializeResolutionSettings();

        // Initialize quality options
        InitializeQualitySettings();

        // Initialize fullscreen setting
        InitializeFullscreenSetting();

        // Initialize panels to be inactive
        graphicsPanel.SetActive(true);
        AudioPanel.SetActive(false);

        // Initialize UI elements
        InitializeAudioSettings();
    }

    private void InitializeResolutionSettings()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);

        // Load saved resolution or use current
        int savedResIndex = PlayerPrefs.GetInt("ResolutionIndex", currentResolutionIndex);
        if (savedResIndex < resolutions.Length)
        {
            resolutionDropdown.value = savedResIndex;
            SetResolution(savedResIndex);
        }
    }

    private void InitializeQualitySettings()
    {
        if (qualityDropdown != null)
        {
            qualityDropdown.ClearOptions();
            qualityDropdown.AddOptions(new List<string>(QualitySettings.names));

            // Load saved quality or use current
            int savedQuality = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
            qualityDropdown.value = savedQuality;
            QualitySettings.SetQualityLevel(savedQuality);
        }
    }

    private void InitializeFullscreenSetting()
    {
        if (fullscreenToggle != null)
        {
            // Load saved fullscreen preference, default to current state if not saved
            bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;
            fullscreenToggle.isOn = savedFullscreen;
            Screen.fullScreen = savedFullscreen;
        }
    }

    private void InitializeAudioSettings()
    {
        if (masterVolumeSlider != null)
        {
            float masterVolume = PlayerPrefs.GetFloat("master", 1f);
            masterVolumeSlider.value = masterVolume;
            SetMasterVolume(masterVolume);
        }

        if (musicVolumeSlider != null)
        {
            float musicVolume = PlayerPrefs.GetFloat("music", 1f);
            musicVolumeSlider.value = musicVolume;
            SetMusicVolume(musicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            float sfxVolume = PlayerPrefs.GetFloat("SFX", 1f);
            sfxVolumeSlider.value = sfxVolume;
            SetSFXVolume(sfxVolume);
        }
    }

    public void ShowGraphicsPanel()
    {
        if (graphicsPanel != null && AudioPanel != null)
        {
            graphicsPanel.SetActive(true);
            controlsPanel.SetActive(false);
            AudioPanel.SetActive(false);
        }
    }

    public void ShowControlsPanel()
    {
        if (graphicsPanel != null && controlsPanel != null && AudioPanel != null)
        {
            controlsPanel.SetActive(true);
            graphicsPanel.SetActive(false);
            AudioPanel.SetActive(false);
        }
    }

    public void ShowAudioPanel()
    {
        if (graphicsPanel != null && AudioPanel != null)
        {
            AudioPanel.SetActive(true);
            graphicsPanel.SetActive(false);
            controlsPanel.SetActive(false);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetControlStickSensitivity(float sensitivity)
    {
        if (playerThatPaused != null)
        {
            playerThatPaused.setSpeedSensitivityMultiplier(sensitivity);
        }
    }

    public void SetMasterVolume(float volume)
    {
        if (audioMixer != null)
        {
            // If volume is 0, set to -80dB (effectively silent)
            float dB = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
            audioMixer.SetFloat("master", dB);
            PlayerPrefs.SetFloat("master", volume);
            PlayerPrefs.Save();
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (audioMixer != null)
        {
            // If volume is 0, set to -80dB (effectively silent)
            float dB = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
            audioMixer.SetFloat("music", dB);
            PlayerPrefs.SetFloat("music", volume);
            PlayerPrefs.Save();
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (audioMixer != null)
        {
            // If volume is 0, set to -80dB (effectively silent)
            float dB = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
            audioMixer.SetFloat("SFX", dB);
            PlayerPrefs.SetFloat("SFX", volume);
            PlayerPrefs.Save();
        }
    }

    public void SetPlayerThatPaused(PlayerVals player) => playerThatPaused = player;
}