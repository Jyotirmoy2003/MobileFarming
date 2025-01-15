using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] GameObject unlocedElements;
    [SerializeField] GameObject lockedElements;
    [SerializeField] TextMeshPro priceText;
    [SerializeField] GameEvent SaveWorldDataEvent;

    [Header("Settings")]
    [SerializeField] int initialPrice=25;


    private int currentPrice ;

    [SerializeField] FeedBackManager feedBackManager;


    void Start()
    {
        if(!feedBackManager) feedBackManager=GetComponent<FeedBackManager>();
        currentPrice = initialPrice;
        priceText.text = currentPrice.ToString();    
    }

    public void Initialize(int price)
    {
       

        if(price <= 0) //chunk is unlocked
        {
            lockedElements.SetActive(false);
            unlocedElements.SetActive(true);
            currentPrice =0;
        }else{
            lockedElements.SetActive(true);
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

    private void Unlock()
    {
        lockedElements.SetActive(false);
        unlocedElements.SetActive(true);
        feedBackManager.PlayFeedback();
        //save data
        currentPrice=0;
        SaveWorldDataEvent.Raise(this,true);
    }
    public int GetInitialPrice()
    {
        return initialPrice;
    }

    public int GetCurrentPrice()
    {
        return currentPrice;
    }
}
