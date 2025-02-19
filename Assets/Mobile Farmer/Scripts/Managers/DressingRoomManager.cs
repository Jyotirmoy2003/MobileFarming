using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DressingRoomManager : MonoSingleton<DressingRoomManager>
{
    [SerializeField] GameEvent onDressSelected;
    [SerializeField] List<DressSetup> dressSetups = new List<DressSetup>();


    public void OnDressSelectd(int index)
    {
        onDressSelected.Raise(this,dressSetups[index]);
    }
}