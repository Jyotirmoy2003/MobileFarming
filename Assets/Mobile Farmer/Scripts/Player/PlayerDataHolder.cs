using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHolder : MonoBehaviour
{
    public bool isPlayer = false;
    public PlayerAnimator playerAnimator;
    public PlayerAnimationEvents playerAnimationEvents;
    public Transform hervestSphere;
    [Header ( "Particel")]
    public SeedParticle seedParticle;
    public WaterParticle waterParticle;

    public void CacheNewVisual(DressSetup dressSetup)
    {
        playerAnimationEvents = dressSetup.playerAnimationEvents;
        playerAnimationEvents.CacheData();
    }
}
