using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class FishingSide : MonoBehaviour, IInteractable,IShakeable
{
    [Header("Ref")]
    [SerializeField] Transform cameraLookAt;
    [SerializeField] SeaControl seaControl;
    [SerializeField] Transform hookTranform;
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
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,true);
        CameraManager.Instance.SwitchCamera(cameraLookAt,new Vector3(0,-0.45f,-8f),new Vector3(0,1.42f,0));
        playerDataHolder = interactingObject.GetComponent<PlayerDataHolder>();
        InitateFishing();
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
        
    }


    public void StopShaking()
    {
        
    }

    #endregion



    void InitateFishing()
    {
        playerDataHolder.playerAnimator.PlayFishingRod(true);
        hookTranform.position = hookStartPos;
        hookTranform.LeanMove(hookEndPos,2f);
        //Hook a fish
        hookedFish = seaControl.GetARandomFishHooked();
    }

    void FishHooked()
    {

    }


    bool IsReady()
    {
        return true;
    }
}
