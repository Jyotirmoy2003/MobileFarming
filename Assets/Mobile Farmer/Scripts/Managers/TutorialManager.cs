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
    private float requireMovementamount=10f;
    private int tutotialIndex=0;



    void Start()
    {
        //if(PlayerPrefs.GetInt("Tutorial",0)==1) SceneManager.LoadScene("Main");
        StartNextTutorial();
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

        CashManager.Instance.CreditCoins(200);
    }

    

    void CropFieldTutorial()
    {
        handImageRect.gameObject.SetActive(false);
        arrowIcon.gameObject.SetActive(false);
        moveSeq.Kill();
        cropFieldTutorialImage.SetActive(true);
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
        missionWaypoint.StopWaypoint();

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
    #endregion

    private void SwitchToMainScene()
    {
        PlayerPrefs.SetInt("Tutorial",1);
        AsyncLoadManager.Instance.LoadSceneAsync("Main");
    }

    
}
