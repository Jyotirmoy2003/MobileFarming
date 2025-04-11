using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
    [SerializeField] AnimationEventHandeler animationEventHandeler;
    [SerializeField] AudioSource footAudio;
    void Start()
    {
        animationEventHandeler.animationEventFired +=OnHorseTouchGround;
    }
    public void OnHorseTouchGround()
    {
        
        AudioManager.instance.PlaySound("Horse Foot");
    }
}
