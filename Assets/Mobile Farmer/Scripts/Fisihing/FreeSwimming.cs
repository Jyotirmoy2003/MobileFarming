using System;
using System.Linq;
using jy_util;
using UnityEngine;
using Random = UnityEngine.Random;


public class FreeSwimming : MonoBehaviour
{
    [SerializeField] SeaControl seaControl;
    public float maxSpeed = 5f;             
    public float minSpeed = 1f;             
    public float rotationSpeed = 2f;        
    public float avoidanceDistance = 5f;    
    public LayerMask obstacleLayer;         
    public float targetProximityThreshold = 1f; 
    public Action onFishReachedToDest;

    private Vector3 _targetPosition;
    private Collider[] _obstacles;



    private NoArgumentFun updateDel;

   

    protected void Update()=>updateDel?.Invoke();
    
    
       
    

    public void SetRandomTargetPosition()
    {
        var angle = Random.Range(0f, 2f * Mathf.PI);
        var radius = Random.Range(0f, seaControl.radius);
        var x = radius * Mathf.Cos(angle);
        var z = radius * Mathf.Sin(angle);
        var y = Random.Range(-seaControl.height / 2, seaControl.height / 2);

        _targetPosition = seaControl.transform.position + new Vector3(x, y, z);
    }

    private void MoveTowardsTarget()
    {
        
        _obstacles = Physics.OverlapSphere(transform.position, avoidanceDistance, obstacleLayer);

        if (_obstacles.Length > 0)
        {
            var avoidanceDirection = _obstacles.Aggregate(Vector3.zero, (current, obstacle) => current + (transform.position - obstacle.transform.position).normalized);

            var avoidanceRotation = Quaternion.LookRotation(avoidanceDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, avoidanceRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            var direction = _targetPosition - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            var distanceToTarget = direction.magnitude;
            var speedMultiplier = Mathf.Clamp01(distanceToTarget / avoidanceDistance); 
            var speed = Mathf.Lerp(minSpeed, maxSpeed, speedMultiplier);
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        }

        if (Vector3.Distance(transform.position, _targetPosition) <= targetProximityThreshold)
        {
            onFishReachedToDest?.Invoke();
        }
    }






    public void RandomMovementActivation(bool isActive)
    {
        if(isActive)
        {
            onFishReachedToDest += SetRandomTargetPosition;
            SetRandomTargetPosition();

        }else{
            onFishReachedToDest -= SetRandomTargetPosition;
        }
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void SetMovementActivation(bool isActive)
    {
        //if(isActive); updateDel = MoveTowardsTarget;
        updateDel = (isActive)? MoveTowardsTarget:util.NullFun;
    }


 
}
