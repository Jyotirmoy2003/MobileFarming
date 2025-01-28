using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RandomMovement))]
public class AnimalBase : MonoBehaviour
{
    [Header("Elemesnts")]
    private RandomMovement randomMovement;

    [Header("Settings")]
    [SerializeField] float idelTimeMin=5f;
    [SerializeField] float idelTimeMAX=5f;
    private bool isMoveing=true;


    void Start()
    {
        randomMovement = GetComponent<RandomMovement>();
        randomMovement.OnReachOnDestination += OnReachToOneDest;
    }

    void OnDestroy()
    {
        randomMovement.OnReachOnDestination -= OnReachToOneDest;
    }

   void OnReachToOneDest()
   {
        Debug.Log("reached to dest called");
        if(!isMoveing) return;

        Debug.Log("reached to dest");
        isMoveing = false;
        randomMovement.StopAndStartMovement(true);
        LeanTween.delayedCall(Random.Range(idelTimeMin,idelTimeMAX), ()=>StartMovemnt() );
   }
   void StartMovemnt()
   {
        Debug.Log("Restaring movement");;
        randomMovement.StopAndStartMovement(false);
        isMoveing = true;
   }




}
