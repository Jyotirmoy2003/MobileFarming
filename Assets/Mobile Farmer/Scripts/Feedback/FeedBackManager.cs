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
    
   
    void Start()
    {
        if(camRef) compList.Add(camRef);
        if(targetTramform) compList.Add(targetTramform);
        else compList.Add(transform);
        compList.Add(this);
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
        if(isAlreadyPlayingFeedback) return;



        if(playingFeedbackIndexForSeq!=startIndex)
            feedbackList[playingFeedbackIndexForSeq].feedbackFinishedExe-=InitiateFeedbackseq;


        //when its the last feedback
        if(feedbackList[playingFeedbackIndexForSeq]==null)
        {
            playingFeedbackIndexForSeq=-1;
            isAlreadyPlayingFeedback=false;
            //raise event on Complete
            CompletePlayingFeedback?.Invoke();
            return;
        }

       
        feedbackList[playingFeedbackIndexForSeq].PushNeededComponent(compList);
        feedbackList[playingFeedbackIndexForSeq].OnFeedbackActiavte();
        feedbackList[playingFeedbackIndexForSeq].feedbackFinishedExe+=InitiateFeedbackseq;
        playingFeedbackIndexForSeq++;
    }
}