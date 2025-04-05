using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using jy_util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
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
    [SerializeField] TMP_InputField codeInputField;

    [Header("Generate Code")]
    [SerializeField] TMP_InputField itemAmountInputField;
    [SerializeField] TMP_Text generatedCode;
    [SerializeField] Transform selectItemContainrParent;
    [SerializeField] GameObject selectItemtoShareGameobject;
    [SerializeField] Image selectedItemIcon;
    [SerializeField] Button GenerateCodeButton;
    private string lastGeneratedCode;
    private E_Inventory_Item_Type selectedItemTypeToShare;
    private int maxPossibleItemToShare =-1;


    [Space]
    [Header("Loading Panel")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] TMP_Text loadingText;


    public Action OnUniversalCloseButtonPressed;

    
    void Start()
    {
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

    #region  Bank UI

    public void BankUIActivationStatus(bool isActive)
    {
        bankTrandeUIConatiner.SetActive(isActive);
    }

    

    // public async void OnGenerateCodeButtonPressed()
    // {
    //     if (int.TryParse(itemAmountInputField.text, out int enteredItemAmount))
    //     {
    //         if (CashManager.Instance.DebitCoin(enteredItemAmount))
    //         {
    //             // Cash debited, generate a random code
    //             string generatedCode = CodeGenerator.GenerateRandomCode();
                
    //             // Await the CreateSharedInfo function
    //             bool success = await DatabaseManager.Instance.CreateSharedInfo(generatedCode, enteredItemAmount, true);

    //             if (success)
    //             {
    //                 // Successfully written to Firebase
    //                 UIManager.Instance.SetGeneratedCode(generatedCode);
    //                 Debug.Log($"<color=orange>Code written</color>: {generatedCode}");
    //                 lastGeneratedCode = generatedCode;
    //             }
    //             else
    //             {
                   
    //             }
    //         }
    //         else
    //         {
    //             Debug.LogError("Insufficient balance or debit failed.");
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("Invalid gold amount entered.");
    //     }
    // }

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

            }
            else
            {
                Debug.Log("Invalid or non-existent code.");
            }
            await DatabaseManager.Instance.DeleteCode(code);
        }
        else
        {
            Debug.Log("Code input is empty.");
        }
    }



    public void OnCopyCodeButtonPressed()
    {
        if (!string.IsNullOrEmpty(lastGeneratedCode))
        {
            GUIUtility.systemCopyBuffer = lastGeneratedCode;
            Debug.Log($"Copied to clipboard: {lastGeneratedCode}");
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
        int enteredAmount = int.Parse(itemAmountInputField.text);
        enteredAmount += amount;
        itemAmountInputField.text = enteredAmount.ToString();
    }
    public void OnGenerateCodePanelTurnOn()
    {
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
        selectItemtoShareGameobject.SetActive(false);

        //select correct icon
        selectedItemIcon.sprite=_GameAssets.Instance.GetItemIcon(item_type);
        maxPossibleItemToShare = InventoryManager.Instance.GetInventory().GetItemAmountInInventory(selectedItemTypeToShare);

        GenerateCodeButton.interactable = !(maxPossibleItemToShare <= 0);
        
    }
    

    #endregion
}
