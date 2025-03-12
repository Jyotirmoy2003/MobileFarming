using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform hookTranform;
   

    
    void Update()
    {
        lineRenderer.SetPosition(1,hookTranform.localPosition);
    }
}
