using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[CustomEditor(typeof(Chunk))]
public class ChunkEditor : Editor
{
    private void OnSceneGUI()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;

        Chunk chunk= (Chunk)target;
        Handles.Label(chunk.transform.position,chunk.name,style);
    }
}
#endif