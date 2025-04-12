using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour ,IUnityAdsLoadListener ,IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    private string adUnitId;
    public Action OnAddSucessFullyWatched;
    private IUnityAdsLoadListener loadListener;

    

    private void Awake()
    {
        #if UNITY_IOS
                        adUnitId = iosAdUnitId;
        #elif UNITY_ANDROID
                adUnitId = androidAdUnitId;
        #endif
    }


    public void LoadRewardedAd()
    {
        Advertisement.Load(adUnitId,loadListener);

    }
    
    [NaughtyAttributes.Button]
    public void ShowRewardedAd()
    {
        Advertisement.Show(adUnitId, this);
        LoadRewardedAd();
    }




    #region LoadCallbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Interstitial Ad Loaded");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) { }
    #endregion

    #region ShowCallbacks
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            OnAddSucessFullyWatched?.Invoke();
            //GameManager.Instance.isRewared = true;
            //GameManager.Instance.RestartGame();
        }
    }
    #endregion


}
