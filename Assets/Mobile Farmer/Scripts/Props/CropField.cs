using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class CropField : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Transform tilesParent;
    private List<CropTile> cropTiles= new List<CropTile>();

    [Header("Settings")]
    [SerializeField] CropData cropData;

    private int tileSown=0;
    private E_Crop_State state;

    [Header("Action")]
    public static Action<CropField> onFullySown;


    void Start()
    {
        state=E_Crop_State.Empty;
        StoreTile();
    }


    void StoreTile()
    {
        for(int i=0;i<tilesParent.childCount;i++)
            cropTiles.Add(tilesParent.GetChild(i).GetComponent<CropTile>());
    }
   

    public void SeedCollidedCallback(Vector3[] seedPos)
    {
        //go throuw all collided particel and sow seed in collided crop tile
        for(int i=0;i<seedPos.Length;i++)
        {
            
            CropTile closestCropTile=GetClosestCropTile(seedPos[i]);
            if(closestCropTile==null)
                continue;

            if(!closestCropTile.IsEmpty())
                continue;

            closestCropTile.Sow(cropData);
            tileSown++;

            if(tileSown>=cropTiles.Count)
                FieldFullySown();
            
        }
    }
    public void WaterCollidedCallBack(Vector3[] waterPos)
    {

    }
    private CropTile GetClosestCropTile(Vector3 seedPos)
    {
        float minDistance=5000f;
        int closesCroptileIndex=-1;

        for(int i=0;i<cropTiles.Count;i++)
        {
            CropTile temp_holding_cropTile=cropTiles[i];
            float disanceTileSeed=Vector3.Distance(temp_holding_cropTile.transform.position,seedPos);
            if(disanceTileSeed < minDistance)
            {
                minDistance=disanceTileSeed;
                closesCroptileIndex=i;
            }
        }


        if(closesCroptileIndex==-1) return null;

        return cropTiles[closesCroptileIndex];
    }


    private void Sow(CropTile cropTile)
    {
        //cropTile.Sow();
    }

    private  void FieldFullySown()
    {
        state= E_Crop_State.Sown;
        onFullySown?.Invoke(this);
    }

    public bool IsEmpty()
    {
        return state==E_Crop_State.Empty;
    }

    public bool IsSown()
    {
        return state == E_Crop_State.Sown;
    }
    public bool IsWatered()
    {
        return state==E_Crop_State.Watered;
    }


}
