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
    
    [Header("Interact Button")]
    [SerializeField] Button IntreactButton;
    [SerializeField] Image interactButtonIcon;

    [Header("Shake Silder")]
    [SerializeField] Slider shakeSlider;
    [SerializeField] Button shakeExitButton;

    public Action OnExitButtonPressed;

    
    void Start()
    {
        shakeModePanel.SetActive(false);
        IntreactButton.gameObject.SetActive(false);
    }



    

   

   


    public void ListenToShakeModeStartEvent(Component sender,object data)
    {
        shakeModePanel.SetActive((bool)data);
    }
    public void ListenToViewChangedEvent(Component sender,object data)
    {
        gamePanel.SetActive(!(bool)data);
    }

    public void SetupIntreactButton(ButtonInfo buttonInfo,bool isActive)
    {
        IntreactButton.gameObject.SetActive(isActive);
        interactButtonIcon.sprite = buttonInfo.sprite;
    }

    public void ShowShakeExit(bool val)
    {
        shakeExitButton.gameObject.SetActive(val);
    }


    public void UpdateShakeSlider(float value)
    {
        shakeSlider.value = value;
    }
    
    public void OnExitButtonPressedCallback()
    {
        OnExitButtonPressed?.Invoke();
    }
}
