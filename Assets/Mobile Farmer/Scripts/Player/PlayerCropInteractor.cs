using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCropInteractor : MonoBehaviour
{
    [SerializeField] Material[] matrials;
    private Transform myTranform;

    void Start()
    {
        myTranform = transform;
    }
    void Update()
    {
        for (int i = 0;i < matrials.Length; i++)
        {
            matrials[i].SetVector("_Player_Position",myTranform.position);
        }
    }
}
