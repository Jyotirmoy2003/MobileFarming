using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropField : MonoBehaviour
{
   

    public void SeedCollidedCallback(Vector3[] seedPos)
    {
        Debug.Log("Cropfield recived seed");
    }
}
