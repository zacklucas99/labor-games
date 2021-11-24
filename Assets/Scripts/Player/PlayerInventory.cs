using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public long maxStack = 10;
    private long coinStack = 0;
    private long potStack = 0;

    public GameObject uiCoinPrefab;
    public GameObject uiPotPrefab;

    public Transform uiPanel;

    private List<GameObject> invList = new List<GameObject>();

    private Vector3 coinPos = new Vector3(5.701775f, -11.02942f, 7.524786f);
    private Vector3 potPos = new Vector3(5.57f, -11.78242f, 7.556757f);
    private float posOffset = 1.5f;

    private int activeSlot = 0;

    private GameObject activeObj;
    private GameObject activeObjLeft;
    private GameObject activeObjRight;

    public Text textLeft;
    public Text textMiddle;
    public Text textRight;

    public float invCooldown = 3;
    public bool hiddenInv = true;

    private void Start()
    {
        if (hiddenInv)
        {
            HideInv();
        }
        
    }
    

    void Update()
    {

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (hiddenInv)
            {
                uiPanel.gameObject.SetActive(true);
                CancelInvoke();
                Invoke(nameof(HideInv), invCooldown);
            }

            if (activeSlot < invList.Count - 1)
            {
                activeSlot++;
                UpdateInv();
                UpdateAmounts();
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (hiddenInv)
            {
                uiPanel.gameObject.SetActive(true);
                CancelInvoke();
                Invoke(nameof(HideInv), invCooldown);
            }

            if (activeSlot > 0)
            {
                activeSlot--;
                UpdateInv();
                UpdateAmounts();
            }
        }
    }

    public bool AddObject(GameObject obj)
    {
        if(obj.tag == "Coin")
        {
            if(coinStack == 0)
            {
                invList.Add(uiCoinPrefab);
                UpdateInv();
            }
            if (coinStack < maxStack)
            {
                coinStack++;
                UpdateAmounts();
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
            if (potStack == 0)
            {
                invList.Add(uiPotPrefab);
                UpdateInv();
            }
            if (potStack < maxStack)
            {
                potStack++;
                UpdateAmounts();
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
            if (coinStack == 1)
            {
                if (activeSlot > 0)
                {
                    activeSlot--;
                }
                invList.Remove(uiCoinPrefab);
                UpdateInv();
            }
            if (coinStack > 0)
            {
                coinStack--;
                UpdateAmounts();
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
            if (potStack == 1)
            {
                if (activeSlot > 0)
                {
                    activeSlot--;
                }
                invList.Remove(uiPotPrefab);
                UpdateInv();
            }
            if (potStack > 0)
            {
                potStack--;
                UpdateAmounts();
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

    private Vector3 GetInitPos(GameObject obj)
    {
        if(obj == uiPotPrefab)
        {
            return potPos;
        } 
        else
        {
            return coinPos;
        }
    }

    private void UpdateInv()
    {
        if (activeObj != null){
            Destroy(activeObj);
        }
        if (activeObjLeft != null)
        {
            Destroy(activeObjLeft);
        }
        if (activeObjRight != null)
        {
            Destroy(activeObjRight);
        }
        if (invList.Count > 0)
        {
            activeObj = Instantiate(invList[activeSlot], GetInitPos(invList[activeSlot]), Quaternion.identity, uiPanel);
            
            if (activeSlot > 0)
            {
                activeObjLeft = Instantiate(invList[activeSlot - 1], GetInitPos(invList[activeSlot - 1]) - new Vector3(posOffset, 0f, 0f), Quaternion.identity, uiPanel);
                
            }
            if (activeSlot < invList.Count - 1)
            {
                activeObjRight = Instantiate(invList[activeSlot + 1], GetInitPos(invList[activeSlot + 1]) + new Vector3(posOffset, 0f, 0f), Quaternion.identity, uiPanel);
                
            }
        }
        
    }

    public string GetActiveObjTag()
    {
        if(activeObj != null)
        {
            return activeObj.tag;
        }
        return "";
    }

    private void SetAmountText(GameObject obj, long amount)
    {
        if(amount > 0)
        {
            if (activeSlot == invList.IndexOf(obj))
            {
                textMiddle.text = amount + "";
            }
            else if (activeSlot + 1 == invList.IndexOf(obj))
            {
                textRight.text = amount + "";
            }
            else if (activeSlot - 1 == invList.IndexOf(obj))
            {
                textLeft.text = amount + "";
            }
        }
        
    }

    private void UpdateAmounts()
    {
        textMiddle.text = "";
        textLeft.text = "";
        textRight.text = "";
        SetAmountText(uiCoinPrefab, coinStack);
        SetAmountText(uiPotPrefab, potStack);
    }

    public bool ItemsActive()
    {
        if(activeObj != null)
        {
            return true;
        }
        return false;
    }

    private void HideInv()
    {
        uiPanel.gameObject.SetActive(false);
        
    }

}
