using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualManager : MonoSingleton<PlayerVisualManager>
{
   [SerializeField] GameObject playerRenderer;
   [SerializeField] Transform horseTransform;
   [SerializeField] Transform horseModePlayerPos;
   [SerializeField] Transform groundPlayerpos;
   [SerializeField] List<ParticleSystem> spawnParticel = new List<ParticleSystem>();
   [SerializeField] Animator horseAnimator;
   [SerializeField] 

   public Action<Vector3> ManagerHorseAnim;

   public void SetPlayerRendererShowStatus(bool isShow)
   {
      playerRenderer.SetActive(isShow);
   }

   public void ListnToOnHorseModeChanged(Component sender,object data)
   {
      HorseVisual((bool)data);
   }

   void HorseVisual(bool isActive)
   {
      if(isActive)
      {
         playerRenderer.transform.position = horseModePlayerPos.position;
         horseTransform.gameObject.SetActive(true);
         ManagerHorseAnim += ManageHorseMovement;

      }else{
         playerRenderer.transform.position = groundPlayerpos.position;
         horseTransform.gameObject.SetActive(false);
         ManagerHorseAnim -= ManageHorseMovement;
      }
      foreach(var item in spawnParticel) item.Play();
   }

   void ManageHorseMovement(Vector3 moveVector)
   {
      if(moveVector.magnitude>0)
      {
         horseTransform.forward = moveVector.normalized;
      }
      horseAnimator.SetFloat("WalkMagnitude",moveVector.magnitude);
   }
}
