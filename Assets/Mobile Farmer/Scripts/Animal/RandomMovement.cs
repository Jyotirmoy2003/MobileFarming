using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using jy_util;

public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range;
    public Action OnReachOnDestination, updateDel;
    public LayerMask chunkLayer;
    public bool isValidPosition;
    public Transform centrePoint;
    public bool beginFromStart = false;
    private bool isAssignedNewDest = false;
    private const int maxAttempts = 10; // Prevent infinite loops

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        updateDel += new util().NullFun;
        if (beginFromStart) updateDel += Movement;
    }

    void Update() => updateDel();

    void Movement()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) 
        {
            if (isAssignedNewDest) OnReachOnDestination?.Invoke();

            Vector3 point;
            if (isAssignedNewDest = GetValidRandomPoint(out point)) 
            {
                Debug.DrawRay(point, Vector3.up * 2, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
        }
    }

    bool GetValidRandomPoint(out Vector3 result)
    {
        result = Vector3.zero;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomPoint = centrePoint.position + UnityEngine.Random.insideUnitSphere * range;
            randomPoint.y = centrePoint.position.y; // Keep it on the same level

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
            {
                Vector3 pointPos = hit.position;

                // Raycast downward from the sampled point to detect the chunk
                if (Physics.Raycast(pointPos + Vector3.up * 2, Vector3.down, out RaycastHit rayHit, 5f, chunkLayer))
                {
                    if (rayHit.collider.TryGetComponent<Chunk>(out var chunk) && chunk.IsUnclocked())
                    {
                        if(rayHit.collider.TryGetComponent<CropFieldDataHolder>(out var cropFieldDataHolder))
                        {
                            return false;
                        }
                        result = pointPos;
                        return true; // Found a valid unlocked chunk
                    }
                }
            }
        }

        Debug.LogWarning("Failed to find a valid position after multiple attempts.");
        return false;
    }

    public void StopAndStartMovement(bool stop)
    {
        if (stop)
        {
            updateDel -= Movement;
        }
        else
        {
            Vector3 point;
            if (isAssignedNewDest = GetValidRandomPoint(out point))
            {
                Debug.DrawRay(point, Vector3.up * 2, Color.blue, 1.0f);
                agent.SetDestination(point);
            }
            updateDel += Movement;
        }
    }
}
