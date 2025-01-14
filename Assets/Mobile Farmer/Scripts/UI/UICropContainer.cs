using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICropContainer : MonoBehaviour
{
   [Header("Elements")]
   [SerializeField] Image iconImage;
   [SerializeField] TMP_Text amountText;

   public void Configure(Sprite icon,int amount)
   {
        amountText.text = amount.ToString();
        this.iconImage.sprite = icon;
   }

   public void UpdateDisplay(int amount)
   {
        amountText.text = amount.ToString();
   }
}
