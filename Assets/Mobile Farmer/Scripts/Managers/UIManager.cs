using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using jy_util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    #region Variables
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject shakeModePanel;
    [Header("Universal Buttons")]
    [SerializeField] GameObject closeButton;
    
    [Header("Interact Button")]
    [SerializeField] Button IntreactButton;
    [SerializeField] Image interactButtonIcon;

    [Header("Shake Silder")]
    [SerializeField] TMP_Text shakeSliderText;
    [SerializeField] Slider shakeSlider;
    [SerializeField] Button shakeExitButton;
    [Space]
    [Header("Bank UI")]
    [SerializeField] GameObject bankTrandeUIConatiner;
    [SerializeField] GameObject bankGetCodeUIContainer,bankSendCodeUIContainer;
    [Header("GetCode")]
    public TMP_InputField codeInputField;

    [Header("Generate Code")]
    public TMP_InputField itemAmountInputField;
    public TMP_Text generatedCode;
    public Transform selectItemContainrParent;
    public GameObject selectItemtoShareScrollview;
    public Image selectedItemIcon;
    public Button GenerateCodeButton;
    public Button selectedItemButton;
    private string lastGeneratedCode;
    private E_Inventory_Item_Type selectedItemTypeToShare;
    private int maxPossibleItemToShare =-1;


    [Space]
    [Header("Loading Panel")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] TMP_Text loadingText;

    [Space]
    [Header("Item Creadited")]
    [SerializeField] RectTransform creditedImageParent;
    [SerializeField] RectTransform creditedChakra;
    [SerializeField] Image crediItemIcon;
    [SerializeField] TMP_Text crediAmoutText;

    [Space]
    [Header("Horse Mode")]
    [SerializeField] GameObject horseButton;

    [Space]
    [Header("Shop Panel")]
    [SerializeField] GameObject shopPanel;
    [SerializeField] List<GameObject> shopPanels = new List<GameObject>();
    

    public Action OnUniversalCloseButtonPressed;
    public Action<int> UniversalButtonAction;

    #endregion
    
    void Start()
    {
        
        creditedImageParent.gameObject.SetActive(false);

        shakeModePanel.SetActive(false);
        IntreactButton.gameObject.SetActive(false);
    }



    

   

   


    public void ListenToShakeModeStartEvent(Component sender,object data)
    {
        shakeModePanel.SetActive((string)data != "");
        shakeSliderText.text = (string)data;
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

    public void SetCloseButton(bool isActive)
    {
        closeButton.SetActive(isActive);
    }

    public void ShowShakeExit(bool val)
    {
        shakeExitButton.gameObject.SetActive(val);
    }


    public void UpdateShakeSlider(float value)
    {
        shakeSlider.value = value;
    }
    
 


    public void OnUniversalClosePressed()
    {
        OnUniversalCloseButtonPressed?.Invoke();
    }

    public void SetLoadingPanel(bool isActive,string data)
    {
        loadingPanel.SetActive(isActive);
        loadingText.text = data;
    }

    
    public void SetLoadingPaenlForTime(bool isActive,string data,float time)
    {
        gameObject.LeanCancel(); //dismiss all previous lean working on this

        SetLoadingPanel(isActive,data);
        LeanTween.delayedCall(time,()=> loadingPanel.SetActive(false));
    }
    /// <summary>
    /// Any class can subcribe to this and listen for ui inputs without making direct reference to the UI
    /// just pass the correct index through the button and listne in reciver class then chechk if you are looking for that index , and handel accordingly
    /// </summary>
    /// <param name="index">Every listening class will look for a specific index which will be passed from the button making it resuable</param>

    public void UniversalButtonPressed(int index)
    {
        UniversalButtonAction?.Invoke(index);
    }



    #region  Bank UI
    public void BankUIActivationStatus(bool isActive)
    {
        bankTrandeUIConatiner.SetActive(isActive);
    }

    #endregion


    public void InsufficientGold()
    {
        shopPanel.SetActive(true);
    }

    public void InsufficientGem()
    {
        shopPanel.SetActive(true);
    }

    public void OnShopButtonPressed()
    {
        shopPanel.SetActive(true);
    }

    public void OnShopClosePressed()
    {
        shopPanel.SetActive(false);
    }


    #region  ItemCreadited UI

    [NaughtyAttributes.Button]
    public void PopUpTest()
    {
        ItemCreadited(E_Inventory_Item_Type.Corn,1000);
    }

    public void ItemCreadited(E_Inventory_Item_Type item_Type,int amount)
    {
        LeanTween.cancel(this.gameObject);

        
        crediItemIcon.sprite = _GameAssets.Instance.GetItemIcon(item_Type);
        crediAmoutText.text =("+")+ CoinSystem.ConvertCoinToString(amount);
        creditedImageParent.gameObject.SetActive(true);

        creditedImageParent.anchoredPosition = new Vector2(0,-1500);

        
        creditedChakra.DORotate(new Vector3(0, 0, 360), 4f, RotateMode.FastBeyond360)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Restart);


        Sequence creditSequence = DOTween.Sequence();

        creditSequence.Append(
            creditedImageParent.DOAnchorPos(Vector2.zero, 1f).SetEase(Ease.InOutQuint)
        );
        creditSequence.AppendInterval(2f); // Wait for 2 seconds
        creditSequence.AppendCallback(() => {
           creditedImageParent.DOAnchorPos( new Vector2(0, 1500),0.3f); // Reset

           creditedChakra.DOKill(); // Stop rotation
            creditedChakra.rotation = Quaternion.identity; // Reset rotation
        });
    }








    #endregion


}
