using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerShakeTreeAbility : MonoBehaviour
{
    [Header("Elemesnts")]
    private AppleTree currentTree;
    private PlayerAnimator playerAnimator;
    [Header("Settings")]
    [SerializeField] float distanceToTree=1f;
    [Range(0f,0.5f)]
    [SerializeField] float shakeThresold=0.05f;

    private Vector2 previousMousePosition;
    private bool isAbilityActive=false,isShaking=false;


    void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
    }
    void Update()
    {
        if (isAbilityActive && !isShaking) ManagerShakeTree();
    }

    public void ListenToTreeModeStartEvent(Component sender,object data)
    {
        if((bool)data && sender is AppleTree)
        {
            currentTree = (AppleTree)sender;
            MoveTowardsTree();
            isAbilityActive = true;
        }
    }

    private void MoveTowardsTree()
    {
        Vector3 treePos = currentTree.transform.position;
        Vector3 dir = transform.position - treePos;

        Vector3 faltDir = dir;
        faltDir.y = 0;

        Vector3 targetPos = treePos + faltDir.normalized * distanceToTree;


        playerAnimator.ManageAnimation(-faltDir);
        LeanTween.move(gameObject,targetPos, 0.5f).setOnComplete(ReachedtoTree);

    }

    void ReachedtoTree()=>playerAnimator.ManageAnimation(Vector3.zero);



    private void ManagerShakeTree()
    {
        if(!Input.GetMouseButton(0))
        {
            currentTree.StopShake();
            return;
        }
           

        float shakeMagnitude = Vector2.Distance(Input.mousePosition , previousMousePosition);

        if(ShouldShake(shakeMagnitude))
        {
            Shake();
        }else{
            currentTree.StopShake();
        }


        previousMousePosition = Input.mousePosition;
    }

    private bool ShouldShake(float shakeMagnitude)
    {
        float screenParcent = shakeMagnitude/Screen.width;

        return screenParcent >= shakeThresold;
    }

    private void Shake()
    {
        isShaking = true;
        playerAnimator.PlayerShakeTreeAnimation(true);
        currentTree.ShakeTree();
        // reset is shaking bool after .5s
        LeanTween.delayedCall(.1f,()=> isShaking = false);
    }
}
