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
        initialScale = cropRendererObject.transform.localScale;
    }

    public void ScaleUp()
    {
        cropRendererObject.LeanScale(Vector3.one,myCropData.growDuration).setEase(LeanTweenType.easeOutBack);
    }
    public void ScaleDown()
    {
        cropRendererObject.LeanScale(Vector3.zero,1)
            .setEase(LeanTweenType.easeOutBack).setOnComplete(()=>LeanTween.delayedCall(1,DeactivateCrop)); //deactivate crop after 1s delay so particel gets time to fully get played

        //harvestParticel.transform.parent = null;  as we are going ro reuse same crop again and again
        harvestParticel.Play();
    }

    public void PushData(CropData cropData)
    {
        myCropData = cropData;
    }

    public void ResetScale()
    {
        cropRendererObject.transform.localScale = initialScale;
    }

    void DeactivateCrop()
    {
        gameObject.SetActive(false);
    }
}
