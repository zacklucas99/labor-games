using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public long maxStack = 10;
    private long coinStack = 0;
    private long potStack = 0;

    public bool AddObject(GameObject obj)
    {
        if(obj.tag == "Coin")
        {
            if (coinStack < maxStack)
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
        else if (obj.tag == "Pot")
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
        else
        {
            Debug.Log("No matching tag");
            return false;
        }
        
    }

    public bool RemoveObject(GameObject obj)
    {
        if (obj.tag == "Coin")
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
        else if (obj.tag == "Pot")
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
        else
        {
            Debug.Log("No matching tag");
            return false;
        }
    }
}
