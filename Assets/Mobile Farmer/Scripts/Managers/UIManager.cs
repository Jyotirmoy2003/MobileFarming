using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject treeButton;
    [SerializeField] GameObject toolContainer;

    void Awake()
    {
        PlayerDetector.OnEnterTreezone += EnteredAppleTreeCallback;
        PlayerDetector.OnExitTreezone += ExitAppleTreeZoneCallback;
    }
    void OnDestroy()
    {
        PlayerDetector.OnEnterTreezone -= EnteredAppleTreeCallback;
        PlayerDetector.OnExitTreezone -= ExitAppleTreeZoneCallback;
    }
    void Start()
    {
        
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

    
}
