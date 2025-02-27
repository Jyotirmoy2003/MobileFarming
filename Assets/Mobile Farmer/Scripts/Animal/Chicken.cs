using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : AnimalBase,IInteractable
{

    [SerializeField] int eggCout = 5;
    [SerializeField] int maxEggCap = 10;

    [SerializeField] InfoUI infoUI;
    [SerializeField] ParticleSystem eggParticel;
    [SerializeField] float eggReproduceTime = 10f;

    private bool isEggReady = false;




    void Start()
    {
        Invoke(nameof(EggReady),eggReproduceTime);
    }

   




    void DropEggs()
    {
        if(isEggReady)
        {
            isEggReady = false;

            ParticleSystem.Burst burst = eggParticel.emission.GetBurst(0);
            burst.count = eggCout;
            eggParticel.emission.SetBurst(0, burst);
            eggParticel.Play();
            infoUI.SetActivationStatus(false);
            infoUI.canChangeStatus = false;
            Invoke(nameof(AddEggstoInventory),2f);

        }
    }

    void AddEggstoInventory()
    {
        InventoryManager.Instance.AddItemToInventory(jy_util.E_Inventory_Item_Type.Egg,eggCout);
        eggCout = 0;
        
    }

    void EggReady()
    {
        isEggReady = true;
        eggCout += 1;
        infoUI.canChangeStatus = false;
        
        if(eggCout >= maxEggCap)
        {
            eggCout = maxEggCap;
        }else{
            Invoke(nameof(EggReady),eggReproduceTime);
        }
    }







    public void InIntreactZone(GameObject interactingObject)
    {
        DropEggs();
    }

    public void Interact(GameObject interactingObject)
    {
        
    }

    public void OutIntreactZone(GameObject interactingObject)
    {
        
    }

    public void ShowInfo(bool val)
    {
        infoUI.SetActivationStatus(val);
    }
}
