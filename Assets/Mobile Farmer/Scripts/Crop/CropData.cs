using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;


[CreateAssetMenu(fileName ="cropData" ,menuName ="GAME/Crop Data")]
public class CropData : ScriptableObject
{
   [Header("Setttings")]
   public Crop cropPrefab;
   public E_Crop_Type cropType;
   public E_Inventory_Item_Type item_type;
   public float growDuration=3f;
   [Range(1,5)]
   public int amountinSingleCrop=3;
   public Sprite uiIconSprite;
   public Sprite cropPopUp;
   public int pricePerPice=1;
}
