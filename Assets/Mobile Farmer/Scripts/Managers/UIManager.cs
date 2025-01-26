using System;
using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject treeModePanel;
    [SerializeField] GameObject treeButton;
    [SerializeField] GameObject toolContainer;
    [SerializeField] Button IntreactButton;

    

    void Awake()
    {
        base.Awake();
        PlayerDetector.OnEnterTreezone += EnteredAppleTreeCallback;
        PlayerDetector.OnExitTreezone += ExitAppleTreeZoneCallback;
    }
    void OnDestroy()
    {
        base.OnDestroy();
        PlayerDetector.OnEnterTreezone -= EnteredAppleTreeCallback;
        PlayerDetector.OnExitTreezone -= ExitAppleTreeZoneCallback;
    }
    void Start()
    {
        treeButton.SetActive(false);
        treeModePanel.SetActive(false);
    }



    private void EnteredAppleTreeCallback(AppleTree appleTree)
    {
        treeButton.SetActive(true);
        toolContainer.SetActive(false);
    }

    private void ExitAppleTreeZoneCallback(AppleTree appleTree)
    {
        treeButton.SetActive(false);
        toolContainer.SetActive(true);
    }

    public void SetViewMode(bool isTree)
    {
        gamePanel.SetActive(!isTree);
        treeModePanel.SetActive(isTree);
        
    }


    public void ListenToTreeModeStartEvent(Component sender,object data)
    {
        SetViewMode((bool)data);
    }

    public void SetupIntreactButton(ButtonInfo buttonInfo,bool isActive)
    {
        IntreactButton.gameObject.SetActive(isActive);
        IntreactButton.image.sprite = buttonInfo.sprite;
    }
    
}
