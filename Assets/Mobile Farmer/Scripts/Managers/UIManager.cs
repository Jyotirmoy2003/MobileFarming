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
/*

    

   

    public async void OnGenerateCodeButtonPressed()
    {
        if (int.TryParse(itemAmountInputField.text, out int enteredItemAmount))
        {
            if (maxPossibleItemToShare >= enteredItemAmount)
            {
                // Item debited
                if(InventoryManager.Instance.RemoveItemFromInventory(selectedItemTypeToShare,enteredItemAmount))
                {
                    //generate a random code
                    string generatedCode = CodeGenerator.GenerateRandomCode();
                    
                    // Await the CreateSharedInfo function
                    bool success = await DatabaseManager.Instance.CreateSharedInfo(generatedCode,selectedItemTypeToShare, enteredItemAmount);

                    if (success)
                    {
                        // Successfully written to Firebase
                        UIManager.Instance.SetGeneratedCode(generatedCode);
                        Debug.Log($"<color=orange>Code written</color>: {generatedCode}");
                        lastGeneratedCode = generatedCode;
                        OnCopyCodeButtonPressed(); //automatic copy the generated code

                        //repopulate the Container as the values should have changed in Inventory
                        PopulateItemSelectContainer();
                    }
                    else
                    {
                    
                    }
                }
            }
            else
            {
                Debug.LogError("Insufficient balance or debit failed.");
            }
        }
        else
        {
            Debug.LogError("Invalid gold amount entered.");
        }
    }
    public async void OnAddCodeButtonPressed()
    {
        string code = codeInputField.text;
        if (!string.IsNullOrEmpty(code))
        {
            // Code is not null
            SharedInfo sharedInfo = await DatabaseManager.Instance.ValidateCode(code);
            
            if (sharedInfo.itemAmount > 0)
            {
                // Valid code, credit Items
                InventoryManager.Instance.AddItemToInventory((E_Inventory_Item_Type)sharedInfo.item_type,sharedInfo.itemAmount);
                ItemCreadited((E_Inventory_Item_Type)sharedInfo.item_type,sharedInfo.itemAmount);
            }
            else
            {
                SetLoadingPaenlForTime(true,"Invalid Code",2f);
            }
            await DatabaseManager.Instance.DeleteCode(code);
        }
        else
        {
            Debug.Log("Code input is empty.");
        }
    }



    public void OnPastButtonPressed()
    {
        string clipboardText = GUIUtility.systemCopyBuffer;
        if(!string.IsNullOrEmpty(clipboardText))
        {
            codeInputField.text = clipboardText;
        }
    }

    public void OnCopyCodeButtonPressed()
    {
        if (!string.IsNullOrEmpty(lastGeneratedCode))
        {
            GUIUtility.systemCopyBuffer = lastGeneratedCode;
            SetLoadingPaenlForTime(true,$"Copied to clipboard: {lastGeneratedCode}",2f);
        }
        else
        {
            Debug.LogError("No code available to copy.");
        }
    }

    public void OpenKeyboard(string text)
    {
        #if UNITY_ANDROID
                TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        #endif
    }

    public void OpenKeyboardNumeric(string text)
    {
        #if UNITY_ANDROID
                TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
        #endif
    }

    public void SetGeneratedCode(string  code)
    {
        generatedCode.text = code;

    }


    public void OnInputFieldValueChanged()
    {
        GenerateCodeButton.interactable = (maxPossibleItemToShare >= int.Parse(itemAmountInputField.text));
    }
    //For + and - button
    public void AddValueToInputField(int amount)
    {
        if(selectedItemTypeToShare == E_Inventory_Item_Type.None) return;

        int enteredAmount = int.Parse(itemAmountInputField.text);
        enteredAmount += amount;
        itemAmountInputField.text = enteredAmount.ToString();
    }
    public void OnGenerateCodePanelTurnOn()
    {
        OnGeneratePanelOpned?.Invoke();
        PopulateItemSelectContainer();

    }

    void PopulateItemSelectContainer()
    {
        InventoryItem[] inventoryItems = InventoryManager.Instance.GetInventory().GetInventoryItems();
        int preMadeButtonCount = selectItemContainrParent.childCount;

        // If we already have enough or fewer buttons, reuse or create as needed
        if (preMadeButtonCount <= inventoryItems.Length)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (i < preMadeButtonCount)
                {
                    // Reuse existing button
                    selectItemContainrParent.GetChild(i).GetComponent<SelectItemButton>()
                        .Configure(_GameAssets.Instance.GetItemIcon(inventoryItems[i].item_type), inventoryItems[i].item_type);
                }
                else
                {
                    // Create new button
                    SelectItemButton selectItemButton = Instantiate(_GameAssets.Instance.selectItemButtonPrefabl, selectItemContainrParent);
                    selectItemButton.Configure(_GameAssets.Instance.GetItemIcon(inventoryItems[i].item_type), inventoryItems[i].item_type);
                }
            }
        }
        else
        {
            // We have more buttons than items — reuse some, destroy the extras
            int i;
            for (i = 0; i < inventoryItems.Length; i++)
            {
                selectItemContainrParent.GetChild(i).GetComponent<SelectItemButton>()
                    .Configure(_GameAssets.Instance.GetItemIcon(inventoryItems[i].item_type), inventoryItems[i].item_type);
            }
            for (int j = i; j < preMadeButtonCount; j++)
            {
                Destroy(selectItemContainrParent.GetChild(j).gameObject);
            }
        }

        if(inventoryItems.Length > 0)
        {
            //Copied the hole code fron SelectedItemType Fun just remove the scrollview deactive line
            selectedItemTypeToShare = inventoryItems[0].item_type;

            //select correct icon
            selectedItemIcon.sprite=_GameAssets.Instance.GetItemIcon(inventoryItems[0].item_type);
            maxPossibleItemToShare = InventoryManager.Instance.GetInventory().GetItemAmountInInventory(selectedItemTypeToShare);
            selectedItemButton.interactable = true;

            GenerateCodeButton.interactable = !(maxPossibleItemToShare <= 0);
            itemAmountInputField.text = maxPossibleItemToShare.ToString();
        }
        else
        { 
            selectedItemTypeToShare = E_Inventory_Item_Type.None;
            selectedItemButton.interactable = false;
            GenerateCodeButton.interactable = false;
            selectItemtoShareScrollview.SetActive(false);
        }
    }



    //listen to button pressed event
    public void ListnToOnSelectShareItemButtonPressed(Component sender,object data)
    {
        if(data is E_Inventory_Item_Type)
        {
            SelectedItemType((E_Inventory_Item_Type)data);
        }
    }

    void SelectedItemType(E_Inventory_Item_Type item_type)
    {
        selectedItemTypeToShare = item_type;
        selectItemtoShareScrollview.SetActive(false);

        //select correct icon
        selectedItemIcon.sprite=_GameAssets.Instance.GetItemIcon(item_type);
        maxPossibleItemToShare = InventoryManager.Instance.GetInventory().GetItemAmountInInventory(selectedItemTypeToShare);
        selectedItemButton.interactable = true;

        GenerateCodeButton.interactable = !(maxPossibleItemToShare <= 0);
        itemAmountInputField.text = maxPossibleItemToShare.ToString();
        
    }
    */

    #endregion






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
