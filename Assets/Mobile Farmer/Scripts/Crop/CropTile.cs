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

    




    
   
    public bool IsEmpty()
    {
        return state==E_Crop_State.Empty;
    }
    public bool IsSown()
    {
        return state==E_Crop_State.Sown;
    }

    public bool IsWatered()
    {
        return state==E_Crop_State.Watered;
    }

    public void Sow(CropData cropData)
    {
        state=E_Crop_State.Sown;

        if(crop == null)
        {
            //set up new Crop in tile
            crop=Instantiate(cropData.cropPrefab,transform.position,Quaternion.identity,cropParent);
            crop.PushData(cropData);
        }else{
            crop.gameObject.SetActive(true);
            crop.ResetScale();
        }
    }


    public void Water(CropData cropData)
    {
        state=E_Crop_State.Watered;
        tileMeshrenderer.gameObject.LeanColor(Color.white * .3f, 1f);

        if(crop == null)
        {
            //when field is mearging while its watered
            Sow(cropData);
            crop.InstantScaleUp();
            return;
        }
        //Let  the crop grow

        crop?.ScaleUp();
    }

    public void Harvest(Component hervestOwner,CropData cropData)
    {
        if(crop == null)
        {
            //when crop field is meargin and player also tring to hervest at same time
            Water(cropData);
        }

        state = E_Crop_State.Empty;
        crop.ScaleDown();
        tileMeshrenderer.gameObject.LeanColor(Color.white , 1f);

        //Event fire
        _GameAssets.Instance.OnHervestedEvent.Raise(hervestOwner,cropData);
    }

    public void ForceHervest()
    {  
        if(state == E_Crop_State.Empty) return; //already harvested correctly
        state = E_Crop_State.Empty;
        crop.ScaleDown();
        tileMeshrenderer.gameObject.LeanColor(Color.white , 1f);
    }

    public void ForceWater()
    {
        if(state == E_Crop_State.Watered) return;

        state=E_Crop_State.Watered;
        tileMeshrenderer.gameObject.LeanColor(Color.white * .3f, 1f);
        crop?.ScaleUp();
    }

    public void ForceSow(CropData cropData)
    {
        if(state == E_Crop_State.Sown) return;

        state=E_Crop_State.Sown;

        if(crop == null)
        {
            //set up new Crop in tile
            crop=Instantiate(cropData.cropPrefab,transform.position,Quaternion.identity,cropParent);
            crop.PushData(cropData);
        }else{
            crop.gameObject.SetActive(true);
            crop.ResetScale();
        }
    }
}
