using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



namespace jy_util{
public class JY_Mono_Utiliy : MonoSingleton<JY_Mono_Utiliy>
{
   NoArgumentFun noArgumentFun,noArgumentFun1,noArgumentFun2,noArgumentFun3;

    public void Delay(NoArgumentFun fun,float time)
    {
        noArgumentFun=fun;
        Invoke(nameof(Execute),time);
    }
    public void Delay1(NoArgumentFun fun,float time)
    {
        noArgumentFun1=fun;
        Invoke(nameof(Execute1),time);
    }

    public void Delay2(NoArgumentFun fun,float time)
    {
        noArgumentFun2=fun;
        Invoke(nameof(Execute2),time);
    }

    public void Delay3(NoArgumentFun fun,float time)
    {
        noArgumentFun3=fun;
        Invoke(nameof(Execute3),time);
    }


    void Execute()=>noArgumentFun();
    void Execute1()=>noArgumentFun1();
    void Execute2()=>noArgumentFun2();
    void Execute3()=>noArgumentFun3();






    
}
public class util{

public void NullFun()
{

}
}

#region Class
[System.Serializable]
public class ButtonInfo{
    public Sprite sprite;
}
[System.Serializable]
public class StorageUIStatus
{
    public E_Inventory_Item_Type Item_Type;
    public Slider slider;
    public TMP_Text text;
    public Image icon;
}
public class WorldData
{
    public List<int> chunkPrices;
    public WorldData()
    {
        chunkPrices = new List<int>();
    }
}
#endregion

public delegate void NoArgumentFun();

#region Structures
[System.Serializable]
public struct BarnItem{
    public E_Inventory_Item_Type item_Type;
    public int maxLoadCapacity;
}

[System.Serializable]
public struct WorkerContiner
{
    public Image img_workerImg;
    public TMP_Text text_workerName;
    public TMP_Text text_workerDescription;
    public TMP_Text text_button;
    public TMP_Text text_amount;
    public Image img_workingCropImg;
}
#endregion

#region ENUMS
public enum E_Crop_State{
        Empty,
        Sown,
        Watered
}
public enum E_Tool{
    None,
    Sow,
    Water,
    Harvest,
}
public enum E_Crop_Type{
    Corn,
    Tomato,
    Carrot,
    Apple,
    Lemon,
    Pumpkin,
}

public enum E_Inventory_Item_Type{
    Corn,
    Tomato,
    Carrot,
    Apple,
    Lemon,
    Pumpkin,
    Milk,
    Fish,
}

public enum E_Crop_Progess{
    Ready,
    Growing,
}

public enum E_ShakeType{
    Any,
    Horizontal,
    Vertical,
}
public enum E_NeedToperformTask_BeforeShake{
    None,
    MovetowardsTarget,
    ReadyFishingRod,
}

public enum E_Worker_State{
    Idle,
    AssignField,
    PerformAction,
    SowField,
    WaterField,
    HervestField,
    LoadoutToBarn,
    WaitForBarnToClear,
}

#endregion


}
