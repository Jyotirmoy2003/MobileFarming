using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(FeedBackManager))]
public class FDM_Editor_Script : Editor
{
    FeedBackManager feedBackManager;
    private bool isTranformRefRequired = false;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This Script is used for giving multiple type of feedback juice to your game",MessageType.Info);
        DrawDefaultInspector();


        feedBackManager = (FeedBackManager)target;
        //GUILayoutOption playbuttonOption=new GUILayoutOption()
        if(GUILayout.Button("Play Feedback",  GUILayout.Height(40)))
        {
            feedBackManager.PlayFeedback();
        }

        #region SHOW HIDE Variables

        //show camera ref field when there is camera type feedback in list
        if(CheckForCameraRef())
        {
            feedBackManager.camRef = (CinemachineVirtualCamera)EditorGUILayout.ObjectField(
                "Cinemachine Camera ref", 
                feedBackManager.camRef, 
                typeof(CinemachineVirtualCamera), 
                true);
        }

        if(isTranformRefRequired=CheckForTransformRef())
        {
            feedBackManager.targetTramform = (Transform)EditorGUILayout.ObjectField(
                "Taeget Transform ref", 
                feedBackManager.targetTramform, 
                typeof(Transform), 
                true);
        }

        if(isTranformRefRequired && feedBackManager.targetTramform == null)
        {
            feedBackManager.targetTramform = feedBackManager.transform;
        }

        if(feedBackManager.overrideRemaps)
        {
            feedBackManager.curveOneRemap = EditorGUILayout.FloatField("Curve One Remap",feedBackManager.curveOneRemap);
            feedBackManager.curveZeroRemap = EditorGUILayout.FloatField("Curve Zero Remap",feedBackManager.curveZeroRemap);
        }
        #endregion

         // Apply changes to the serialized object
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        
    }


    #region Ref valid checker
    bool CheckForCameraRef()
    {
        foreach(FeedbackBase item in feedBackManager.feedbackList)
            if(item is FB_Camera)
                return true;

        return false;
        
    }
    bool CheckForTransformRef()
    {
        foreach(FeedbackBase item in feedBackManager.feedbackList)
            if(item is FB_Transform)
                return true;
        return false;
    }

    bool CheckForPPVolumeRef()
    {
        foreach(FeedbackBase item in feedBackManager.feedbackList)
            if(item is FB_PostProcess)
                return true;
        return false;
    }
    #endregion

}
