using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public long maxCoinStack = 10;
    private long coinStack = 0;

    public bool AddCoin()
    {
        if(coinStack < maxCoinStack)
        {
            coinStack++;
            Debug.Log("Coins: " + coinStack);
            return true;
        }
        else
        {
            Debug.Log("Coins: " + coinStack);
            return false;
        }
    }

    public bool RemoveCoin()
    {
        if (coinStack > 0)
        {
            coinStack--;
            Debug.Log("Coins: " + coinStack);
            return true;
        }
        else
        {
            Debug.Log("Coins: " + coinStack);
            return false;
        }
    }
}
