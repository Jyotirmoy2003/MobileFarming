using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPListenHandeler : MonoSingleton<IAPListenHandeler>
{
    [SerializeField] GameEvent OnIApDoneEvent;
    public void OnProductPurches(Product product)
    {
        OnIApDoneEvent.Raise(this,product);
    }
}
