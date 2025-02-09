using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameAssets : MonoSingleton<_GameAssets>
{
    [Header("Events")]
    public GameEvent OnPlayerInteractStatusChangeEvent;
    public GameEvent OnHervestedEvent;
    public GameEvent OnViewChangeEvent;
    [Header("Particel System")]
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

    [Space]
    [Header("Material Ref")]
    public Material defaultCropTileMat;
    public Material wateredCropTileMat;

    [Space]
    [Header("CropData")]
    public CropField[] allFieldInGame ;
    public List<CropData> cropDatas=new List<CropData>();

    public void ListnToOnGameCache(Component sender,object data)
    {
      allFieldInGame = FindObjectsOfType<CropField>();

    }
    
}
