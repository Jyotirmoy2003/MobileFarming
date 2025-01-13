using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] GameObject cropRendererObject;
    [SerializeField] ParticleSystem harvestParticel;
    private CropData myCropData;
    void Start()
    {
        
    }

    public void ScaleUp()
    {
        cropRendererObject.LeanScale(Vector3.one,myCropData.growDuration).setEase(LeanTweenType.easeOutBack);
    }
    public void ScaleDown()
    {
        cropRendererObject.LeanScale(Vector3.zero,1)
            .setEase(LeanTweenType.easeOutBack).setOnComplete(()=>Destroy(this.gameObject));

        harvestParticel.transform.parent = null;
        harvestParticel.Play();
    }

    public void PushData(CropData cropData)
    {
        myCropData = cropData;
    }
}
