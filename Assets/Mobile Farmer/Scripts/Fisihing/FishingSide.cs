using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class FishingSide : MonoBehaviour, IInteractable,IShakeable
{
    [Header("Ref")]
    [SerializeField] List<Fish> allFishes = new List<Fish>();
    [SerializeField] Transform fishTargetWhenHooked1,fishTargetWhenHooked2;
    [SerializeField] Transform cameraLookAt;
    [SerializeField] Transform hookTranform;
    [SerializeField] GameObject rope;
    [SerializeField] Vector3 hookStartPos,hookEndPos;
    [Header("Settings")]
    [SerializeField] float timeToReadyFish = 30f;
    [SerializeField] E_ShakeType needToShake;
    [SerializeField] E_NeedToperformTask_BeforeShake taskBeforeShake;

    [Header ("Camera settings")]
    [SerializeField] Vector3 cameraBodyOffSet;
    [SerializeField] Vector3 cameraAim;

    [Header("UI")]
    [SerializeField] ButtonInfo fishingButtonInfo;


    private PlayerDataHolder playerDataHolder;
    private Fish hookedFish;
    private bool isReady = false;


    public E_ShakeType e_ShakeType { get {return needToShake;} set {needToShake = value;} }
    public E_NeedToperformTask_BeforeShake e_NeedToperformTask_BeforeShake { get {return taskBeforeShake;}  set {taskBeforeShake = value;}  }





    void Start()
    {
       Deactivate();
       Invoke(nameof(MakeFishingsideReady),timeToReadyFish);
    }

    void MakeFishingsideReady()
    {
        isReady = true;
    }


    void Activate()
    {
        foreach(Fish item in allFishes)
        {
            item.gameObject.SetActive(true);
        }
    }

    void Deactivate()
    {
        rope.SetActive(false);
        foreach(Fish item in allFishes)
        {
            item.gameObject.SetActive(false);
        }
    }



    #region INTERFACE


    public void InIntreactZone(GameObject interactingObject)
    {
        if(!interactingObject.CompareTag(_GameAssets.PlayerTag) || !IsReady()) return;
        UIManager.Instance.SetupIntreactButton(fishingButtonInfo,true);
    }

    public void Interact(GameObject interactingObject)
    {
        playerDataHolder = interactingObject.GetComponent<PlayerDataHolder>();

        isReady = false;
         _GameAssets.Instance.OnViewChangeEvent.Raise(this,true);
        CameraManager.Instance.SwitchCamera(cameraLookAt,cameraBodyOffSet,cameraAim);
        UIManager.Instance.SetupIntreactButton(fishingButtonInfo,false);
        
        Activate();
    }


    public void OutIntreactZone(GameObject interactingObject)
    {
        if(!interactingObject.CompareTag(_GameAssets.PlayerTag) || !IsReady()) return;
        UIManager.Instance.SetupIntreactButton(fishingButtonInfo,false);

        
    }
    public GameObject IntiateShake(GameObject gameObject)
    {
        return this.gameObject;
    }

    public void ShowInfo(bool val)
    {
        
    }
    public void Shake(float magnitude)
    {
        hookedFish.ShakeValue(magnitude);
    }


    public void StopShaking()
    {
        hookedFish?.ShakeValue(0);
    }

    public void ReachedtoTarget()
    {
       
        InitateFishing();
    }

    #endregion



    void InitateFishing()
    {
        foreach(Fish item in allFishes) item.ResetFish();


        
        playerDataHolder.playerAnimator.PlayFishingRod(true);
        hookTranform.position = hookStartPos;
       
        Invoke(nameof(FishingRodCasted),1f);
    }
    void FishingRodCasted()
    {
        rope.SetActive(true);
        hookTranform.LeanMove(hookEndPos,2f);
        Invoke(nameof(SetUpFish),2.5f);
    }

    void SetUpFish()
    {
       

         //Hook a fish
        hookedFish = GetARandomFishHooked();
        hookedFish.HookthisFish(hookTranform,fishTargetWhenHooked1,fishTargetWhenHooked2);
        hookedFish.FishHooked += FishHooked;
        hookedFish.FishCatched += FishCatched;
    }

    private void FishCatched()
    {
        hookedFish.FishCatched -= FishCatched;
        rope.SetActive(false);
        CameraManager.Instance.SwitchCamera();
        playerDataHolder.playerAnimator.PlayFishingRod(false);

        Invoke(nameof(MakeFishingsideReady),timeToReadyFish);

    }

    void FishHooked()
    {
        hookedFish.FishHooked -= FishHooked;
        _GameAssets.Instance.OnShakeInitiateEvent.Raise(this,"Catch the Fish!");
    }


    bool IsReady()
    {
        return isReady;
    }

    Fish GetARandomFishHooked()
    {
        return allFishes[Random.Range(0, allFishes.Count)];
    }
}
