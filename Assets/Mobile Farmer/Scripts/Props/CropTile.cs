using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class CropTile : MonoBehaviour
{
    [SerializeField] //fot teset now its shown in inspector
    private E_Crop_State state;
    [SerializeField] Transform cropParent;







    void Start()
    {
        
    }

   
    public bool IsEmpty()
    {
        return state==E_Crop_State.Empty;
    }

    public void Sow(CropData cropData)
    {
        state=E_Crop_State.Sown;
        Crop crop=Instantiate(cropData.cropPrefab,transform.position,Quaternion.identity,cropParent);
    }
}
