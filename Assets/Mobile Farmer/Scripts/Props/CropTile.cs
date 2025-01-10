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
        crop=Instantiate(cropData.cropPrefab,transform.position,Quaternion.identity,cropParent);
    }
    public void Water(CropData cropData)
    {
        state=E_Crop_State.Watered;
        tileMeshrenderer.material=_GameAssets.Instance.wateredCropTileMat;

        //Let  the crop grow
        crop.ScaleUp();
    }
}
