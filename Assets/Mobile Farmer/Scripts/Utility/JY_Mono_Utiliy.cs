using System.Collections;
using System.Collections.Generic;
using UnityEngine;



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
[System.Serializable]
public class ButtonInfo{
    public Sprite sprite;
}
public delegate void NoArgumentFun();

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
}

public enum E_Crop_Progess{
    Ready,
    Growing,
}

public class WorldData
{
    public List<int> chunkPrices;
    public WorldData()
    {
        chunkPrices = new List<int>();
    }
}

}
