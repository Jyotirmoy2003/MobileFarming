using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomMovement))]
public class AnimalBase : MonoBehaviour
{
    [Header("Elemesnts")]
    [SerializeField] Chunk alocatedChunk;
    [SerializeField] FeedBackManager feedBackManager;
    private RandomMovement randomMovement;

    [Header("Settings")]
    [SerializeField] protected bool canInteract =false;
    [SerializeField]float idelTimeMin=5f;
    [SerializeField] float idelTimeMAX=5f;
    private bool isMoveing=true;
    protected bool shouldStartMovement = true;

    void Start()
    {
        randomMovement = GetComponent<RandomMovement>();
        randomMovement.OnReachOnDestination += OnReachToOneDest;
        alocatedChunk.chunkUnlocked += OnUnlockedMyChunk;
        Invoke(nameof(Init),3f);
    }


    void Init()
    {
          
          if(alocatedChunk.IsUnclocked())
          {
               randomMovement.StopAndStartMovement(false);
               feedBackManager.CompletePlayingFeedback -= Init;
          }
    }



    void OnDisable()
    {
        if(randomMovement)randomMovement.OnReachOnDestination -= OnReachToOneDest;
    }

   void OnReachToOneDest()
   {
        if(!isMoveing) return;
        isMoveing = false;
        randomMovement.StopAndStartMovement(true);
        LeanTween.delayedCall(Random.Range(idelTimeMin,idelTimeMAX), ()=>StartMovemnt() );
   }
   void StartMovemnt()
   {
        if(!shouldStartMovement) return; //dont start  movemnt if its not needed from child class
        randomMovement.StopAndStartMovement(false);
        isMoveing = true;
        
   }

   protected void StopMovement()
   {
        LeanTween.cancel(this.gameObject);
        randomMovement.StopAndStartMovement(true);
        shouldStartMovement = false;
   }
   

   void OnUnlockedMyChunk()
   {
          Debug.Log("Chunk unlocaked");
          alocatedChunk.chunkUnlocked -= OnUnlockedMyChunk;
          Instantiate(_GameAssets.Instance.spawnDustParticel,transform);
          feedBackManager.PlayFeedback();
          feedBackManager.CompletePlayingFeedback += Init;
   }




}
