using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] GameObject cropRendererObject;
    [SerializeField] ParticleSystem harvestParticel;
    private CropData myCropData;
    private Vector3 initialScale ;

    void Start()
    {
        initialScale = transform.localScale;
    }

    public void ScaleUp()
    {
        cropRendererObject.LeanScale(Vector3.one,myCropData.growDuration).setEase(LeanTweenType.easeOutBack);
    }
    public void ScaleDown()
    {
        cropRendererObject.LeanScale(Vector3.zero,1)
            .setEase(LeanTweenType.easeOutBack).setOnComplete(()=>gameObject.SetActive(false));

        harvestParticel.transform.parent = null;
        harvestParticel.Play();
    }

    public void PushData(CropData cropData)
    {
        myCropData = cropData;
    }

    public void ResetScale()
    {
        transform.localScale = initialScale;
    }
}
