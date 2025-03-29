using System.Collections;
using System.Collections.Generic;
using jy_util;
using Unity.VisualScripting;
using UnityEngine;

public class Tree : MonoBehaviour,IInteractable,IShakeable
{
    [Header("Elements")]
    [SerializeField] Renderer treeRendere;
    [SerializeField] Transform fruitParent;
    [SerializeField] CropData cropData;
    [SerializeField] ButtonInfo treeButtonInfo;
    public E_NeedToperformTask_BeforeShake e_NeedToperformTask_BeforeShake { get {return taskBeforeShake;} set {taskBeforeShake = value;}} 

    [Header("Settings")]
    [SerializeField] E_ShakeType needToShake;
    [SerializeField] E_NeedToperformTask_BeforeShake taskBeforeShake;
    [SerializeField] float maxShakeMagnitude = 0.005f;
    [SerializeField] float shakeIncreament = 0.2f;
    [SerializeField] float furitShakeMultiplayer;

    [Header("Camera settings")]
    [SerializeField] Vector3 cameraBodyOffset;
    [SerializeField] Vector3 cameraAimOffset;
    private float shakeSliderValue = 0f;
    private float shakeMagnitude = 0;
    private bool IsShaking = false;
    private List<Fruit> fruitsInTree = new List<Fruit>();
    private PlayerDataHolder playerDataHolder;

    public E_ShakeType e_ShakeType { get { return needToShake;} set { needToShake = value;} }

    void Start()
    {
        for(int i=0 ;i<fruitParent.childCount;i++)
            fruitsInTree.Add(fruitParent.GetChild(i).GetComponent<Fruit>());
        
    }

    public void Initialize()
    {
        SetTreeCamActivation(true);
        shakeSliderValue = 0;
    }



    public void SetTreeCamActivation(bool isActive)
    {
        if(isActive)CameraManager.Instance.SwitchCamera(treeRendere.transform,cameraBodyOffset,cameraAimOffset);
        else CameraManager.Instance.SwitchCamera();
    }

    private void StartShakeTree()
    {
        IsShaking = true;
        TweenShake(maxShakeMagnitude);
        UpdateShakeSlider();
    }

    private void StopShake()
    {
        if(!IsShaking) return;

        IsShaking = false;
        TweenShake(0);
    }

    private void TweenShake(float targetMagnitude)
    {
        LeanTween.cancel(treeRendere.gameObject);
        LeanTween.value(treeRendere.gameObject,UpdateMaterial,shakeMagnitude,targetMagnitude,1);
    }

    private void UpdateMaterial(float magnitude)
    {
        shakeMagnitude = magnitude;
        treeRendere.material.SetFloat("_Magnitude",shakeMagnitude);
        foreach(Fruit fruit in fruitsInTree)
        {
            //dont shake if apple is on the groudn
            if(fruit.IsFree())
                continue;
            fruit.Shake(shakeMagnitude * furitShakeMultiplayer);
        }
    }

    private void UpdateShakeSlider()
    {
        shakeSliderValue += shakeIncreament;
        UIManager.Instance.UpdateShakeSlider(shakeSliderValue);

        //Make apple fall from tree
        for(int i=0 ;i < fruitParent.childCount; i++)
        {
            float appleParcent = (float)i/fruitParent.childCount;

            Fruit currentFruit = fruitsInTree[i];
            if(shakeSliderValue >  appleParcent && !currentFruit.IsFree())
                ReleaseApple(currentFruit);
        }

        //stop when reached max
        if(shakeSliderValue >=1)
            ExitTree();
        
        
    }

    private void ReleaseApple(Fruit fruit)
    {
        fruit.Release();
        _GameAssets.Instance.OnHervestedEvent.Raise(playerDataHolder,cropData);
    }




    private void ExitTree()
    {
        _GameAssets.Instance.playerAnimator.PlayerShakeTreeAnimation(false);
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,false);
        _GameAssets.Instance.OnShakeInitiateEvent.Raise(this,"");
        SetTreeCamActivation(false);
        TweenShake(0);

        ResstFruits();

        
         //little delay so that it not get overriden in some devicies
        LeanTween.delayedCall(.1f,()=>playerDataHolder.playerAnimator.PlayerShakeTreeAnimation(false));
    }

    private void ResstFruits()
    {
        foreach(var fruit in fruitsInTree)
            fruit.Reset();
    }

    public bool IsReady()
    {
        foreach (var fruit in fruitsInTree)
        if(!fruit.IsReady()) return false;
        return true;
    }

    #region INTERFACE
    public void Interact(GameObject interactingObject)
    {
        if(!interactingObject.CompareTag(_GameAssets.PlayerTag)) return;

        playerDataHolder = interactingObject.GetComponent<PlayerDataHolder>();

        if(!IsReady()) return;
        UIManager.Instance.UpdateShakeSlider(0);

        //call events Change view activate shake
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,true);
        _GameAssets.Instance.OnShakeInitiateEvent.Raise(this,"Shake the Tree!");


        
        IntiateShake(interactingObject);
    }

    public void InIntreactZone(GameObject interactingObject)
    {
        if(!interactingObject.CompareTag(_GameAssets.PlayerTag)) return;
        if(!IsReady()) return;
        UIManager.Instance.SetupIntreactButton(treeButtonInfo,true);
    }

    public void OutIntreactZone(GameObject interactingObject)
    {
        if(!interactingObject.CompareTag(_GameAssets.PlayerTag)) return;
        UIManager.Instance.SetupIntreactButton(treeButtonInfo,false);
    }





    public void ReachedtoTarget()
    {
        playerDataHolder.playerAnimator.PlayerReadyToShake();
    }
    public void Shake(float magnitude)
    {
        _GameAssets.Instance.playerAnimator.PlayerShakeTreeAnimation(true);
        StartShakeTree();
    }

    public GameObject IntiateShake(GameObject gameObject)
    {
        Initialize();
        return this.gameObject;
    }

    public void StopShaking()
    {
        UIManager.Instance.SetupIntreactButton(treeButtonInfo,false);
        StopShake();
    }

    public void ShowInfo(bool val)
    {
       
    }
    #endregion
}
