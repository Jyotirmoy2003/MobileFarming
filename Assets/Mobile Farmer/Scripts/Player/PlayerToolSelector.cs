using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using jy_util;
using UnityEngine;
using UnityEngine.UI;



public class PlayerToolSelector : MonoBehaviour
{
    [SerializeField]
    private E_Tool activeTool;
    [Header("Elements")]
    [SerializeField] Image[] toolImage;
    [SerializeField] GameEvent OnPlayerChangeTool;




    [Header("Settiga")]
    [SerializeField] Color selectedColor;

    void Start()
    {
        
    }

    

    public void SelectTool(int toolIndex)
    {
        //set up & Cache new Tool
        activeTool = (E_Tool)toolIndex;

        //when tool changed let other scripts know
        OnPlayerChangeTool.Raise(this,toolIndex);
        
        for(int i=0;i<toolImage.Length;i++)
            toolImage[i].color = (i==toolIndex)? selectedColor : Color.white;
    }


    public bool CanSow()
    {
        return activeTool == E_Tool.Sow;
    }
    public bool CanWater()
    {
        return activeTool == E_Tool.Water;
    }
    public bool CanHarvest()
    {
        return activeTool == E_Tool.Harvest;
    }

}
