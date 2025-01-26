using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class Tree : MonoBehaviour,IInteractable,IShakeable
{
    [Header("Elements")]
    [SerializeField] GameObject treeCam;
    [SerializeField] Renderer treeRendere;
    [SerializeField] Transform fruitParent;
    [SerializeField] CropData cropData;
    [SerializeField] ButtonInfo treeButtonInfo;
    //private AppleTreeManager treeManager;

    [Header("Settings")]
    [SerializeField] float maxShakeMagnitude = 0.005f;
    [SerializeField] float shakeIncreament = 0.2f;
    [SerializeField] float furitShakeMultiplayer;
    private float shakeSliderValue = 0f;
    private float shakeMagnitude = 0;
    private bool IsShaking = false;
    private List<Fruit> fruitsInTree = new List<Fruit>();

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
        treeCam.SetActive(isActive);
    }

    private void ShakeTree()
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

        
        for(int i=0 ;i < fruitParent.childCount; i++)
        {
            float appleParcent = (float)i/fruitParent.childCount;

            Fruit currentFruit = fruitsInTree[i];
            if(shakeSliderValue >  appleParcent && !currentFruit.IsFree())
                ReleaseApple(currentFruit);
        }

        if(shakeSliderValue >=1)
            ExitTree();
        
        
    }

    private void ReleaseApple(Fruit fruit)
    {
        fruit.Release();
        _GameAssets.Instance.OnHervestedEvent.Raise(this,cropData);
    }




    private void ExitTree()
    {
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,false);
        SetTreeCamActivation(false);
        TweenShake(0);

        ResstFruits();
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
        if(!IsReady()) return;
        UIManager.Instance.UpdateShakeSlider(0);
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,true);
        IntiateShake(interactingObject);
    }

    public void InIntreactZone()
    {
        if(!IsReady()) return;
        UIManager.Instance.SetupIntreactButton(treeButtonInfo,true);
    }

    public void OutIntreactZone()
    {
        UIManager.Instance.SetupIntreactButton(treeButtonInfo,false);
    }

    public void Shake()
    {
        ShakeTree();
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
    #endregion
}
