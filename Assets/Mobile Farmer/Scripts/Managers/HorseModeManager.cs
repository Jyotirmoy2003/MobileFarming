using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseModeManager : MonoBehaviour
{
    [SerializeField] GameEvent   OnHorseModechanged;
    private bool isHorseActive = false;

    [NaughtyAttributes.Button]
    public void OnHorseButtonpressed()
    {  
        //toggle horese mode
        OnHorseModechanged.Raise(this,isHorseActive = !isHorseActive);
    }
    
}
