using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] PlayerAnimator playerAnimator;
    private CharacterController characterController;

    [Header("Settings")]
    [SerializeField] float moveSpeed=5f;
    void Start()
    {
        characterController=GetComponent<CharacterController>();
    }

    void Update()
    {
        
    }

    

    void ManageMovement(Vector3 moveVector)
    {
        moveVector=moveVector *moveSpeed *Time.deltaTime / Screen.width;
        moveVector.z=moveVector.y;
        moveVector.y=0;
        
        characterController?.Move(moveVector);
        //setup animation
        playerAnimator.ManageAnimation(moveVector);
    }


    public void ListenToJoystickInputEvent(Component sender,object data)
    {
        if(data is Vector3)
        {
            
            ManageMovement((Vector3)data);
        }
    }
}
