using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropFieldDataHolder : MonoBehaviour
{
    public Transform chunkTranform;
    public Chunk chunk;
    public CropField cropField;
    public FeedBackManager cropFieldFeedbackManager;
    public BoxCollider cropFieldRangeTrigger;
    public CropFieldMerger cropFieldMerger;
    public CropTile cropTile;
    public InfoUI infoUI;
    public Transform tileParent;
    //[HideInInspector]
    public CropFieldDataHolder right,bottom,left,above,left_above,right_above,left_bottom,right_bottom;
   
}
