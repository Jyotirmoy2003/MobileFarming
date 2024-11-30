using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="cropData" ,menuName ="GAME/Crop Data")]
public class CropData : ScriptableObject
{
   [Header("Setttings")]
   public Crop cropPrefab;
}
