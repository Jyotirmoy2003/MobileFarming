using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public interface IInteractable 
{
    
    void Interact(GameObject interactingObject);
    void InIntreactZone();
    void OutIntreactZone();

    void ShowInfo(bool val);
    
}
