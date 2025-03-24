using System;
using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;

public class MobileJoystick : MonoSingleton<MobileJoystick>
{
    [Header("Events")]
    [SerializeField] GameEvent JoystickInputEvent;
    [Header("Elements")]
    [SerializeField] RectTransform joystickOutline;
    [SerializeField] RectTransform joystickKnob;
    [Header("Settings")]
    [SerializeField] float moveFactor=540f;

    private Vector3 clickedPosition;
    private Vector3 move;
    private Action updateAction=util.NullFun;

    void Start()
    {
        HideJoystick();
    }

    // Update is called once per frame
    void Update()=>updateAction();
   

    public void ClickedOnJoystickZoneCallBack()
    {
        move=Vector3.zero;
        clickedPosition=Input.mousePosition;
        joystickOutline.position=clickedPosition;

        ShowJoystick();
    }

    private void ShowJoystick()
    {
        joystickOutline.gameObject.SetActive(true);
        SetControl(true);
    }

    private void HideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        SetControl(false);
        move=Vector3.zero;
        JoystickInputEvent?.Raise(this,move);
    }

    private void ControlJoystick()
    {
        Vector3 currentPositon=Input.mousePosition;
        Vector3 direction=currentPositon-clickedPosition;

        //Clamp knob inside outline
        float moveMagnitude= direction.magnitude * moveFactor / Screen.width;
        moveMagnitude=MathF.Min(moveMagnitude,joystickOutline.rect.width / 2);

        move=direction.normalized * moveMagnitude;
        Vector3 targetPosition = clickedPosition +move;

        joystickKnob.position=targetPosition;
        //raise event 
        JoystickInputEvent.Raise(this,move);
        if(Input.GetMouseButtonUp(0))
        {
            HideJoystick();
        }
    }

    public void SetControl(bool canControl)
    {
        if(canControl)
        {
            updateAction=ControlJoystick;
        }else{
            updateAction=util.NullFun;
        }
    }
}
