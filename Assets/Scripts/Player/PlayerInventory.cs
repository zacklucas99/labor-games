using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public long maxStack = 10;
    private long coinStack = 0;
    private long potStack = 0;

    public bool AddCoin()
    {
        if(coinStack < maxStack)
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

    public bool AddPot()
    {
        if (potStack < maxStack)
        {
            potStack++;
            Debug.Log("Pots: " + potStack);
            return true;
        }
        else
        {
            Debug.Log("Pots: " + potStack);
            return false;
        }
    }

    public bool RemovePot()
    {
        if (potStack > 0)
        {
            potStack--;
            Debug.Log("Pots: " + potStack);
            return true;
        }
        else
        {
            Debug.Log("Pots: " + potStack);
            return false;
        }
    }
}
