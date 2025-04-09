using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public InitializeAds initializeAds;
    public BannerAds bannerAds;
    public InterstitialAds interstitialAds;
    public RewardedAds rewardedAds;

    public static AdsManager Instance { get; private set; }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


        bannerAds.LoadBannerAd();
        interstitialAds.LoadInterstitialAd();
        rewardedAds.LoadRewardedAd();
    }


    void Start()
    {
        UIManager.Instance.UniversalButtonAction +=ListenToUniversalButtonEvent;    
    }


    void OnDisable()
    {
        UIManager.Instance.UniversalButtonAction -= ListenToUniversalButtonEvent;       
    }



    private int currentCoinamount  = 0;
    void OnRewardAddButtonPressedCoin(int amount)
    {
        currentCoinamount = amount;
        rewardedAds.OnAddSucessFullyWatched += RewardGranted;
        rewardedAds.ShowRewardedAd();
    }

    void RewardGranted()
    {
        rewardedAds.OnAddSucessFullyWatched -= RewardGranted;
        TransactionEffectManager.Instance.PlayCoinParticel(currentCoinamount);
    }


    public void ListenToUniversalButtonEvent(int data)
    {
        switch (data)
        {
            case 1001:
                OnRewardAddButtonPressedCoin(500);
                break;
        }
    }
}
