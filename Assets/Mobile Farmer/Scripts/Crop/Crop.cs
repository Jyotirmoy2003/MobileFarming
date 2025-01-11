using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] GameObject cropRendererObject;
    private CropData myCropData;
    void Start()
    {
        
    }

    public void ScaleUp()
    {
        cropRendererObject.LeanScale(Vector3.one,myCropData.growDuration).setEase(LeanTweenType.easeOutBack);
    }

    public void PushData(CropData cropData)
    {
        myCropData = cropData;
    }
}
