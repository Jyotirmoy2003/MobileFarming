
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform hookTranform;


    
    void Update()
    {
        lineRenderer.SetPosition(1,hookTranform.localPosition);
        //lineRenderer.SetPosition(0,_GameAssets.Instance.fishingRodTip.position);
    }

  

    
}
