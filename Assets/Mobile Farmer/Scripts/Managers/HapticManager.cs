using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

public class HapticManager : MonoSingleton<HapticManager>
{
    public void LightHaptic()
    {
        HapticFeedback.LightFeedback();
    }

    public void MediumHaptic()
    {
        HapticFeedback.MediumFeedback();
    }


}
