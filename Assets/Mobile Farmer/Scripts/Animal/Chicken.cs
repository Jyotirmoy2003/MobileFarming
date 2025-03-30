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
            Vector3 particelPos = new Vector3(transform.position.x,transform.position.y+1,transform.position.z);

            ParticleSystem tempParticel = Instantiate(eggParticel,particelPos,Quaternion.identity);

            ParticleSystem.Burst burst = tempParticel.emission.GetBurst(0);
            burst.count = eggCout;
            tempParticel.emission.SetBurst(0, burst);
            tempParticel.Play();
            infoUI.SetActivationStatus(false);
            infoUI.canChangeStatus = false;

            HapticManager.Instance.MediumHaptic();
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
        if(val && eggCout>0) infoUI.SetActivationStatus(true);
        infoUI.SetActivationStatus(false);
    }
}
