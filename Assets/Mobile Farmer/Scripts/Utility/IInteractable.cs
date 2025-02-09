using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public interface IInteractable 
{
    
    void Interact(GameObject interactingObject);
    void InIntreactZone(GameObject interactingObject);
    void OutIntreactZone(GameObject interactingObject);

    void ShowInfo(bool val);
    
}
