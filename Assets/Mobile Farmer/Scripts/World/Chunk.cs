using System;

using System.Collections.Generic;
using jy_util;
using TMPro;

using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject unlocedElements;
    [SerializeField] GameObject lockedElements;
    [SerializeField] TextMeshPro priceText;
    [SerializeField] GameEvent SaveWorldDataEvent,chunkUnlockedEvent;
    [SerializeField] ChunkWalls chunkWalls;
    [Tooltip("Put some transfomr ref where animals can move around")]
    [SerializeField] List<Transform> animalPetrolPoints = new List<Transform>();
    public CropFieldDataHolder cropFieldDataHolder;

    [Header("Settings")]
    [SerializeField] int initialPrice=25;

    public Action chunkUnlocked;


    private int currentPrice ;

    [SerializeField] FeedBackManager feedBackManager;


    void Start()
    {
        if(!feedBackManager) feedBackManager=GetComponent<FeedBackManager>();
        currentPrice = initialPrice;
        priceText.text = JY_Mono_Utiliy.ConverCoinToString(currentPrice);

    }

    public void Initialize(int price)
    {
       

        if(price <= 0) //chunk is unlocked
        {
            lockedElements.SetActive(false);
            unlocedElements.SetActive(true);
            currentPrice =0;
        }else{
            //lockedElements.SetActive(true);
            unlocedElements.SetActive(false);
            //show left price
            currentPrice = price;
            priceText.text = currentPrice.ToString();
        }
    }

    public void TryUnlcok()
    {
        //check for enough money
        if(!CashManager.Instance.DebitCoin(1)) return;

        //decrease the chunk price 
        currentPrice --;
        priceText.text = currentPrice.ToString();

        if(currentPrice <= 0)
            Unlock();
    }

    public virtual void Unlock()
    {
        lockedElements.SetActive(false);
        unlocedElements.SetActive(true);
        feedBackManager.PlayFeedback();
        //save data
        currentPrice=0;
        SaveWorldDataEvent.Raise(this,true);
        chunkUnlockedEvent.Raise(this,true);

        chunkUnlocked?.Invoke();
        //
        AudioManager.instance.PlaySound("Chunk_Unlocked");
    }
    public int GetInitialPrice()
    {
        return initialPrice;
    }

    public int GetCurrentPrice()
    {
        return currentPrice;
    }

    public bool IsUnclocked()
    {
        return (currentPrice<=0);
    }

    public void UpdateWall(int configuration=0000)
    {
        chunkWalls.Configur(configuration);
    }

    public void DisplayLockedElements()
    {
        lockedElements.SetActive(true);
    }
    public bool canAnimalCome()
    {
        return animalPetrolPoints.Count>0 && currentPrice <=0;
    }
    public Vector3 GetAnimalPoint()
    {
        return animalPetrolPoints[UnityEngine.Random.Range(0, animalPetrolPoints.Count)].position;
    }

    private void OnDrawGizmos()
    {

        Gizmos. color = Color. red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 5);
        Gizmos.color =new Color(0, 0, 0, 0);

        Gizmos.DrawCube(transform.position, Vector3.one * 5) ;

    }
}
