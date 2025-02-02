using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCloseInfoUIManager : MonoBehaviour
{
   
    private IInteractable temp_holding_Intreactable;





    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IInteractable>(out temp_holding_Intreactable))
        {
            temp_holding_Intreactable.ShowInfo(true);
        }
    }


    void OnTriggerExit(Collider other)
    {
         if(other.TryGetComponent<IInteractable>(out temp_holding_Intreactable))
        {
            temp_holding_Intreactable.ShowInfo(false);
        }
    }

}
