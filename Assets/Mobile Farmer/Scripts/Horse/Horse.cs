using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
    [SerializeField] ParticleSystem horseDustParticel;
    [SerializeField] AnimationEventHandeler animationEventHandeler;
    [SerializeField] float RandomTimeDiff = 5f;
    void Start()
    {
        animationEventHandeler.animationEventFired +=OnHorseTouchGround;
    }
    public void OnHorseTouchGround()
    {
        Instantiate(horseDustParticel,transform.position,transform.rotation);
        AudioManager.instance.PlaySound("Horse Foot");
    }

    void PlaySnorAudio()
    {
        if(gameObject.activeSelf)
            AudioManager.instance.PlaySound("Horse_Snort");
    }

    void OnEnable()
    {
        InvokeRepeating(nameof(PlaySnorAudio),4f,Random.Range((10+RandomTimeDiff),(10- RandomTimeDiff)));
    }

    void Osable()
    {
        CancelInvoke();       
    }
}
