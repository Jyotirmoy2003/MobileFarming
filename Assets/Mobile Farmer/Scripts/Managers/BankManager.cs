using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankManager : MonoBehaviour
{

    [SerializeField] Transform selectItemContainerParent;
    private bool isStillinTrigger = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_GameAssets.PlayerTag))
        {
            isStillinTrigger = true;
            CancelInvoke(nameof(EnterinBank)); // Cancel any previous invokes
            Invoke(nameof(EnterinBank), 1f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_GameAssets.PlayerTag))
        {
            isStillinTrigger = false;
            CancelInvoke(nameof(EnterinBank)); // Cancel the pending enter action
            ExitfromBank();
        }
    }

    void EnterinBank()
    {
        if (!isStillinTrigger) return;

        _GameAssets.Instance.OnViewChangeEvent.Raise(this, true); // Turn off joystick control
        UIManager.Instance.OnUniversalCloseButtonPressed -= CloseButtonPressed; // Ensure no duplicate subscriptions
        UIManager.Instance.OnUniversalCloseButtonPressed += CloseButtonPressed;
        UIManager.Instance.SetCloseButton(true);

        // Bank UI activate
        UIManager.Instance.BankUIActivationStatus(true);
    }

    void ExitfromBank()
    {
        CloseButtonPressed();
    }

    void CloseButtonPressed()
    {
        UIManager.Instance.SetCloseButton(false);
        UIManager.Instance.OnUniversalCloseButtonPressed -= CloseButtonPressed;
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,false); //turn on joystick controll

        //Bank UI deactivate
        UIManager.Instance.BankUIActivationStatus(false);

    }

    public void OnGenerateCodePanelOpen()
    {
        
    }

    
}
