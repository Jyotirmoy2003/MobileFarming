using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerDetector : MonoBehaviour
{
    [SerializeField] GameEvent SaveWorldDataEvent;
    public static Action<AppleTree> OnEnterTreezone,OnExitTreezone;
    private void OnTriggerStay(Collider collider)
    {
        if(collider.CompareTag("ChunkTrigger"))
        {
            Chunk chunk= collider.GetComponentInParent<Chunk>();

            chunk?.TryUnlcok();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent<AppleTree>(out AppleTree tree))
        {
            TriggeredAppleTree(tree);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("ChunkTrigger"))
        {
            SaveWorldDataEvent.Raise(this,true);
        }else if(collider.TryGetComponent<AppleTree>(out AppleTree tree))
        {
            ExitAppleTreeZone(tree);
        }
    }


    private void TriggeredAppleTree(AppleTree appleTree)
    {
        OnEnterTreezone?.Invoke(appleTree);
    }

    private void ExitAppleTreeZone(AppleTree appleTree)
    {
        OnEnterTreezone?.Invoke(appleTree);
    }



}
