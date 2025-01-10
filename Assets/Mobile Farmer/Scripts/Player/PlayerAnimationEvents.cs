using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerAnimationEvents : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] ParticleSystem seedParticle;
    [SerializeField] ParticleSystem waterParticle;


    void Start()
    {
        
    }

    private void PlaySeedParticle()
    {
        seedParticle.Play();
    }

    private void PlayWaterAnimation()
    {
        waterParticle.Play();
    }
  
}
