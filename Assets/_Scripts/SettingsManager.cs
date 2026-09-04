using System;
using FirAnimations;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
#if IS_YANDEX
using YG;
#endif

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public SettingsData data;
    
    public Button RuButton;
    public Button EnButton;

    public Slider SoundSlider;
    public Image SoundOff;
    public MirrorComponent[] Mirrors;
    public Slider MirrorSlider;
    public Image MirrorOff;

    public Toggle AxisToggle;
    public Toggle TimerToggle;
    public TextMeshProUGUI TimerText;

    public Button ResetCubicButton;
    public Button ReviewButton;

    public FirAnimation Settings;
    public FirAnimation Info;
    
    public GameObject AxisGameObject;
    
    public AudioMixer mixer;
    
    public Action<float> OnMirrorChange;

    public void Initialize()
    {
        Instance = this;
        data = SaveLoadSystem<SettingsData>.Load("Settings", new ());

        foreach (MirrorComponent mirror in Mirrors)
            mirror.Initialize();
        SetMixerValues();
        Subscribe();
        AxisGameObject.SetActive(data.IsAxis);
        AxisToggle.isOn = !data.IsAxis;
        TimerText.gameObject.SetActive(data.IsTimer);
        TimerToggle.isOn = !data.IsTimer;
        OnMirrorChange?.Invoke(data.MirrorValue);
    }

    private void Subscribe()
    {
        RuButton.onClick.AddListener(RuLanguage);
        EnButton.onClick.AddListener(EnLanguage);
        
        SoundSlider.onValueChanged.AddListener(value =>
        {
            SoundOff.gameObject.SetActive(value < 0.1);
            if(!SoundOff.gameObject.activeSelf)
                mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
            else
                mixer.SetFloat("SFXVolume", -80);

            data.SFXValue = value;
            SaveLoadSystem<SettingsData>.Save("Settings", data);
        });
        
        MirrorSlider.onValueChanged.AddListener(value =>
        {
            MirrorOff.gameObject.SetActive(value < 0.5f);
            data.MirrorValue = value;
            OnMirrorChange?.Invoke(value);
            SaveLoadSystem<SettingsData>.Save("Settings", data);
        });
        
        AxisToggle.onValueChanged.AddListener(v =>
        {
            AxisGameObject.SetActive(!v);
            data.IsAxis = !v;
            SaveLoadSystem<SettingsData>.Save("Settings", data);
        });
        
        TimerToggle.onValueChanged.AddListener(v =>
        {
            TimerText.gameObject.SetActive(!v);
            data.IsTimer = !v;
            SaveLoadSystem<SettingsData>.Save("Settings", data);
        });
    }

    private void RuLanguage()
    {
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale("ru-RU");
        LocalizationSettings.SelectedLocale = locale;

        data.Language = "ru-RU";
        data.isPlayerLanguage = true;
        SaveLoadSystem<SettingsData>.Save("Settings", data);
    }
    private void EnLanguage()
    {
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale("en");
        LocalizationSettings.SelectedLocale = locale;
            
        data.Language = "en";
        data.isPlayerLanguage = true;
        SaveLoadSystem<SettingsData>.Save("Settings", data);
    }
    private void SetMixerValues()
    {
        SoundOff.gameObject.SetActive(data.SFXValue < 0.1);
        SoundSlider.value = data.SFXValue;
        if(!SoundOff.gameObject.activeSelf)
            mixer.SetFloat("SFXVolume", Mathf.Log10(data.SFXValue) * 20);
        else
            mixer.SetFloat("SFXVolume", -80);
        
        MirrorOff.gameObject.SetActive(data.MirrorValue < 0.5f);
        MirrorSlider.value = data.MirrorValue;
        
        Locale locale;
        if (data.isPlayerLanguage)
        {
            if(data.Language.Equals("ru-RU"))
                locale = LocalizationSettings.AvailableLocales.GetLocale("ru-RU");
            else
                locale = LocalizationSettings.AvailableLocales.GetLocale("en");
        }
        else
        {
#if IS_YANDEX
            if(string.Equals(YG2.lang, "ru"))
                locale = LocalizationSettings.AvailableLocales.GetLocale("ru-RU");
            else
                locale = LocalizationSettings.AvailableLocales.GetLocale("en");
#else
            locale = LocalizationSettings.AvailableLocales.GetLocale("en");
#endif
        }
        
        LocalizationSettings.SelectedLocale = locale;
    }

    public void SettingsSwitch()
    {
        if(Settings.AnimationTime == 1)
            Settings.Reverse();
        else if(Settings.AnimationTime == 0)
            Settings.Play();
    }

    public void InfoSwitch()
    {
        if(Info.AnimationTime == 1)
            Info.Reverse();
        else if(Info.AnimationTime == 0)
            Info.Play();
    }
    
    private void OnDestroy()
    {
        RuButton.onClick.RemoveAllListeners();
        EnButton.onClick.RemoveAllListeners();
        
        SoundSlider.onValueChanged.RemoveAllListeners();
        MirrorSlider.onValueChanged.RemoveAllListeners();
    }
}