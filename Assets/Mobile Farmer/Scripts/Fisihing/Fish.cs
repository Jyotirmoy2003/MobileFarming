using System;
using System.Collections;
using System.Collections.Generic;
using jy_util;
using Unity.Mathematics;
using UnityEngine;

public class Fish : FreeSwimming
{
    #region STATES
    public FishIdle fishIdleState = new FishIdle();
    public FishMoveToHook fishMoveToHookState = new FishMoveToHook();
    public FishRegistHook fishRegistHookState = new FishRegistHook();

    public FishState currentState;
    #endregion

    public FeedBackManager feedBackManager;
    public E_Inventory_Item_Type fishType;


    [HideInInspector]
    public Transform hookPos;
    [HideInInspector]
    public Transform fishTargetWhenHooked1,fishTargetWhenHooked2;
    public Action FishHooked,FishCatched;
    private float shakeThresold = 350f;
    public float shakeValue = 0f;

    [HideInInspector]
    public float shakeSliderValue = 0f;
    [HideInInspector]
    public float surfaceY = 3f;

    private Coroutine moveCoroutine; // Store reference to running coroutine


    void Start()
    {
        shakeThresold = _GameAssets.Instance.fishShakeThresold;
        currentState = fishIdleState;
        currentState.EnterState(this);
    }


    void Update()
    {
        base.Update();
        currentState.UpdateState(this);
    }

    public void ResetFish()
    {
        transform.localScale = Vector3.one;
        SwithState(fishIdleState);
    }


    public void HookthisFish(Transform hookPos,Transform fishTargetWhenHooked1,Transform fishTargetWhenHooked2)
    {
        this.hookPos = hookPos;
        this.fishTargetWhenHooked1 = fishTargetWhenHooked1;
        this.fishTargetWhenHooked2 = fishTargetWhenHooked2;

        SwithState(fishMoveToHookState);
    }

    

    public void ShakeValue(float magnitude)
    {
        shakeValue = magnitude;

        if (shakeThresold < magnitude)
        {
            SetMovementActivation(false);

            // Start movement coroutine if it's not already running
            if (moveCoroutine == null)
            {
                moveCoroutine = StartCoroutine(SmoothMoveUp());
            }
        }
        else
        {
            SetMovementActivation(true);

            // Stop movement when shaking stops
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }
    }

    // Coroutine for smooth movement
    private IEnumerator SmoothMoveUp()
    {
        float baseMoveSpeed = 2f; // Base speed
        float maxMoveAmount = 2f; // Maximum movement per shake
        float minMoveAmount = 0.5f; // Minimum movement per shake
        

            while (true)
            {
                // Calculate move amount based on shake magnitude
                float moveAmount = Mathf.Lerp(minMoveAmount, maxMoveAmount, (shakeValue - shakeThresold) / (10f - shakeThresold));

                // Move fish upward
                Vector3 target = new Vector3(transform.position.x, transform.position.y + moveAmount, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, target, baseMoveSpeed * Time.deltaTime);

                // Normalize progress so that when fish reaches y = 3, progress is 1
                //float progress = Mathf.Clamp01((transform.position.y - startY) / (surfaceY - startY)); 
                

                yield return null; // Wait for next frame
            }
        }



   public void SwithState(FishState nextState)
   {
        currentState.ExitState(this);
        nextState.EnterState(this);
        currentState = nextState;
        
    
   }

   public void UpdateShakeSlider()
   {
        shakeSliderValue = (transform.localPosition.y/surfaceY);
        UIManager.Instance.UpdateShakeSlider(shakeSliderValue);

        if(shakeSliderValue > 1)
        {
            //Fish catched
            feedBackManager.PlayFeedback();
            StopAllCoroutines();
            
            FishCatched?.Invoke();

            SwithState(fishIdleState);
            SetMovementActivation(false);
            InventoryManager.Instance.AddItemToInventory(fishType,1);

        }
   }
}


public abstract class FishState
{
    public abstract void EnterState(Fish fish);
    public abstract void UpdateState(Fish fish);

    public abstract void ExitState(Fish fish);

}



public class FishIdle : FishState
{
    private Fish fish;
    public override void EnterState(Fish fish)
    {
        this.fish = fish;
        
         //Set new Speed;
        fish.maxSpeed =0.2f;
        fish.minSpeed = 0.1f;
        fish.rotationSpeed = 0.3f;

        fish.SetRandomTargetPosition();
        fish.SetMovementActivation(true);
    }

    public override void ExitState(Fish fish)
    {
        
    }

    public override void UpdateState(Fish fish)
    {
        
    }
}

public class FishRandomMovement : FishState
{
    private Fish fish;
    public override void EnterState(Fish fish)
    {
        this.fish = fish;
        fish.SetRandomTargetPosition();
        fish.SetMovementActivation(true);
    }

    public override void ExitState(Fish fish)
    {
        
    }

    public override void UpdateState(Fish fish)
    {
        
    }
}

public class FishMoveToHook : FishState
{
    private Fish fish;
    public override void EnterState(Fish fish)
    {
        this.fish = fish;
        fish.RandomMovementActivation(false);
        
        //set up for hook position
        fish.onFishReachedToDest += FishreachedToHook; 
        fish.SetTargetPosition(fish.hookPos.position);
        //Set new Speed;
        fish.maxSpeed =0.6f;
        fish.minSpeed = 0.4f;
        fish.rotationSpeed = 0.6f;
    }

    public override void ExitState(Fish fish)
    {
        
    }

    public override void UpdateState(Fish fish)
    {
        
    }

    void FishreachedToHook()
    {
        fish.onFishReachedToDest -= FishreachedToHook;
        fish.FishHooked?.Invoke();
        //fish.hookPos.SetParent(fish.transform);

        fish.SwithState(fish.fishRegistHookState);
    }
}

public class FishRegistHook : FishState
{
    private Fish fish;
    private bool isTaregtis1 = true;
    public override void EnterState(Fish fish)
    {
        this.fish = fish;

         //Set new Speed;
        fish.maxSpeed =0.8f;
        fish.minSpeed = 0.5f;
        fish.rotationSpeed = 0.6f;


        fish.onFishReachedToDest += FishReachedToTarget;
        isTaregtis1 = true;
        fish.SetTargetPosition(fish.fishTargetWhenHooked1.position);
        fish.SetMovementActivation(true);
    }

    public override void ExitState(Fish fish)
    {
        fish.onFishReachedToDest -= FishReachedToTarget;
        

        //call events to change the view and turn off slider
        _GameAssets.Instance.OnViewChangeEvent.Raise(fish,false);
        _GameAssets.Instance.OnShakeInitiateEvent.Raise(fish,"");
    }

    public override void UpdateState(Fish fish)
    {
        fish.hookPos.position = fish.transform.position;
        fish.UpdateShakeSlider();

        
    }

    void FishReachedToTarget()
    {
        if(isTaregtis1)
        {
            fish.SetTargetPosition(fish.fishTargetWhenHooked2.position);
        }else{
            fish.SetTargetPosition(fish.fishTargetWhenHooked1.position);
        }

        isTaregtis1 = !isTaregtis1;
    }
}


