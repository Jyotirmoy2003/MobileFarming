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




    [Header("Settiga")]
    [SerializeField] Color selectedColor;

    void Start()
    {
        
    }

    

    public void SelectTool(int toolIndex)
    {
        activeTool = (E_Tool)toolIndex;

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
