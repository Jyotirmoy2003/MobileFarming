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
    [SerializeField] SeaControl seaControl;
    [SerializeField] Transform hookTranform;
    [SerializeField] GameObject rope;
    [SerializeField] Vector3 hookStartPos,hookEndPos;
    [Header("Settings")]
    [SerializeField] E_ShakeType needToShake;
    [SerializeField] E_NeedToperformTask_BeforeShake taskBeforeShake;

    [Header("UI")]
    [SerializeField] ButtonInfo fishingButtonInfo;


    private PlayerDataHolder playerDataHolder;
    private Fish hookedFish;


    public E_ShakeType e_ShakeType { get {return needToShake;} set {needToShake = value;} }
    public E_NeedToperformTask_BeforeShake e_NeedToperformTask_BeforeShake { get {return taskBeforeShake;}  set {taskBeforeShake = value;}  }










    #region INTERFACE


    public void InIntreactZone(GameObject interactingObject)
    {
        if(!interactingObject.CompareTag(_GameAssets.PlayerTag) || !IsReady()) return;
        UIManager.Instance.SetupIntreactButton(fishingButtonInfo,true);
    }

    public void Interact(GameObject interactingObject)
    {
        playerDataHolder = interactingObject.GetComponent<PlayerDataHolder>();

         _GameAssets.Instance.OnViewChangeEvent.Raise(this,true);
        CameraManager.Instance.SwitchCamera(cameraLookAt,new Vector3(0,-0.45f,-8f),new Vector3(0,1.42f,0));
        
        
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


        rope.SetActive(true);
        playerDataHolder.playerAnimator.PlayFishingRod(true);
        hookTranform.position = hookStartPos;
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

    }

    void FishHooked()
    {
        hookedFish.FishHooked -= FishHooked;
        _GameAssets.Instance.OnShakeInitiateEvent.Raise(this,true);
    }


    bool IsReady()
    {
        return true;
    }

    Fish GetARandomFishHooked()
    {
        return allFishes[Random.Range(0, allFishes.Count)];
    }
}
