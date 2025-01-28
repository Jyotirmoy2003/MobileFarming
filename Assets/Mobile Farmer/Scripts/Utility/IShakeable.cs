using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public interface IShakeable 
{
    public E_ShakeType e_ShakeType{ get; set; }
    GameObject IntiateShake(GameObject gameObject);
    void Shake(float magnitude);
    void StopShaking();
}
