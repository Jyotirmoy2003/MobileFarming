using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandeler : MonoBehaviour
{
    public Action animationEventFired;
    public void EventFired()
    {
        animationEventFired?.Invoke();
    }
}
