using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] PlayerAnimator playerAnimator;
    [SerializeField] PlayerVisualManager playerVisualManager;
    private CharacterController characterController;

    [Header("Settings")]
    [SerializeField] float normalMovespeed=50f;
    [SerializeField] float interactMovespeed=40f;
    [SerializeField] float horseSpeed = 100f;
    private float moveSpeed;
    void Start()
    {
        moveSpeed = normalMovespeed;
        characterController=GetComponent<CharacterController>();
    }


    

    void ManageMovement(Vector3 moveVector)
    {
        moveVector=moveVector *moveSpeed *Time.deltaTime / Screen.width;
        moveVector.z=moveVector.y;
        moveVector.y=0;
        //setup animation
        playerAnimator.ManageAnimation(moveVector);
        playerVisualManager.ManagerHorseAnim?.Invoke(moveVector);

        moveVector.y=-1;
        characterController?.Move(moveVector);
    }


    public void ListenToJoystickInputEvent(Component sender,object data)
    {
        if(data is Vector3)
        {
            
            ManageMovement((Vector3)data);
        }
    }

    public void ListenToPlayerInteracting(Component sender, object data)
    {
        if((bool)data)
        {
            moveSpeed = interactMovespeed;
        }else{
            moveSpeed = normalMovespeed;
        }
    }

    public void ListenTOPlayerHorseModeChange(Component sender,object data)
    {
        HorseModeStart((bool)data);
    }

    void HorseModeStart(bool isStart)
    {
        moveSpeed = isStart? horseSpeed : normalMovespeed;
    }
}
