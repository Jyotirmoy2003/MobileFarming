using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class _GameAssets : MonoSingleton<_GameAssets>
{
    public static readonly string PlayerTag = "Player";
    [Header("Events")]
    public GameEvent OnShakeInitiateEvent;
    public GameEvent OnPlayerInteractStatusChangeEvent;
    public GameEvent OnHervestedEvent;
    public GameEvent OnViewChangeEvent;
    public GameEvent OnSimpleUIButtonPressed;

    public GameEvent OnSheepModeChangedEvent;
    [Header("Particel System")]
    public ParticleSystem spawnDustParticel;
    public ParticleSystem seedParticel;
    public ParticleSystem waterParticel;

    [Space]
    [Header("Player Components")]
    public PlayerAnimator playerAnimator;
    public PlayerToolSelector playerToolSelector;

    [Space]
    [Header("Managers")]
    public InventoryManager inventoryManager;

    [Space]
    [Header("3D object Ref")]
    public GameObject wateringCan;
    public GameObject harvestScythe;
    public Transform fishingRodTip;
    [Space]
    [Header("UI")]
    public SelectItemButton selectItemButtonPrefabl;

    [Space]
    [Header("Material Ref")]
    public Material defaultCropTileMat;
    public Material wateredCropTileMat;

    [Space]
    [Header("CropData")]
    public CropField[] allFieldInGame ;
    public List<CropData> cropDatas=new List<CropData>();

    [Space]
    [Header("Float Data")]
    public float fishShakeThresold =350f;

    public void ListnToOnGameCache(Component sender,object data)
    {
      allFieldInGame = FindObjectsOfType<CropField>();

    }

    public CropData GetCropData(E_Inventory_Item_Type item_Type)
    {
        foreach(CropData cropData in cropDatas) if(cropData.item_type == item_Type) return cropData;

        Debug.LogError("Crop data not added in _GameAssets");
        return null;
    }

    public Sprite GetItemIcon(E_Inventory_Item_Type item_type)
    {
        foreach(CropData item in cropDatas)
        {
            if(item.item_type == item_type)
            {
                return item.uiIconSprite;

            }
        }
            
        return null;
    }
    
}
