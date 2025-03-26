using System;
using System.Collections;
using jy_util;
using UnityEditor;
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
    private E_ShakeType neededShakeType;


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
            neededShakeType = shakeable.e_ShakeType;
            

            isAbilityActive = true;
        }else{
            //stop shake mode
            shakeable = null;
            isAbilityActive = false;
            isShaking = false;
           
        }
    }

    public void ListenToOnViewChange(Component sender,object data)
    {
        if((bool)data && sender is IShakeable)
        {
            shakeable = (IShakeable)sender;
            shakebleGameobject = shakeable.IntiateShake(this.gameObject);
            neededShakeType = shakeable.e_ShakeType;
            
            if(shakeable.e_NeedToperformTask_BeforeShake == E_NeedToperformTask_BeforeShake.MovetowardsTarget)MoveTowardsTarget();
            else ReachedtoTarget();
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
       shakeable.ReachedtoTarget();
    }
        
    



    private void ManagerShakeTree()
    {
       switch(neededShakeType)
       {
            case E_ShakeType.Any:
                ShakeAny();
                break;
            case E_ShakeType.Horizontal:
                ShakeHorizontal();
                break;
            case E_ShakeType.Vertical:
                ShakeVerticel();
                break;
       }
    }
    void ShakeAny()
    {
        if(!Input.GetMouseButton(0))
        {
            shakeable.StopShaking();
            return;
        }
           

        float shakeMagnitude = Vector2.Distance(Input.mousePosition , previousMousePosition);

        if(ShouldShake(shakeMagnitude))
        {
            Shake(shakeMagnitude);
        }else{
            shakeable.StopShaking();
        }


        previousMousePosition = Input.mousePosition;
    }

    void ShakeHorizontal()
    {
        if(!Input.GetMouseButton(0))
        {
            shakeable.StopShaking();
            return;
        }
           

        float shakeMagnitude = MathF.Abs(Input.mousePosition.x - previousMousePosition.x);

        if(ShouldShake(shakeMagnitude))
        {
            Shake(shakeMagnitude);
        }else{
            shakeable.StopShaking();
        }


        previousMousePosition = Input.mousePosition;
    }
    
    void ShakeVerticel()
    {
        if(!Input.GetMouseButton(0))
        {
            shakeable.StopShaking();
            return;
        }
           

        float shakeMagnitude = MathF.Abs(Input.mousePosition.y - previousMousePosition.y);

        if(ShouldShake(shakeMagnitude))
        {
            Shake(shakeMagnitude);
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

    private void Shake(float shakeMagnitude)
    {
        isShaking = true;
        //playerAnimator.PlayerShakeTreeAnimation(true);
        shakeable.Shake(shakeMagnitude);
        // reset is shaking bool after .5s
        LeanTween.delayedCall(.1f,()=> isShaking = false);
    }
}
