using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barn : MonoBehaviour
{
    public List<CropField> nearByFields = new List<CropField>();
   
   public CropField GetUnlockedField(CropData cropData)
   {
       return nearByFields[0];
   }
}
