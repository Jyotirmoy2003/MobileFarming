using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject treeCam;
    [SerializeField] Renderer treeRendere;
    [SerializeField] Transform fruitParent;
    [SerializeField] CropData cropData;
    private AppleTreeManager treeManager;

    [Header("Settings")]
    [SerializeField] float maxShakeMagnitude = 0.005f;
    [SerializeField] float shakeIncreament = 0.2f;
    [SerializeField] float furitShakeMultiplayer;
    private float shakeSliderValue = 0f;
    private float shakeMagnitude = 0;
    private bool IsShaking = false;
    private List<Apple> fruitsInTree = new List<Apple>();

    void Start()
    {
        for(int i=0 ;i<fruitParent.childCount;i++)
            fruitsInTree.Add(fruitParent.GetChild(i).GetComponent<Apple>());
        
    }

    public void Initialize(AppleTreeManager treeManager)
    {
        SetTreeCamActivation(true);
        shakeSliderValue = 0;
        this.treeManager = treeManager;
    }



    public void SetTreeCamActivation(bool isActive)
    {
        treeCam.SetActive(isActive);
    }

    public void ShakeTree()
    {
        IsShaking = true;
        TweenShake(maxShakeMagnitude);
        UpdateShakeSlider();
    }

    public void StopShake()
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
        foreach(Apple fruit in fruitsInTree)
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
        treeManager.UpdateShakeSlider(shakeSliderValue);

        
        for(int i=0 ;i < fruitParent.childCount; i++)
        {
            float appleParcent = (float)i/fruitParent.childCount;

            Apple currentApple = fruitsInTree[i];
            if(shakeSliderValue >  appleParcent && !currentApple.IsFree())
                ReleaseApple(currentApple);
        }

        if(shakeSliderValue >=1)
            ExitTree();
        
        
    }

    private void ReleaseApple(Apple fruit)
    {
        fruit.Release();
        _GameAssets.Instance.OnHervestedEvent.Raise(this,cropData);
    }




    private void ExitTree()
    {
        treeManager.EndTreeMode();
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
}
