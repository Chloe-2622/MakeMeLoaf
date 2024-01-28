using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Win Menu")]

    [Header("Main Menu")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button SettingsButton;
    [SerializeField] private Button QuitButton;


    [Header("Settings Menu")]
    [SerializeField] private GameObject SettingsMenu;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider GeneralSlider;
    [SerializeField] private Slider EffectSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private TMP_Dropdown ResolutionDrop;
    [SerializeField] private TMP_Dropdown QualityDrop;
    [SerializeField] private Toggle Toggle;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button ApplyButton;
    private Resolution[] resolutions;



    public void Start()
    {

        PlayButton.onClick.AddListener(StartAction);
        SettingsButton.onClick.AddListener(ActiveSettingsUI);
        QuitButton.onClick.AddListener(QuitAction);

        BackButton.onClick.AddListener(ActiveMainUI);
        ApplyButton.onClick.AddListener(SetSettings);

    }

    [ContextMenu("ActiveMainsUI")]
    public void ActiveMainUI()
    {
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void StartAction()
    {
        SettingsMenu.SetActive(false);
        MainMenu.SetActive(false);
    }

    public void QuitAction()
    {
        Application.Quit();
    }

    [ContextMenu("ActiveSettingsUI")]
    public void ActiveSettingsUI()
    {
        MainMenu.SetActive(false);
        SettingsMenu.SetActive(true);

        resolutions = Screen.resolutions;

        ResolutionDrop.ClearOptions();
        List<String> options = new List<string>();
        int currentRes = 0;
        for(var i =0; i<resolutions.Length;i++)
        {
            Resolution res = resolutions[i];
            options.Add(res.width + " x " + res.height + " " + Mathf.Round((int)res.refreshRateRatio.value)+" Hz");
            if(res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
            {
                currentRes = i;
            }
        }

        ResolutionDrop.AddOptions(options);
        ResolutionDrop.value = currentRes;
        ResolutionDrop.RefreshShownValue();
    }

    [ContextMenu("SetSettings")]
    private void SetSettings()
    {
        mixer.SetFloat("MainVolume", LinToDb(GeneralSlider.value));
        mixer.SetFloat("EffectVolume", LinToDb(EffectSlider.value));
        mixer.SetFloat("MusicVolume", LinToDb(MusicSlider.value));
        QualitySettings.SetQualityLevel(QualityDrop.value);
        Resolution res = Screen.resolutions[ResolutionDrop.value];
        Screen.SetResolution(res.width, res.height, Toggle.isOn);
        Debug.Log(res);
    }

    private float LinToDb(float x)
    {
        return 20 * Mathf.Log10(x);
    }


}
