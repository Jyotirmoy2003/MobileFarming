using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkWalls : MonoBehaviour
{
   [SerializeField] GameObject frontWall;
   [SerializeField] GameObject rightWall;
   [SerializeField] GameObject backWall;
   [SerializeField] GameObject leftwall;


   public void Configur(int data)
   {
        frontWall.SetActive(IsKthBitSet(data,0));
        rightWall.SetActive(IsKthBitSet(data,1));
        backWall.SetActive(IsKthBitSet(data,2));
        leftwall.SetActive(IsKthBitSet(data,3));
   }

   private bool IsKthBitSet(int configuration, int k)
   {
        return !((configuration & (1 << k)) > 0);
        
   }
}
