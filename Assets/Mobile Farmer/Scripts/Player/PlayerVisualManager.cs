using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualManager : MonoSingleton<PlayerVisualManager>
{
   [SerializeField] GameObject playerRenderer;


 

   public void SetPlayerRendererShowStatus(bool isShow)
   {
        playerRenderer.SetActive(isShow);
   }
}
