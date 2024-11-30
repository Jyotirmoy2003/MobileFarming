using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class FeedBackManager : MonoBehaviour
{
    [SerializeField] bool isSequencialFlow=true;
    [SerializeField] CinemachineVirtualCamera camRef;
    [SerializeField] int startIndex=0;
    [SerializeField] List<FeedbackBase> feedbackList=new List<FeedbackBase>();
   
    void Start()
    {
        
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isSequencialFlow)
            {
                
            }else
                InitiateFeedback();
        }
    }

    void InitiateFeedback()
    {
        
        for(int i=startIndex;i<feedbackList.Count;i++)
        {
            feedbackList[i].PushNeededComponent(camRef);
            feedbackList[i].OnFeedbackActiavte();
        }

        
    }

    void InitiateFeedbackseq()
    {

    }
}
