using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public interface IShakeable 
{
    public E_ShakeType e_ShakeType{ get; set; }
    public E_NeedToperformTask_BeforeShake e_NeedToperformTask_BeforeShake{ get; set; }

    void ReachedtoTarget();    
    GameObject IntiateShake(GameObject gameObject);
    void Shake(float magnitude);
    void StopShaking();
}
