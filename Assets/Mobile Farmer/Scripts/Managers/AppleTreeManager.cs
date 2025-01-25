using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AppleTreeManager : MonoBehaviour
{
    private AppleTree lastTree;
   [SerializeField] GameEvent onTreeModeStarted;
   [Header("Elements")]
   [SerializeField] Slider shakeSlider;







    void Start()
    {
        Subcribe(true);
    }

    void OnDestroy()
    {
        Subcribe(false);
    }

    void Subcribe(bool Subcribe)
    {
        if(Subcribe)
        {
            PlayerDetector.OnEnterTreezone += EnteredTreeZoneCallback;
            PlayerDetector.OnExitTreezone += ExitTreeZoneCallback;
        }else{
            PlayerDetector.OnEnterTreezone -= EnteredTreeZoneCallback;
            PlayerDetector.OnExitTreezone -= ExitTreeZoneCallback;
        }
    }

   

    public void AppleTreeButtonCallback()
    {
        lastTree?.Initialize(this);
        
        //Set slider back to 0 again
        UpdateShakeSlider(0);
        onTreeModeStarted.Raise(lastTree,true);
    }

    public void EnteredTreeZoneCallback(AppleTree appleTree)
    {
        lastTree = appleTree;
    }

    public void ExitTreeZoneCallback(AppleTree appleTree)
    {
        lastTree = null;
    }



    public void UpdateShakeSlider(float value)
    {
        shakeSlider.value = value;
    }
}
