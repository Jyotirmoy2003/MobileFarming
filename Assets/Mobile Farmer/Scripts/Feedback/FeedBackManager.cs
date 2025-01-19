using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.Universal;

[SaveDuringPlay]
public class FeedBackManager : MonoBehaviour
{
    [Header("Ref")]
    [HideInInspector] public CinemachineVirtualCamera camRef;
    [HideInInspector] public Transform targetTramform;

    [Space]
    [Header("Settings")]
    [SerializeField] bool isSequencialFlow=true;
    [SerializeField] int startIndex=0;

    [Space]
    [Header("Feedbacks")]
    public List<FeedbackBase> feedbackList=new List<FeedbackBase>();



    private int playingFeedbackIndexForSeq=-1;
    private bool isAlreadyPlayingFeedback=false;
    public Action CompletePlayingFeedback;
    private List<Component> compList=new List<Component>();
    private List<FeedbackBase> tempInsteanceOfFeedback=new List<FeedbackBase>();
   
    void Start()
    {
        CopyList();

        if(camRef) compList.Add(camRef);
        if(targetTramform) compList.Add(targetTramform);
        else compList.Add(transform);
        compList.Add(this);

    }

    void CopyList()
    {
        foreach(FeedbackBase item in feedbackList)
        {
            FeedbackBase tempHoldingFeedback = item.CloneMe();
            if(tempHoldingFeedback)
                tempInsteanceOfFeedback.Add(tempHoldingFeedback);
        }
    }
    public void PlayFeedback()
    {
        if(feedbackList.Count<=0){
            Debug.Log("No feedback to play");
            return;
        }
        if(isSequencialFlow)
        {
            playingFeedbackIndexForSeq=startIndex;
            InitiateFeedbackseq();
        }else
            InitiateFeedback();
    }



    void InitiateFeedback()
    {
        
        for(int i=startIndex;i<feedbackList.Count;i++)
        {
            feedbackList[i].PushNeededComponent(compList);
            feedbackList[i].OnFeedbackActiavte();
        }

        CompletePlayingFeedback?.Invoke();
    }

    void InitiateFeedbackseq()
    {
        if(playingFeedbackIndexForSeq!=startIndex)
            tempInsteanceOfFeedback[playingFeedbackIndexForSeq-1].feedbackFinishedExe-=InitiateFeedbackseq;

        Debug.Log("New sequeence valid: "+(tempInsteanceOfFeedback.Count<=playingFeedbackIndexForSeq));
        Debug.Log("Feedback Playing :"+playingFeedbackIndexForSeq);
        //when its the last feedback
        if(tempInsteanceOfFeedback.Count<=playingFeedbackIndexForSeq)
        {
            Debug.Log("Feedback Comeplete");
            playingFeedbackIndexForSeq=-1;
            isAlreadyPlayingFeedback=false;
            //raise event on Complete
            CompletePlayingFeedback?.Invoke();
            return;
        }

        
        tempInsteanceOfFeedback[playingFeedbackIndexForSeq].PushNeededComponent(compList);
        tempInsteanceOfFeedback[playingFeedbackIndexForSeq].OnFeedbackActiavte();
        tempInsteanceOfFeedback[playingFeedbackIndexForSeq].feedbackFinishedExe+=InitiateFeedbackseq;
        playingFeedbackIndexForSeq++;
    }
}
