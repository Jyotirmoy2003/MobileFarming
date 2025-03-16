using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
//using UnityEngine.Rendering.Universal;

[SaveDuringPlay]
public class FeedBackManager : MonoBehaviour
{
    [HideInInspector] public CinemachineVirtualCamera camRef;
    [HideInInspector] public Transform targetTramform;

    [Space]
    [Header("Settings")]
    public bool overrideRemaps=false;
    [HideInInspector] public float curveZeroRemap=0f;
    [HideInInspector] public float curveOneRemap=0f;
    [SerializeField] bool isSequencialFlow=true;
    [SerializeField] int startIndex=0;

    [Space]
    [Header("Feedbacks")]
    public List<FeedbackBase> feedbackList=new List<FeedbackBase>();


    private int maxDurationFeedbackindex = 0;
    private int playingFeedbackIndexForSeq=-1;
    private bool isAlreadyPlayingFeedback=false;
    public Action CompletePlayingFeedback;
    public UnityEvent UEvent_completePlayingFeedback;
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
            if(item == null)
            {
                Debug.Log("<color=yellow>Feedback List is Invalid</color>");
                continue;
            }
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
        maxDurationFeedbackindex = 0;
        float lastLongestDuration = -1;
        for(int i=startIndex;i<feedbackList.Count;i++)
        {
            if(tempInsteanceOfFeedback[i].duration > lastLongestDuration)
            {
                maxDurationFeedbackindex = i;
            }
            tempInsteanceOfFeedback[i].PushNeededComponent(compList);
            tempInsteanceOfFeedback[i].OnFeedbackActiavte();
        }
        tempInsteanceOfFeedback[maxDurationFeedbackindex].feedbackFinishedExe += CompletePlayingFeedbackNonSequ;
        CompletePlayingFeedback?.Invoke();
    }

    void CompletePlayingFeedbackNonSequ()
    {
        tempInsteanceOfFeedback[maxDurationFeedbackindex].feedbackFinishedExe -= CompletePlayingFeedback;
        CompletePlayingFeedback?.Invoke();
    }

    void InitiateFeedbackseq()
    {
        if(playingFeedbackIndexForSeq!=startIndex)
            tempInsteanceOfFeedback[playingFeedbackIndexForSeq-1].feedbackFinishedExe-=InitiateFeedbackseq;

       
        //when its the last feedback
        if(tempInsteanceOfFeedback.Count<=playingFeedbackIndexForSeq)
        {
           
            playingFeedbackIndexForSeq=-1;
            isAlreadyPlayingFeedback=false;
            //raise event on Complete
            CompletePlayingFeedback?.Invoke();
            UEvent_completePlayingFeedback?.Invoke();
            return;
        }

       
        tempInsteanceOfFeedback[playingFeedbackIndexForSeq].PushNeededComponent(compList);
        tempInsteanceOfFeedback[playingFeedbackIndexForSeq].OnFeedbackActiavte();
        tempInsteanceOfFeedback[playingFeedbackIndexForSeq].feedbackFinishedExe+=InitiateFeedbackseq;
        playingFeedbackIndexForSeq++;
    }
}