using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Renderer appleRenderer;

    public void Shake(float magnitude)
    {
        appleRenderer.material.SetFloat("_Magnitude", magnitude);
    }
}
