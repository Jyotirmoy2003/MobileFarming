using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerDetector : MonoBehaviour
{
    [SerializeField] GameEvent SaveWorldDataEvent;
    private void OnTriggerStay(Collider collider)
    {
        if(collider.CompareTag("ChunkTrigger"))
        {
            Chunk chunk= collider.GetComponentInParent<Chunk>();

            chunk?.TryUnlcok();
        }
    }

  
    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("ChunkTrigger"))
        {
            SaveWorldDataEvent.Raise(this,true);
        }
    }


 



}
