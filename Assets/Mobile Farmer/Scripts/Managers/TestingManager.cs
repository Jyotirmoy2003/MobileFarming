using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingManager : MonoBehaviour
{

    [SerializeField] WorldManager worldManager;

    public void UnlockAllChunk()
    {
        worldManager.UnlockAllChunks();
    }

    public void AddCoin()
    {
        CashManager.Instance.CreditCoins(10000);
    }

    public void RemoveCoin()
    {
        CashManager.Instance.ClearCoin();
    }

    public void AddGem()
    {
        CashManager.Instance.CreditGems(100);
    }

    public void RemoveGem()
    {
        CashManager.Instance.ClearGems();
    }


}
