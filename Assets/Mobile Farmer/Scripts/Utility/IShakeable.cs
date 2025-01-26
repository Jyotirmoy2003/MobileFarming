using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShakeable 
{
    GameObject IntiateShake(GameObject gameObject);
    void Shake();
    void StopShaking();
}
