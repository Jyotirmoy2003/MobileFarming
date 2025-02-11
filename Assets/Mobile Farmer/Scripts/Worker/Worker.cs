using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using jy_util;
using Unity.VisualScripting;
using UnityEditor;

public class Worker : MonoBehaviour
{
    #region STATES
    public WorkerBase currentState ;
    public AssignField assignFieldState = new AssignField();
    public WaitForBarnToClear waitForBarnToClearState = new WaitForBarnToClear();
    public PerformAction performActionState = new PerformAction();
    public HarvestField harvestFieldState = new HarvestField();
    public SowField sowFieldState = new SowField();
    public WaterField waterFieldState = new WaterField();

    #endregion



    public workerStat workerStat;
    public CropField assignedCropField;
    public CropFieldDataHolder cropFieldDataHolder;

    [Header("Elements")]
    public NavMeshAgent navMeshAgent;
    [SerializeField] PlayerDataHolder playerDataHolder;



    private Action onDelayDone;
    private int carringCrop = 0;
    void Start()
    {
        
        currentState = assignFieldState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState( WorkerBase nextState)
    {
        currentState.ExitState(this);
        nextState.EnterState(this);
        currentState = nextState;
    }


    public void StartTimmer(float time, Action onComplete)
    {
        onDelayDone = onComplete;
        Invoke(nameof(Delay),time);
    }

    void Delay()=>onDelayDone?.Invoke();

    public void ListenToonCropHervested(Component sender,object data)
    {
        if((sender as PlayerDataHolder)== playerDataHolder) //when this worker hervested the crop
        {
            carringCrop += (data as CropData).amountinSingleCrop;

            if(carringCrop >= workerStat.maxLoadCapacity)
            {
                carringCrop = workerStat.maxLoadCapacity;
                currentState.ListenToEvent(sender,data);
            }
        }
    }

}






public abstract class WorkerBase 
{
    public abstract void EnterState(Worker wk);
    public abstract void UpdateState(Worker wk);
    public abstract void ExitState(Worker wk);
    public abstract void ListenToEvent(Component sender,object data, int id,Worker wk);
    public abstract void ListenToEvent(Component sender,object data);
    public abstract void StartTime(String coroutineName,Action OnComplete);

}





public class AssignField : WorkerBase
{
    Worker worker;
    public override void EnterState(Worker wk)
    {
        worker = wk;
        worker.navMeshAgent.SetDestination(worker.assignedCropField.transform.position); //for testing let it be here
    }

    public override void ExitState(Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        if(id == 1) //Cached Event
        {
            worker.navMeshAgent.SetDestination(worker.assignedCropField.transform.position);
        }
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        if(worker.navMeshAgent.remainingDistance <= 0.1f)
        {
            worker.SwitchState(worker.performActionState);
        }
    }



}


public class PerformAction : WorkerBase
{
    private Worker worker;
    public override void EnterState(Worker wk)
    {
        worker = wk;
        worker.StartTimmer(3f,DelayDone);
        
    }

    public override void ExitState(Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        
    }
    void DelayDone()
    {
        SelectNextState();
    }

    void SelectNextState()
    {
        switch(worker.assignedCropField.state)
        {
            case E_Crop_State.Empty:
            worker.SwitchState(worker.sowFieldState);
            break;
            case E_Crop_State.Sown:
            worker.SwitchState(worker.waterFieldState);
            break;
            case E_Crop_State.Watered:
            worker.SwitchState(worker.harvestFieldState);
            break;
        }
    }
}



public class SowField : WorkerBase
{
    Worker worker;
    CropFieldDataHolder dataHolder;
    int index = -1;
    bool isDone = false;


    public override void EnterState(Worker wk)
    {
        isDone = false;
        index = -1;
        worker = wk;
        dataHolder = wk.cropFieldDataHolder;
        SetNextDest();
        worker.assignedCropField.onFullySown += onCompleteSow;
        worker.assignedCropField.Interact(worker.gameObject);
        worker.navMeshAgent.speed = worker.workerStat.moveSpeedWhileWorking;
    }

    public override void ExitState(Worker wk)
    {
        worker.navMeshAgent.speed = worker.workerStat.walkSpeed;
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        if(worker.navMeshAgent.remainingDistance <=0.1f)
        {
            SetNextDest();
        }
    }

    void SetNextDest()
    {
        if(isDone)
        {
            worker.SwitchState(worker.performActionState);
        }
        index=(index+1)%dataHolder.cropField.cropTiles.Count;

        //target next undone tile
        int maxTryCount = 10; //to avoid infinte loop

        while(dataHolder.cropField.cropTiles[index].IsSown() && (maxTryCount--  > 0))
        {
            index=(index+1)%dataHolder.cropField.cropTiles.Count;
        }
        
        worker.navMeshAgent.SetDestination(dataHolder.cropField.cropTiles[index].transform.position);
    }

    void onCompleteSow(CropField cropField)
    {
        //when compete sowing unsubcribe
        worker.assignedCropField.onFullySown -= onCompleteSow;
        isDone = true;
    }
}

public class WaterField : WorkerBase
{
    Worker worker;
    CropFieldDataHolder dataHolder;
    int index = -1;
    bool isDone = false;
    public override void EnterState(Worker wk)
    {
        isDone = false;
        index = -1;
        worker = wk;
        dataHolder = wk.cropFieldDataHolder;
        SetNextDest();
        worker.assignedCropField.onFullyWatered += onCompleteWater;
        worker.assignedCropField.Interact(worker.gameObject);
        worker.navMeshAgent.speed = worker.workerStat.moveSpeedWhileWorking;
    }

    public override void ExitState(Worker wk)
    {
        worker.navMeshAgent.speed = worker.workerStat.walkSpeed;
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        if(worker.navMeshAgent.remainingDistance <=0.1f)
        {
            SetNextDest();
        }
    }

    void SetNextDest()
    {
        if(isDone)
        {
            worker.SwitchState(worker.performActionState);
        }
        index=(index+1)%dataHolder.cropField.cropTiles.Count;

        //target next undone tile
        int maxTryCount = 10; //to avoid infinte loop

        while(dataHolder.cropField.cropTiles[index].IsWatered() && (maxTryCount--  > 0))
        {
            index=(index+1)%dataHolder.cropField.cropTiles.Count;
        }
        worker.navMeshAgent.SetDestination(dataHolder.cropField.cropTiles[index].transform.position);
    }

    void onCompleteWater(CropField cropField)
    {
        //when compete sowing unsubcribe
        worker.assignedCropField.onFullyWatered -= onCompleteWater;
        isDone = true;
    }
}

public class HarvestField : WorkerBase
{
    Worker worker;
    CropFieldDataHolder dataHolder;
    int index = -1;
    bool isDone = false;
    public override void EnterState(Worker wk)
    {
        isDone = false;
        index = -1;
        worker = wk;
        dataHolder = wk.cropFieldDataHolder;
        SetNextDest();
        worker.assignedCropField.OnFullyHarvested += onCompleteHervest;
        worker.assignedCropField.Interact(worker.gameObject);
        worker.navMeshAgent.speed = worker.workerStat.walkSpeed+1;
    }

    public override void ExitState(Worker wk)
    {
        worker.navMeshAgent.speed = worker.workerStat.walkSpeed;
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        if(worker.navMeshAgent.remainingDistance <=0.1f)
        {
            SetNextDest();
        }
    }

    void SetNextDest()
    {
        if(isDone)
        {
            worker.SwitchState(worker.performActionState);
        }
        index=(index+1)%dataHolder.cropField.cropTiles.Count;
        //target next undone tile
        int maxTryCount = 10; //to avoid infinte loop

        while(dataHolder.cropField.cropTiles[index].IsEmpty() && (maxTryCount--  > 0))
        {
            index=(index+1)%dataHolder.cropField.cropTiles.Count;
        }
        worker.navMeshAgent.SetDestination(dataHolder.cropField.cropTiles[index].transform.position);
    }

    void onCompleteHervest(CropField cropField)
    {
        //when compete  unsubcribe
        worker.assignedCropField.OnFullyHarvested -= onCompleteHervest;
        isDone = true;
    }
}

public class LoadoutToBarn : WorkerBase
{
    public override void EnterState(Worker wk)
    {
        
    }

    public override void ExitState(Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        
    }
}

public class WaitForBarnToClear : WorkerBase
{
    public override void EnterState(Worker wk)
    {
        
    }

    public override void ExitState(Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data, int id, Worker wk)
    {
        
    }

    public override void ListenToEvent(Component sender, object data)
    {
        
    }

    public override void StartTime(string coroutineName, Action OnComplete)
    {
        
    }

    public override void UpdateState(Worker wk)
    {
        
    }
}







