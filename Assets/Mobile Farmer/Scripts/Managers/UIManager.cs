using System;
using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject shakeModePanel;
    [SerializeField] GameObject toolContainer;
    [Header("Interact Button")]
    [SerializeField] Button IntreactButton;
    [SerializeField] Image interactButtonIcon;

    [Header("Shake Silder")]
    [SerializeField] Slider shakeSlider;

    

    // void Awake()
    // {
    //     base.Awake();
    //     PlayerDetector.OnEnterTreezone += EnteredAppleTreeCallback;
    //     PlayerDetector.OnExitTreezone += ExitAppleTreeZoneCallback;
    // }
    // void OnDestroy()
    // {
    //     base.OnDestroy();
    //     PlayerDetector.OnEnterTreezone -= EnteredAppleTreeCallback;
    //     PlayerDetector.OnExitTreezone -= ExitAppleTreeZoneCallback;
    // }
    void Start()
    {
        shakeModePanel.SetActive(false);
        IntreactButton.gameObject.SetActive(false);
    }



    

   

    public void SetViewMode(bool isTree)
    {
        gamePanel.SetActive(!isTree);
        shakeModePanel.SetActive(isTree);
        
    }


    public void ListenToShakeModeStartEvent(Component sender,object data)
    {
        SetViewMode((bool)data);
    }

    public void SetupIntreactButton(ButtonInfo buttonInfo,bool isActive)
    {
        IntreactButton.gameObject.SetActive(isActive);
        interactButtonIcon.sprite = buttonInfo.sprite;
    }


    public void UpdateShakeSlider(float value)
    {
        shakeSlider.value = value;
    }
    
}
