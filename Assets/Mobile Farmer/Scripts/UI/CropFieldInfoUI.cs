using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CropFieldInfoUI : InfoUI
{
    [Header("Elemsnts")]
    [SerializeField] Image cropIcon;
    public void Initialize(CropData cropData)
    {
        cropIcon.sprite = cropData.uiIconSprite;
    }
}
