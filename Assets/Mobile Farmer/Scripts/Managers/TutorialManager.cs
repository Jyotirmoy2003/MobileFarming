using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    Sequence moveSeq;
    [SerializeField] RectTransform handImageRect,arrowIcon;
    [SerializeField] ParticleSystem sucessFullParticel;
    [SerializeField] GameObject cropFieldLocked,shopLocked,cropFieldTutorialImage;
    private float requireMovementamount=10f;
    private int tutotialIndex=0;



    void Start()
    {
        //if(PlayerPrefs.GetInt("Tutorial",0)==1) SceneManager.LoadScene("Main");
        StartNextTutorial();
        arrowIcon.gameObject.SetActive(false);
    }
    void MoveMentTutorial()
    {
        moveSeq = DOTween.Sequence();

        moveSeq.Append(handImageRect.DOAnchorPos(new Vector2(200f,-430),1f))
        .Append(handImageRect.DOAnchorPos(new Vector2(-20f,-500),1f))
        .Append(handImageRect.DOAnchorPos(new Vector2(-250f,-320),1f));

        moveSeq.Play().SetLoops(-1,LoopType.Restart);
        cropFieldLocked.SetActive(false);
    }

    void UnlockChunkTutorial()
    {
        cropFieldLocked.SetActive(true);
        arrowIcon.gameObject.SetActive(true);

        moveSeq.Append(arrowIcon.DOAnchorPos(new Vector2(360,-250),1f))
        .Append(arrowIcon.DOAnchorPos(new Vector2(360,-200),0.7f));

        moveSeq.Play().SetLoops(-1,LoopType.Restart);

        CashManager.Instance.CreditCoins(200);
    }

    void CropFieldTutorial()
    {
        handImageRect.gameObject.SetActive(false);
        arrowIcon.gameObject.SetActive(false);
        moveSeq.Kill();
        cropFieldTutorialImage.SetActive(true);
    }

    void Done()
    {
        sucessFullParticel.Play();
        tutotialIndex++;
        LeanTween.delayedCall(1f,()=>StartNextTutorial());
       
    }

    void StartNextTutorial()
    {
        switch (tutotialIndex)
        {
            case 0:
                MoveMentTutorial();
                break;
            case 1:
                handImageRect.gameObject.SetActive(false);
                UnlockChunkTutorial();
                break;
            case 2:
                LeanTween.delayedCall(1f,()=>CropFieldTutorial());
                break;
        }
    }

    public void ListenTOMovement(Component sender,object data)
    {
        if(tutotialIndex !=0) return;
        requireMovementamount -= 0.1f;
        if(requireMovementamount <=0)
        {
            //Movement tutorial done
            moveSeq.Kill();
            Done();
        }
    } 

    public void ListenToChunkUnlock(Component sender ,object data)
    {
        if(tutotialIndex != 1) return;
        moveSeq.Kill();
        arrowIcon.gameObject.SetActive(false);

        Done();
    } 
    public void ListenToSellCrops(Component sender, object data)
    {
        if((int)data >= 1)
        {
            Done();
            LeanTween.delayedCall(1,()=>SwitchToMainScene());
        }
    }

    public void OnOkButtonPressed()
    {
        cropFieldTutorialImage.SetActive(false);
        Done();
    } 

    private void SwitchToMainScene()
    {
        PlayerPrefs.SetInt("Tutorial",1);
        SceneManager.LoadScene("Main");
    }
}
