using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] TMP_InputField goldAmountInputField;
    [SerializeField] TMP_Text generatedCode;
    private string lastGeneratedCode;


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

    public void OnGenerateCodePanelOpen()
    {
        
    }

    public async void OnGenerateCodeButtonPressed()
    {
        if (int.TryParse(goldAmountInputField.text, out int enteredGoldAmount))
        {
            if (CashManager.Instance.DebitCoin(enteredGoldAmount))
            {
                // Cash debited, generate a random code
                string generatedCode = CodeGenerator.GenerateRandomCode();
                
                // Await the CreateSharedInfo function
                bool success = await DatabaseManager.Instance.CreateSharedInfo(generatedCode, enteredGoldAmount, true);

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
            int goldAmount = await DatabaseManager.Instance.ValidateCode(code);
            
            if (goldAmount > 0)
            {
                // Valid code, credit coins
                CashManager.Instance.CreditCoins(goldAmount);

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
    

    #endregion
}
