using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseChunk : MonoBehaviour
{
    [SerializeField] Transform horseTranform;

    [Header("Camera")]
    [SerializeField] Vector3 cameraBodyOffset;
    [SerializeField] Vector3 cameraAimOffset;
    private bool isStillinTrigger;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_GameAssets.PlayerTag))
        {
            isStillinTrigger = true;
            CancelInvoke(nameof(ShowHorsePurchesPanel)); // Cancel any previous invokes
            Invoke(nameof(ShowHorsePurchesPanel), 1f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_GameAssets.PlayerTag))
        {
            isStillinTrigger = false;
            CancelInvoke(nameof(ShowHorsePurchesPanel)); // Cancel the pending enter action
            CloseHorsePurchesPanel();
        }
    }


    #region Purches Logic

    void ShowHorsePurchesPanel()
    {
        if(!HorseModeManager.Instance.IshorsePurchesed())
        {
            UIManager.Instance.OnUniversalCloseButtonPressed += CloseHorsePurchesPanel;
            UIManager.Instance.UniversalButtonAction += PurchessButtonPressed;
            _GameAssets.Instance.OnViewChangeEvent.Raise(this,true);
            UIManager.Instance.SetCloseButton(true);


            //camera view change for better look
            CameraManager.Instance.SwitchCamera(horseTranform,cameraBodyOffset,cameraAimOffset);

            //show panel with little dely so camera can transit
            LeanTween.delayedCall(2f,()=>UIManager.Instance.SetupHorseBuyPanel(true,false));
        }
    }

    

    void CloseHorsePurchesPanel()
    {
        LeanTween.cancel(this.gameObject);


        UIManager.Instance.OnUniversalCloseButtonPressed -=CloseHorsePurchesPanel;
        UIManager.Instance.UniversalButtonAction -= PurchessButtonPressed;

        UIManager.Instance.SetCloseButton(false);
        UIManager.Instance.SetupHorseBuyPanel(false,false);
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,false);

        CameraManager.Instance.SwitchCamera();
    }

    void PurchessButtonPressed(int index)
    {
        if(index == 501)
        {
            AdsManager.Instance.rewardedAds.OnAddSucessFullyWatched += OnSucessfullyMadePurches;
            AdsManager.Instance.rewardedAds.ShowRewardedAd();
        }
    }

    void OnSucessfullyMadePurches()
    {
        AdsManager.Instance.rewardedAds.OnAddSucessFullyWatched -= OnSucessfullyMadePurches;
        CloseHorsePurchesPanel();
        HorseModeManager.Instance.OnHorsePurchased();
        UIManager.Instance.ShowHorseButton(true);
    }

    #endregion

    public void ListenToOnHorseModeChange(Component sender,object data)
    {
        horseTranform.gameObject.SetActive(!(bool)data);
    }
}
