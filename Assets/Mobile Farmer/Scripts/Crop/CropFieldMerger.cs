using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using jy_util;
using UnityEngine;

public class CropFieldMerger : MonoBehaviour
{
    [SerializeField] CropFieldDataHolder myDataHolder;
    [SerializeField] List<CropFieldDataHolder> connectedFields = new List<CropFieldDataHolder>();
    [Range(0.1f,5f)]
    [SerializeField] float initateTime = 3f;
    public int priority = 10;
    public bool IsConnected = false;
    private bool calledByChunkUnlocking=false;


    void Start()
    {
        if(!myDataHolder.cropField) Destroy(this);
        LeanTween.delayedCall(2f,()=>SetUpList());
        //SetUpList();
    }
    



   void SetUpList()
   {
        calledByChunkUnlocking = false;
        if(myDataHolder.right?.cropField != null) connectedFields.Add(myDataHolder.right);
        if(myDataHolder.left?.cropField != null) connectedFields.Add(myDataHolder.left)  ;
        if(myDataHolder.above?.cropField != null) connectedFields.Add(myDataHolder.above)  ;
        if(myDataHolder.bottom?.cropField != null) connectedFields.Add(myDataHolder.bottom)  ;
        
        //float delay = Random.Range(0,3f);
        float delay = initateTime;
        
        LeanTween.delayedCall(delay,()=>TryToMerge());
   }




    public void TryToMerge()
    {
       
        if(!myDataHolder.chunk.IsUnclocked()) return;

       
      
        for(int i=0 ; i<connectedFields.Count ; i++)
        {
            if(connectedFields[i] == null) continue;

            if(!connectedFields[i].chunk.IsUnclocked()) continue;
            
            if(connectedFields[i].cropFieldMerger.IsConnected) continue;

            if(connectedFields[i].cropField.GetCropData().cropType != myDataHolder.cropField?.GetCropData().cropType) continue;

            Merge(i);

        }
    }

    void Merge(int index)
    {
        //set both merger to connected 
        connectedFields[index].cropFieldMerger.IsConnected = true;
        IsConnected = true;

        if(connectedFields[index].chunkTranform.position.x > myDataHolder.chunkTranform.position.x && connectedFields[index].chunkTranform.position.z == myDataHolder.chunkTranform.position.z)
        {
            //Connect to Right Side
            
            //Setup Collider
            myDataHolder.cropFieldRangeTrigger.size = new Vector3(9,1,4);
            myDataHolder.cropFieldRangeTrigger.center = new Vector3(2.5f,0,0);

            myDataHolder.infoUI.transform.localPosition = new Vector3(2.5f,2,0);
            
            StartCoroutine(CreateTileRightSide(index));

            Destroy(connectedFields[index].cropField);
            Destroy(connectedFields[index].cropFieldRangeTrigger);
            Destroy(connectedFields[index].infoUI.gameObject);

        }else if(connectedFields[index].chunkTranform.position.x == myDataHolder.chunkTranform.position.x && connectedFields[index].chunkTranform.position.z > myDataHolder.chunkTranform.position.z)
        {
            //Connect Above

            //Setup Collider
            myDataHolder.cropFieldRangeTrigger.size = new Vector3(4,1,9);
            myDataHolder.cropFieldRangeTrigger.center = new Vector3(0,0,2.5f);

            myDataHolder.infoUI.transform.localPosition = new Vector3(0f,2,2.5f);
            StartCoroutine(CreateTileAboveSide(index));

            Destroy(connectedFields[index].cropField);
            Destroy(connectedFields[index].cropFieldRangeTrigger);
            Destroy(connectedFields[index].infoUI.gameObject);

        }else if(connectedFields[index].chunkTranform.position.x < myDataHolder.chunkTranform.position.x && connectedFields[index].chunkTranform.position.z == myDataHolder.chunkTranform.position.z)
        {
            //Connect Left

            //Setup Collider
            myDataHolder.cropFieldRangeTrigger.size = new Vector3(9,1,4);
            myDataHolder.cropFieldRangeTrigger.center = new Vector3(-2.5f,0,0);

            myDataHolder.infoUI.transform.localPosition = new Vector3(-2.5f,2,0);
            StartCoroutine(CreateTileLeftSide(index));

            Destroy(connectedFields[index].cropField);
            Destroy(connectedFields[index].cropFieldRangeTrigger);
            Destroy(connectedFields[index].infoUI.gameObject);

        }else{
            //Connect Bottom

            //Setup Collider
            myDataHolder.cropFieldRangeTrigger.size = new Vector3(4,1,9);
            myDataHolder.cropFieldRangeTrigger.center = new Vector3(0f,0,-2.5f);

            myDataHolder.infoUI.transform.localPosition = new Vector3(0,2,-2.5f);
            StartCoroutine(CreateTileBottomSide(index));

            Destroy(connectedFields[index].cropField);
            Destroy(connectedFields[index].cropFieldRangeTrigger);
            Destroy(connectedFields[index].infoUI.gameObject);

        }
    }

    IEnumerator CreateTileRightSide(int index)
    {
        //Create new Tile ans set position
        for(int i=2; i<4;i++)
        {
            for(int j=-1;j<2;j++)
            {
                CropTile tempCroptile=Instantiate(myDataHolder.cropTile,myDataHolder.tileParent);
                tempCroptile.transform.localPosition = new Vector3(i,0,j);

                yield return new WaitForSeconds(0.1f);
                tempCroptile.feedBackManager?.PlayFeedback();

            }
        }
        //set both merger to connected 
        connectedFields[index].cropFieldMerger.IsConnected = true;
        IsConnected = true;

        //Do repranting
        while(connectedFields[index].tileParent.childCount>0)
        {
            connectedFields[index].tileParent.GetChild(0).transform.SetParent(myDataHolder.tileParent);
        }
        myDataHolder.cropField.MergeDone(calledByChunkUnlocking);
    }
    IEnumerator CreateTileLeftSide(int index)
    {
        //Create new Tile ans set position
        for(int i=-2; i>-4;i--)
        {
            for(int j=-1;j<2;j++)
            {
                CropTile tempCroptile=Instantiate(myDataHolder.cropTile,myDataHolder.tileParent);
                tempCroptile.transform.localPosition = new Vector3(i,0,j);

                yield return new WaitForSeconds(0.1f);
                tempCroptile.feedBackManager?.PlayFeedback();

            }
        }
        //set both merger to connected 
        connectedFields[index].cropFieldMerger.IsConnected = true;
        IsConnected = true;

        //Do repranting
        while(connectedFields[index].tileParent.childCount>0)
        {
            connectedFields[index].tileParent.GetChild(0).transform.SetParent(myDataHolder.tileParent);
        }
        myDataHolder.cropField.MergeDone(calledByChunkUnlocking);
    }

    IEnumerator CreateTileAboveSide(int index)
    {
        //Create new Tile ans set position
        for(int i=-1; i<2;i++)
        {
            for(int j=2;j<4;j++)
            {
                CropTile tempCroptile=Instantiate(myDataHolder.cropTile,myDataHolder.tileParent);
                tempCroptile.transform.localPosition = new Vector3(i,0,j);

                yield return new WaitForSeconds(0.1f);
                tempCroptile.feedBackManager?.PlayFeedback();

            }
        }
        //set both merger to connected 
        connectedFields[index].cropFieldMerger.IsConnected = true;
        IsConnected = true;

        //Do repranting
        while(connectedFields[index].tileParent.childCount>0)
        {
            connectedFields[index].tileParent.GetChild(0).transform.SetParent(myDataHolder.tileParent);
        }
        myDataHolder.cropField.MergeDone(calledByChunkUnlocking);
    }

    IEnumerator CreateTileBottomSide(int index)
    {
        //Create new Tile ans set position
        for(int i=-1; i<2;i++)
        {
            for(int j=-2;j>-4;j--)
            {
                CropTile tempCroptile=Instantiate(myDataHolder.cropTile,myDataHolder.tileParent);
                tempCroptile.transform.localPosition = new Vector3(i,0,j);

                yield return new WaitForSeconds(0.1f);
                tempCroptile.feedBackManager?.PlayFeedback();

            }
        }
        
        

        //Do repranting
        while(connectedFields[index].tileParent.childCount>0)
        {
            connectedFields[index].tileParent.GetChild(0).transform.SetParent(myDataHolder.tileParent);
        }
        myDataHolder.cropField.MergeDone(calledByChunkUnlocking);

        yield return new WaitForSeconds(.2f);
        switch(myDataHolder.cropField.state)
        {
            case E_Crop_State.Sown:
                myDataHolder.cropField.InstantlySowTile();
                break;
            case E_Crop_State.Watered:
                myDataHolder.cropField.InstantlyWaterTile();
                break;
        }
    }

   
    public void ListnToChunkUnlocked(Component sender,object data)
    {
        if(!myDataHolder.chunk.IsUnclocked()) return; //When this chunk is locked dont listen to event
        calledByChunkUnlocking = true;

        SelectMerger();
    }

    void SelectMerger()
    {
        for(int i=0 ; i<connectedFields.Count ; i++)
        {
            if(connectedFields[i] == null) continue;

            if(!connectedFields[i].chunk.IsUnclocked()) continue;
            
            if(connectedFields[i].cropFieldMerger.IsConnected) continue;

            if(connectedFields[i].cropField.GetCropData().cropType != myDataHolder.cropField?.GetCropData().cropType) continue;

            if(connectedFields[i].cropFieldMerger.priority >= priority) connectedFields[i].cropFieldMerger.TryToMerge();
            else{
                Merge(i);
            }

        }
    }

 

}
