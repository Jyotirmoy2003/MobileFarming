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
    public Transform tileParent;
    //[HideInInspector]
    public CropFieldDataHolder right,bottom,left,above;
}
