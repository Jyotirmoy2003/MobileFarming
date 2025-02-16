using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CropFieldInfoUI : InfoUI
{
    [Header("Elemsnts")]
    [SerializeField] SpriteRenderer croPopup;
    public void Initialize(CropData cropData)
    {
        croPopup.sprite = cropData.cropPopUp;
    }
}
