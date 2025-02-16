using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


[RequireComponent(typeof(Billboard))]
//[RequireComponent(typeof(CanvasGroup))]
public class InfoUI : MonoBehaviour
{

    private Billboard billboard;
    private CanvasGroup canvasGroup;
    private SpriteRenderer spriteRenderer;
    public bool canChangeStatus = true;
    private Color fullColor = new Color(1,1,1,1);
    private Color zeroColor = new Color(1,1,1,0);


    void Start()
    {
        billboard = GetComponent<Billboard>();
        canvasGroup = GetComponent<CanvasGroup>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(canvasGroup)canvasGroup.alpha = 0;
        //gameObject.SetActive(false);
    }
    public void SetActivationStatus(bool val)
    {
        if(!canChangeStatus) return;
        if(canvasGroup == null)
        {
            ActivationSprite(val);
            
        }else{
            ActivationCanvas(val);
        }

        
        
    }

    void ActivationSprite(bool val)
    {
        SetGameObject(true);
        if(billboard == null) billboard = GetComponent<Billboard>();
        billboard.IsActive = val;
        if(val)
            DOVirtual.Color(zeroColor, fullColor, 0.3f, (value) =>
            {
                spriteRenderer.color = value;
            });
        else
            DOVirtual.Color(fullColor, zeroColor, 0.3f, (value) =>
            {
                spriteRenderer.color = value;
            }).OnComplete(()=>SetGameObject(false));;
    }

    void ActivationCanvas(bool val)
    {
        SetGameObject(true);
        if(billboard == null) billboard = GetComponent<Billboard>();
        billboard.IsActive = val;

        if(val)canvasGroup.DOFade(1,0.3f);
        else canvasGroup.DOFade(0,0.3f).OnComplete(()=>SetGameObject(false));
    }

    void SetGameObject(bool val)=>gameObject.SetActive(val);

}
