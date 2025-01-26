using System;
using System.Collections;

using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerShakeTreeAbility : MonoBehaviour
{
    [Header("Elemesnts")]
    private IShakeable shakeable;
    private PlayerAnimator playerAnimator;
    private GameObject shakebleGameobject;
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
        if((bool)data && sender is IShakeable)
        {
            shakeable = (IShakeable)sender;
            shakebleGameobject = shakeable.IntiateShake(this.gameObject);
            MoveTowardsTarget();
            isAbilityActive = true;
        }else{
            //stop shake mode
            shakeable = null;
            isAbilityActive = false;
            isShaking = false;
            //little delay so that it not get overriden in some devicies
            LeanTween.delayedCall(.1f,()=>playerAnimator.PlayerShakeTreeAnimation(false));
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 treePos = shakebleGameobject.transform.position;
        Vector3 dir = transform.position - treePos;

        Vector3 faltDir = dir;
        faltDir.y = 0;

        Vector3 targetPos = treePos + faltDir.normalized * distanceToTree;

        playerAnimator.ManageAnimation(-faltDir);
        LeanTween.move(gameObject,targetPos, 0.5f).setOnComplete(ReachedtoTarget);

    }

    void ReachedtoTarget(){
        playerAnimator.ManageAnimation(Vector3.zero);
        playerAnimator.PlayerReadyToShake();
    }
        
    



    private void ManagerShakeTree()
    {
        if(!Input.GetMouseButton(0))
        {
            shakeable.StopShaking();
            return;
        }
           

        float shakeMagnitude = Vector2.Distance(Input.mousePosition , previousMousePosition);

        if(ShouldShake(shakeMagnitude))
        {
            Shake();
        }else{
            shakeable.StopShaking();
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
        shakeable.Shake();
        // reset is shaking bool after .5s
        LeanTween.delayedCall(.1f,()=> isShaking = false);
    }
}
