using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleButtonEvent : MonoBehaviour
{
    [SerializeField] int index = -1;

    public void ButtonPressed()
    {
        _GameAssets.Instance.OnSimpleUIButtonPressed.Raise(this,index);
    }
}
