using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [Header("Save")]
    [SerializeField] string saveFileName = "settings.txt";
    private SettingsSave settingsSave = new SettingsSave();
    private string dataPath;

    [Header("ToggleImages")]
    [SerializeField] GameObject sfxOff;
    [SerializeField] GameObject musicOff;

    [Space]
    [Header("Toggle")]
    [SerializeField] Toggle joystickToggle;
    [SerializeField] Toggle hapticToggle;
    [SerializeField] Toggle sfxToggle;
    [SerializeField] Toggle musicToggle;

    [Space]
    [Header("Audio")]
    [SerializeField] AudioMixer audioMixer;
    void Start()
    {
        settingsPanel?.SetActive(false);

        dataPath = Application.persistentDataPath + saveFileName;
        #if UNITY_EDITOR
        dataPath = Application.dataPath + saveFileName;
        #endif
        
        
        settingsSave = new SettingsSave();
        Load();
    }


    public void ToggleHaptic()
    {
        AudioManager.instance.PlaySound("Button");
        HapticManager.Instance.SetHapticStatus(hapticToggle.isOn);

        if(settingsSave == null) settingsSave = new SettingsSave();
        settingsSave.isHaptic = hapticToggle.isOn;

        Save();
    }

    public void ToggleJoystic()
    {
        UIManager.Instance.SetJoysticVisibleStatus(joystickToggle.isOn);
        AudioManager.instance.PlaySound("Button");

        if(settingsSave == null) settingsSave = new SettingsSave();
        settingsSave.isJyositc = joystickToggle.isOn;

        Save();
    }

    public void ToggleSFX()
    {
        sfxOff.SetActive(!sfxToggle.isOn);
        audioMixer.SetFloat("SFX",(sfxToggle.isOn)? 0 : -80);
        AudioManager.instance.PlaySound("Button");

        if(settingsSave == null) settingsSave = new SettingsSave();
        settingsSave.isSFX = sfxToggle.isOn;
        
        Save();
    }


    public void ToggleMusic()
    {
        musicOff.SetActive(!musicToggle.isOn);
        audioMixer.SetFloat("Music",(musicToggle.isOn)? 0 : -80);
        AudioManager.instance.PlaySound("Button");

        if(settingsSave == null) settingsSave = new SettingsSave();
        settingsSave.isMusic = musicToggle.isOn;

        Save();
    }


    public void OnSupportButtonPressed()
    {
        string email = "Jyotiff59@gmail.com"; // Replace with your actual support email
        string subject = EscapeURL("Support Request");
        string body = EscapeURL("Hi, I need help with...");

        string mailto = $"mailto:{email}?subject={subject}&body={body}";

        Application.OpenURL(mailto);
    }

    string EscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }










    void Save()
    {
        SaveAndLoad.Save<SettingsSave>(dataPath,settingsSave);
        
    }

    void Load()
    {
        settingsSave = SaveAndLoad.Load<SettingsSave>(dataPath);

        if(settingsSave != null)
        {
            audioMixer.SetFloat("Music",(musicToggle.isOn)? 0 : -80);
            audioMixer.SetFloat("SFX",(sfxToggle.isOn)? 0 : -80);
            
            if(!settingsPanel) return;
            HapticManager.Instance.SetHapticStatus(settingsSave.isHaptic);
            UIManager.Instance.SetJoysticVisibleStatus(settingsSave.isJyositc);

            hapticToggle.isOn = settingsSave.isHaptic;
            joystickToggle.isOn = settingsSave.isJyositc;

            musicToggle.isOn = settingsSave.isMusic;
            musicOff.SetActive(!musicToggle.isOn);

            sfxToggle.isOn = settingsSave.isSFX;
            sfxOff.SetActive(!sfxToggle.isOn);

        }
    }

   
}

[System.Serializable]
public class SettingsSave
{
    public bool isJyositc;
    public bool isHaptic;
    public bool isSFX;
    public bool isMusic;
}