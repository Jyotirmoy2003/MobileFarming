using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

public class HapticManager : MonoSingleton<HapticManager>
{
    bool canHapitc = true;

    public void LightHaptic()
    {
        if(!canHapitc) return;
        HapticFeedback.LightFeedback();
    }

    public void MediumHaptic()
    {
        if(!canHapitc) return;
        HapticFeedback.MediumFeedback();
    }

    public void SetHapticStatus(bool canHapitc)
    {
        this.canHapitc = canHapitc;
    }
}
