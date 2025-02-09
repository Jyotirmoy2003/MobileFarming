using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIntrector : MonoBehaviour
{
    private IInteractable currentInteractable;
    private IInteractable temp_holding_Intreactable;





    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IInteractable>(out temp_holding_Intreactable))
        {
            if(currentInteractable != temp_holding_Intreactable)
            {
                EnterNewInteractbale(temp_holding_Intreactable);
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
         if(other.TryGetComponent<IInteractable>(out temp_holding_Intreactable))
        {
            ExitInteractable(temp_holding_Intreactable);
        }
    }


    void EnterNewInteractbale(IInteractable interactable)
    {
        currentInteractable = interactable;
        currentInteractable.InIntreactZone(this.gameObject);

    }

    public void Intreact()
    {
        currentInteractable?.Interact(this.gameObject);
        AudioManager.instance.PlaySound("UI_Button");
    }

    void ExitInteractable(IInteractable interactable)
    {
        if(currentInteractable == interactable)
        {
            currentInteractable.OutIntreactZone(this.gameObject);
            currentInteractable = null;
        }
    }
}
