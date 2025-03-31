using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    Sequence moveSeq;
    [SerializeField] MissionWaypoint missionWaypoint;
    [SerializeField] RectTransform handImageRect,arrowIcon,arrowContainer;
    [SerializeField] ParticleSystem sucessFullParticel;
    [SerializeField] GameObject cropFieldLocked,shopLocked,cropFieldTutorialImage;
    private bool ListenToEvent=false;
    private float requireMovementamount=5f;
    private int tutotialIndex=0;



    void Start()
    {
        //if(PlayerPrefs.GetInt("Tutorial",0)==1) SceneManager.LoadScene("Main");
        StartNextTutorial();
        CashManager.Instance.ClearCoin();
        arrowIcon.gameObject.SetActive(false);
        arrowContainer.gameObject.SetActive(false);
    }
    #region  TUTORIAL
    void MoveMentTutorial()
    {
        moveSeq = DOTween.Sequence();

        moveSeq.Append(handImageRect.DOAnchorPos(new Vector2(200f,-430),1f))
        .Append(handImageRect.DOAnchorPos(new Vector2(-20f,-500),1f))
        .Append(handImageRect.DOAnchorPos(new Vector2(-250f,-320),1f));

        moveSeq.Play().SetLoops(-1,LoopType.Restart);
        cropFieldLocked.SetActive(false);
        ListenToEvent = true;
    }

    void UnlockChunkTutorial()
    {
        //cropFieldLocked.SetActive(true);
        arrowIcon.gameObject.SetActive(true);

        //show arrow
        missionWaypoint.InitiateWaypoint(cropFieldLocked.transform);

        // moveSeq.Append(arrowIcon.DOAnchorPos(new Vector2(360,-250),1f))
        // .Append(arrowIcon.DOAnchorPos(new Vector2(360,-200),0.7f));
       
        moveSeq = DOTween.Sequence();
        moveSeq.Append(arrowIcon.DOSizeDelta(new Vector2(235,150),1,true)).Append(arrowIcon.DOSizeDelta(new Vector2(235,235),1,true));

        moveSeq.Play().SetLoops(5,LoopType.Restart);

        CashManager.Instance.CreditCoins(3000);
        ListenToEvent =true;

        
    }

    

    void CropFieldTutorial()
    {
        
        handImageRect.gameObject.SetActive(false);
        arrowIcon.gameObject.SetActive(false);
        moveSeq.Kill();
        cropFieldTutorialImage.SetActive(true);
        ListenToEvent = true;
       
    }
    #endregion

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
                LeanTween.delayedCall(3f,()=>CropFieldTutorial());
                break;
        }
    }

    #region  EVENT LISTENER
    public void ListenTOMovement(Component sender,object data)
    {
        if(!ListenToEvent) return;
        if(tutotialIndex !=0) return;
        requireMovementamount -= 0.1f;
        if(requireMovementamount <=0)
        {
            //Movement tutorial done
            moveSeq.Kill();
            requireMovementamount = 100;
            handImageRect.gameObject.SetActive(false);
            ListenToEvent = false;
            AudioManager.instance.PlaySound("Yay");
            LeanTween.delayedCall(1.8f,()=>Done());
        }
    } 

    public void ListenToChunkUnlock(Component sender ,object data)
    {
        if(!ListenToEvent) return;
        if(tutotialIndex != 1) return;
        moveSeq.Kill();
        missionWaypoint.StopWaypoint();
        ListenToEvent = false;
        Done();
    } 
    public void ListenToSellCrops(Component sender, object data)
    {
        if(!ListenToEvent) return;
        if((int)data >= 1)
        {
            AudioManager.instance.PlaySound("Yay");
            ListenToEvent = false;
            LeanTween.delayedCall(1,()=>Done());
            LeanTween.delayedCall(3,()=>SwitchToMainScene());
        }
    }

    public void OnOkButtonPressed()
    {
        cropFieldTutorialImage.SetActive(false);
        AudioManager.instance.PlaySound("UI_Button");
        AudioManager.instance.PlaySound("Win");
        ListenToEvent = true;
        Done();
    } 
    #endregion

    private void SwitchToMainScene()
    {
        PlayerPrefs.SetInt("Tutorial",1);
        AsyncLoadManager.Instance.LoadSceneAsync("Main");
    }

    
}
