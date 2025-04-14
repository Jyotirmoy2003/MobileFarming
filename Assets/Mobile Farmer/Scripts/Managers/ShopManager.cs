using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoSingleton<ShopManager>
{
    [SerializeField] GameObject shopPanel;
    [Header("Gem")]
    [SerializeField] GameObject gemPanel;
    [SerializeField] GameObject gemActiveButtonImage;
    [SerializeField] GameObject gemDeactiveButtonImage;

    [Space]
    [Header("Coin")]
    [SerializeField] GameObject coinPanel;
    [SerializeField] GameObject coinActiveButtonImage;
    [SerializeField] GameObject coinDeactiveButtonImage;



    void Start()
    {
        OnGemShopClicked();
    }

    public void ShopButtonPressed()
    {
        UIManager.Instance.OnUniversalCloseButtonPressed += ShopClose;
        UIManager.Instance.SetCloseButton(true);
        shopPanel.SetActive(true);
    }

    void ShopClose()
    {
        UIManager.Instance.OnUniversalCloseButtonPressed -= ShopClose;
        UIManager.Instance.SetCloseButton(false);
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
}
