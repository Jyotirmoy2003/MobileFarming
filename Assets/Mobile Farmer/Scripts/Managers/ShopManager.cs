using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class ShopManager : MonoSingleton<ShopManager>
{
    [SerializeField] GameObject shopPanel;
    [Header("Gem")]
    [SerializeField] GameObject gemPanel;
    [SerializeField] GameObject gemActiveButtonImage;
    [SerializeField] GameObject gemDeactiveButtonImage;
    [SerializeField] List<S_PurchessAmount_Pair> gemPurcheses = new List<S_PurchessAmount_Pair>();

    [Space]
    [Header("Coin")]
    [SerializeField] GameObject coinPanel;
    [SerializeField] GameObject coinActiveButtonImage;
    [SerializeField] GameObject coinDeactiveButtonImage;
    [SerializeField] List<S_PurchessAmount_Pair> coinPurcheses = new List<S_PurchessAmount_Pair>();



    void Start()
    {
        OnGemShopClicked();
    }

    public void ShopButtonPressed()
    {
        // UIManager.Instance.OnUniversalCloseButtonPressed += ShopClose;
        // UIManager.Instance.SetCloseButton(true);
        shopPanel.SetActive(true);
    }

    void ShopClose()
    {
        // UIManager.Instance.OnUniversalCloseButtonPressed -= ShopClose;
        // UIManager.Instance.SetCloseButton(false);
        shopPanel.SetActive(false);
    }


    public void OnGemShopClicked()
    {
        ActivateGemPanel();
    }


    public void OnCoinShopClicked()
    {
        ActivateCoinPanel();
    }

    void ActivateGemPanel()
    {
        coinPanel.SetActive(false);
        coinActiveButtonImage.SetActive(false);
        coinDeactiveButtonImage.SetActive(true);


        gemPanel.SetActive(true);
        gemActiveButtonImage.SetActive(true);
        gemDeactiveButtonImage.SetActive(false);
    }

    void ActivateCoinPanel()
    {
        gemPanel.SetActive(false);
        gemActiveButtonImage.SetActive(false);
        gemDeactiveButtonImage.SetActive(true);

        coinPanel.SetActive(true);
        coinActiveButtonImage.SetActive(true);
        coinDeactiveButtonImage.SetActive(false);
    }


    public void OnCoinPurchessClicked(int index)
    {
        if(index> coinPurcheses.Count)
        {
            //show ad
            AdsManager.Instance.rewardedAds.OnAddSucessFullyWatched += AdWatchedForCoin;
            AdsManager.Instance.rewardedAds.ShowRewardedAd();
            return;
        }
        int priceAmount = coinPurcheses[index].amountToDebit;

        if(CashManager.Instance.DebitGems(priceAmount))
        {
            //gems debited
            TransactionEffectManager.Instance.PlayCoinParticel(coinPurcheses[index].amountToCredit);
            UIManager.Instance.ItemCreadited(E_Inventory_Item_Type.Coin,coinPurcheses[index].amountToCredit);
            ShopClose();
        }else{
            ShopClose();
            UIManager.Instance.SetLoadingPaenlForTime(true,"Not enough Gems!",1f);

        }
    }

    public void OnGemPurchessClicked(int index)
    {
        if(index> gemPurcheses.Count)
        {
            //show ad
            AdsManager.Instance.rewardedAds.OnAddSucessFullyWatched += AdWatchedForGems;
            AdsManager.Instance.rewardedAds.ShowRewardedAd();
            return;
        }
    }

    void AdWatchedForCoin()
    {
        AdsManager.Instance.rewardedAds.OnAddSucessFullyWatched -= AdWatchedForCoin;
        TransactionEffectManager.Instance.PlayCoinParticel(800);
        UIManager.Instance.ItemCreadited(E_Inventory_Item_Type.Coin,800);
        ShopClose();
    }

    void AdWatchedForGems()
    {
        AdsManager.Instance.rewardedAds.OnAddSucessFullyWatched -= AdWatchedForGems;
        TransactionEffectManager.Instance.PlayGemParticel(5 );
        UIManager.Instance.ItemCreadited(E_Inventory_Item_Type.Gem,800);
        ShopClose();
    }
}
