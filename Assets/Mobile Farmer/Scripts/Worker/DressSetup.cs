using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DressSetup : MonoBehaviour
{
   public GameObject wateringCan;
   public GameObject Scythe;
   public GameObject sack;
   public Animator animator;
   public PlayerAnimationEvents playerAnimationEvents;

   [Tooltip("These are used only for the Dressing Room part not in game")]
   public bool isPurched = false;
   public int GemPrice = 40;

   public TMP_Text gemPriceText ;
}
