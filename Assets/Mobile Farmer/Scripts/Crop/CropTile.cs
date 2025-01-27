using System;
using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class CropTile : MonoBehaviour
{
    [SerializeField] //fot teset now its shown in inspector
    private E_Crop_State state;
    [SerializeField] Transform cropParent;
    [SerializeField] MeshRenderer tileMeshrenderer;
    private Crop crop;
    public FeedBackManager feedBackManager;

    




    void Start()
    {
        
    }

   
    public bool IsEmpty()
    {
        return state==E_Crop_State.Empty;
    }
    public bool IsSown()
    {
        return state==E_Crop_State.Sown;
    }

    

    public void Sow(CropData cropData)
    {
        state=E_Crop_State.Sown;

        //set up new Crop in tile
        crop=Instantiate(cropData.cropPrefab,transform.position,Quaternion.identity,cropParent);
        crop.PushData(cropData);
    }


    public void Water(CropData cropData)
    {
        state=E_Crop_State.Watered;
        tileMeshrenderer.gameObject.LeanColor(Color.white * .3f, 1f);
        //Let  the crop grow

        crop?.ScaleUp();
    }

    public void Harvest(CropData cropData)
    {
        state = E_Crop_State.Empty;
        crop.ScaleDown();
        tileMeshrenderer.gameObject.LeanColor(Color.white , 1f);

        //Event fire
        _GameAssets.Instance.OnHervestedEvent.Raise(this,cropData);
    }
}
