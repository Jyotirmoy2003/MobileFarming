using System;
using System.Collections;
using System.Collections.Generic;
using jy_util;
using UnityEngine;
using UnityEngine.AI; //important

//if you use this code you are contractually obligated to like the YT video
public class RandomMovement : MonoBehaviour //don't forget to change the script name if you haven't
{
    public NavMeshAgent agent;
    public float range; //radius of sphere
    public Action OnReachOnDestination,updateDel;

    public Transform centrePoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area

    public bool beginFromStart = false;
    private bool isAssigendNewDest = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        updateDel += new util().NullFun;
        if(beginFromStart) updateDel += Movement;
    }

    
    void Update()=>updateDel();

    void Movement()
    {
        if(agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            if(isAssigendNewDest)OnReachOnDestination?.Invoke();

            Vector3 point;
            if (isAssigendNewDest = RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }
        }

    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
    public void StopAndStartMovement(bool stop)
    {
        if(stop)
        {
            updateDel -= Movement;
        }else{
            Vector3 point;
            if (isAssigendNewDest = RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }
            updateDel += Movement;
        }
    }

    
}
