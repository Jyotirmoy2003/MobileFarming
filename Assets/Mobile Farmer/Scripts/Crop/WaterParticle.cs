using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(ParticleSystem))]
public class WaterParticle : MonoBehaviour
{
    [SerializeField] PlayerDataHolder ownerData;
   public  Action<Vector3[],PlayerDataHolder> onWaterCollided;
    ParticleSystem ps;
    void Start()
    {
        ps= GetComponent<ParticleSystem>();
    }


    //when particles collide with crop field
    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
        int collisionAmount=ps.GetCollisionEvents(other,collisionEvents);

        Vector3[] collisionPosition=new Vector3[collisionAmount];


        for(int i=0;i<collisionAmount;i++)
            collisionPosition[i]=collisionEvents[i].intersection;
        

        onWaterCollided?.Invoke(collisionPosition,ownerData);
    }
}
