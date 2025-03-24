using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : FreeSwimming
{
    

   
}


public abstract class FishState
{
    public abstract void EnterState(Fish fish);
    public abstract void UpdateState(Fish fish);

    public abstract void ExitState(Fish fish);

}



public class FishIdle : FishState
{
    public override void EnterState(Fish fish)
    {
        throw new NotImplementedException();
    }

    public override void ExitState(Fish fish)
    {
        throw new NotImplementedException();
    }

    public override void UpdateState(Fish fish)
    {
        throw new NotImplementedException();
    }
}

public class FishMoveToHook : FishState
{
    public override void EnterState(Fish fish)
    {
        throw new NotImplementedException();
    }

    public override void ExitState(Fish fish)
    {
        throw new NotImplementedException();
    }

    public override void UpdateState(Fish fish)
    {
        throw new NotImplementedException();
    }
}


