using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationPlayer : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        Invoke(nameof(SetAnim),0.2f);
    }

    void SetAnim()
    {
        animator.SetInteger("RandomAnim",Random.Range(1,5));
    }

    
}
