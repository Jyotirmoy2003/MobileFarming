using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Billboard))]
[RequireComponent(typeof(CanvasGroup))]
public class InfoUI : MonoBehaviour
{

    private Billboard billboard;
    private CanvasGroup canvasGroup;


    void Start()
    {
        billboard = GetComponent<Billboard>();
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0;
        //gameObject.SetActive(false);
    }
    public void SetActivationStatus(bool val)
    {
        
        SetGameObject(true);
        if(billboard == null) billboard = GetComponent<Billboard>();
        billboard.IsActive = val;

        if(val)canvasGroup.DOFade(1,0.3f);
        else canvasGroup.DOFade(0,0.3f).OnComplete(()=>SetGameObject(false));
    }

    void SetGameObject(bool val)=>gameObject.SetActive(val);

}
