using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeChunk : Chunk
{
    [SerializeField] Chunk connectedChunk;
    public override void Unlock()
    {
        base.Unlock();
        connectedChunk.Unlock();
    } 
    
}
