using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseModeManager : MonoBehaviour
{
    [SerializeField] GameEvent   OnHorseModechanged;

    [NaughtyAttributes.Button]
    public void OnHorseButtonpressed()
    {
        OnHorseModechanged.Raise(this,true);
    }
    [NaughtyAttributes.Button]
    public void OnHorseButtonpressedOff()
    {
        OnHorseModechanged.Raise(this,false);
    }
}
