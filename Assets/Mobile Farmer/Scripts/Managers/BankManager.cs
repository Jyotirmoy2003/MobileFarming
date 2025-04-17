using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BankManager : MonoBehaviour
{

    [SerializeField] Transform selectItemContainerParent;
    [SerializeField] string fileName;


    private string lastGeneratedCode;
    private E_Inventory_Item_Type selectedItemTypeToShare;
    private int maxPossibleItemToShare =-1;
    private bool isStillinTrigger = false;
    private BankSaves bankSaves;
    private string dataPath;

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



    void LoadCode()
    {
        dataPath = Application.persistentDataPath + fileName;
        #if UNITY_EDITOR
        dataPath = Application.dataPath + fileName;
        #endif

        bankSaves = SaveAndLoad.Load<BankSaves>(dataPath);

        if(bankSaves == null)
        {
            bankSaves = new BankSaves();
        }
    }





    void SaveCode(string code)
    {
        dataPath = Application.persistentDataPath + fileName;
        #if UNITY_EDITOR
        dataPath = Application.dataPath + fileName;
        #endif

        bankSaves.previousCodes.Add(code);

        //remove extras
        while(bankSaves.previousCodes.Count > 5)
        {
            bankSaves.previousCodes.RemoveAt(0);
        }

        //save data
        SaveAndLoad.Save(dataPath,bankSaves);
    }

    void EnterinBank()
    {
        if (!isStillinTrigger) return;

        //_GameAssets.Instance.OnViewChangeEvent.Raise(this, true); // Turn off joystick control
        UIManager.Instance.OnUniversalCloseButtonPressed -= CloseButtonPressed; // Ensure no duplicate subscriptions
        UIManager.Instance.OnUniversalCloseButtonPressed += CloseButtonPressed;
        UIManager.Instance.SetCloseButton(true);

        UIManager.Instance.UniversalButtonAction +=UniversalButtonInput;

        // Bank UI activate
        UIManager.Instance.BankUIActivationStatus(true);

        LoadCode();
        PopulatePrivousCodes();
    }

    void ExitfromBank()
    {
        CloseButtonPressed();
    }

    void CloseButtonPressed()
    {
       
        UIManager.Instance.SetCloseButton(false);
        UIManager.Instance.OnUniversalCloseButtonPressed -= CloseButtonPressed;
        UIManager.Instance.UniversalButtonAction -=UniversalButtonInput;
        //_GameAssets.Instance.OnViewChangeEvent.Raise(this,false); //turn on joystick controll

        //Bank UI deactivate
        UIManager.Instance.BankUIActivationStatus(false);

    }





    void UniversalButtonInput(int index)
    {
        switch (index)
        {
            case 50:
                OpenKeyboard();
                break;
            case 51:
                OpenKeyboardNumeric();
                break;
            case 52:
                OnInputFieldValueChanged();
                break;
            case 53:
                AddValueToInputField(1);
                break;
            case 54:
                AddValueToInputField(-1);
                break;
            case 55:
                OnGenerateCodeButtonPressed();
                break;
            case 56:
                OnCopyCodeButtonPressed();
                break;
            case 57:
                OnAddCodeButtonPressed();
                break;
            case 58:
                OnPastButtonPressed();
                break;
            case 59:
                PopulateItemSelectContainer();
                break;
            case 60:
                GUIUtility.systemCopyBuffer = bankSaves.previousCodes[0];
                UIManager.Instance.SetLoadingPaenlForTime(true,$"Copied to clipboard: {bankSaves.previousCodes[0]}",2f);
                break;
            case 61:
                GUIUtility.systemCopyBuffer = bankSaves.previousCodes[1];
                UIManager.Instance.SetLoadingPaenlForTime(true,$"Copied to clipboard: {bankSaves.previousCodes[1]}",2f);
                break;
            case 62:
                GUIUtility.systemCopyBuffer = bankSaves.previousCodes[2];
                UIManager.Instance.SetLoadingPaenlForTime(true,$"Copied to clipboard: {bankSaves.previousCodes[2]}",2f);
                break;
            case 63:
                GUIUtility.systemCopyBuffer = bankSaves.previousCodes[3];
                UIManager.Instance.SetLoadingPaenlForTime(true,$"Copied to clipboard: {bankSaves.previousCodes[3]}",2f);
                break;
            case 64:
                GUIUtility.systemCopyBuffer = bankSaves.previousCodes[4];
                UIManager.Instance.SetLoadingPaenlForTime(true,$"Copied to clipboard: {bankSaves.previousCodes[4]}",2f);
                break;
            
            default:
            break;
        }
    }




    #region  UI Code

    async void PopulatePrivousCodes()
    {
       

        for(int i=0 ; i<UIManager.Instance.bundelButtons.Count; i++)
        {
            if(i < bankSaves.previousCodes.Count)
            {
                SharedInfo sharedInfo = await DatabaseManager.Instance.ValidateCode(bankSaves.previousCodes[i]);
                
                UIManager.Instance.bundelButtons[i].interactable = (sharedInfo != null);
                
            }else
            {
                UIManager.Instance.bundelButtons[i].interactable = false;
            }
        }
    }
    

   

    public async void OnGenerateCodeButtonPressed()
    {
        if (int.TryParse(UIManager.Instance.itemAmountInputField.text, out int enteredItemAmount))
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
                        UIManager.Instance.generatedCode.text = generatedCode;
                        Debug.Log($"<color=orange>Code written</color>: {generatedCode}");
                        lastGeneratedCode = generatedCode;
                        OnCopyCodeButtonPressed(); //automatic copy the generated code

                        //repopulate the Container as the values should have changed in Inventory
                        PopulateItemSelectContainer();

                        SaveCode(generatedCode);
                        PopulatePrivousCodes();
                    }
                    else
                    {
                        //give back those items which was debited
                        InventoryManager.Instance.AddItemToInventory(selectedItemTypeToShare,enteredItemAmount);

                        UIManager.Instance.SetLoadingPaenlForTime(true,"Faild",1f);
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
        string code = UIManager.Instance.codeInputField.text;
        if (!string.IsNullOrEmpty(code) && code.Length < 6)
        {
            
            SharedInfo sharedInfo = await DatabaseManager.Instance.ValidateCode(code);
            if(sharedInfo == null)
            {
                UIManager.Instance.SetLoadingPaenlForTime(true,"Invalid Code",0.5f);
                return;
            }
            // Code is not null
            if (sharedInfo.itemAmount > 0)
            {
                // Valid code, credit Items
                InventoryManager.Instance.AddItemToInventory((E_Inventory_Item_Type)sharedInfo.item_type,sharedInfo.itemAmount);
                UIManager.Instance.ItemCreadited((E_Inventory_Item_Type)sharedInfo.item_type,sharedInfo.itemAmount);
                //Re popu late bundel button by any chance if player Adds the code in selft bank
                PopulatePrivousCodes();
            }
            else
            {
                UIManager.Instance.SetLoadingPaenlForTime(true,"Invalid Code",2f);
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
            UIManager.Instance.codeInputField.text = clipboardText;
        }
    }

    public void OnCopyCodeButtonPressed()
    {
        if (!string.IsNullOrEmpty(lastGeneratedCode))
        {
            GUIUtility.systemCopyBuffer = lastGeneratedCode;
            UIManager.Instance.SetLoadingPaenlForTime(true,$"Copied to clipboard: {lastGeneratedCode}",2f);
        }
        else
        {
            Debug.LogError("No code available to copy.");
        }
    }

    public void OpenKeyboard()
    {
        #if UNITY_ANDROID
                TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        #endif
    }

    public void OpenKeyboardNumeric()
    {
        #if UNITY_ANDROID
                TouchScreenKeyboard.Open("", TouchScreenKeyboardType.NumberPad);
        #endif
    }

    public void SetGeneratedCode(string  code)
    {
        UIManager.Instance.generatedCode.text = code;

    }


    public void OnInputFieldValueChanged()
    {
        UIManager.Instance.GenerateCodeButton.interactable = (maxPossibleItemToShare >= int.Parse(UIManager.Instance.itemAmountInputField.text));
    }
    //For + and - button
    public void AddValueToInputField(int amount)
    {
        if(selectedItemTypeToShare == E_Inventory_Item_Type.None) return;

        int enteredAmount = int.Parse(UIManager.Instance.itemAmountInputField.text);
        enteredAmount += amount;
        UIManager.Instance.itemAmountInputField.text = enteredAmount.ToString();
    }
    

    void PopulateItemSelectContainer()
    {
        InventoryItem[] inventoryItems = InventoryManager.Instance.GetInventory().GetInventoryItems();
        int preMadeButtonCount = UIManager.Instance.selectItemContainrParent.childCount;

        // If we already have enough or fewer buttons, reuse or create as needed
        if (preMadeButtonCount <= inventoryItems.Length)
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (i < preMadeButtonCount)
                {
                    // Reuse existing button
                    UIManager.Instance.selectItemContainrParent.GetChild(i).GetComponent<SelectItemButton>()
                        .Configure(_GameAssets.Instance.GetItemIcon(inventoryItems[i].item_type), inventoryItems[i].item_type);
                }
                else
                {
                    // Create new button
                    SelectItemButton selectItemButton = Instantiate(_GameAssets.Instance.selectItemButtonPrefabl, UIManager.Instance.selectItemContainrParent);
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
                UIManager.Instance.selectItemContainrParent.GetChild(i).GetComponent<SelectItemButton>()
                    .Configure(_GameAssets.Instance.GetItemIcon(inventoryItems[i].item_type), inventoryItems[i].item_type);
            }
            for (int j = i; j < preMadeButtonCount; j++)
            {
                Destroy(UIManager.Instance.selectItemContainrParent.GetChild(j).gameObject);
            }
        }

        if(inventoryItems.Length > 0)
        {
            //Copied the hole code fron SelectedItemType Fun just remove the scrollview deactive line
            selectedItemTypeToShare = inventoryItems[0].item_type;

            //select correct icon
            UIManager.Instance.selectedItemIcon.sprite=_GameAssets.Instance.GetItemIcon(inventoryItems[0].item_type);
            maxPossibleItemToShare = InventoryManager.Instance.GetInventory().GetItemAmountInInventory(selectedItemTypeToShare);
            UIManager.Instance.selectedItemButton.interactable = true;

            UIManager.Instance.GenerateCodeButton.interactable = !(maxPossibleItemToShare <= 0);
            UIManager.Instance.itemAmountInputField.text = maxPossibleItemToShare.ToString();
        }
        else
        { 
            selectedItemTypeToShare = E_Inventory_Item_Type.None;
            UIManager.Instance.selectedItemButton.interactable = false;
            UIManager.Instance.GenerateCodeButton.interactable = false;
            UIManager.Instance.selectItemtoShareScrollview.SetActive(false);
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
        UIManager.Instance.selectItemtoShareScrollview.SetActive(false);

        //select correct icon
        UIManager.Instance.selectedItemIcon.sprite=_GameAssets.Instance.GetItemIcon(item_type);
        maxPossibleItemToShare = InventoryManager.Instance.GetInventory().GetItemAmountInInventory(selectedItemTypeToShare);
        UIManager.Instance.selectedItemButton.interactable = true;

        UIManager.Instance.GenerateCodeButton.interactable = !(maxPossibleItemToShare <= 0);
        UIManager.Instance.itemAmountInputField.text = maxPossibleItemToShare.ToString();
        
    }
    


    #endregion


    
}

public class BankSaves
{
    public List<string> previousCodes = new List<string>();
}