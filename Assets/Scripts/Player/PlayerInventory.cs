using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public long maxStack = 10;
    private long coinStack = 0;
    private long potStack = 0;
    private long boneStack = 0;

    public GameObject uiCoinPrefab;
    public GameObject uiPotPrefab;
    public GameObject uiBonePrefab;

    public Transform uiPanel;

    private List<GameObject> invList = new List<GameObject>();

    private Vector3 coinPos = new Vector3(0f, 12f, -27f);
    private Vector3 potPos = new Vector3(0f, -6f, -27f);
    private Vector3 bonePos = new Vector3(0f, 10f, 0f);
    private float posOffset = 90f;

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
        else if(obj.tag == "Bone")
        {
            if (boneStack == 0)
            {
                invList.Add(uiBonePrefab);
                UpdateInv();
            }
            if (boneStack < maxStack)
            {
                boneStack++;
                UpdateAmounts();
                Debug.Log("Bones: " + boneStack);
                return true;
            }
            else
            {
                Debug.Log("Bones: " + boneStack);
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

        else if (obj.tag == "Bone")
        {
            if (boneStack == 1)
            {
                if (activeSlot > 0)
                {
                    activeSlot--;
                }
                invList.Remove(uiBonePrefab);
                UpdateInv();
            }
            if (boneStack > 0)
            {
                boneStack--;
                UpdateAmounts();
                Debug.Log("Bones: " + boneStack);
                return true;
            }
            else
            {
                Debug.Log("Pots: " + boneStack);
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
        else if(obj == uiBonePrefab)
        {
            return bonePos;
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
            activeObj = Instantiate(invList[activeSlot], uiPanel.transform.position, Quaternion.identity, uiPanel);
            activeObj.transform.localPosition = GetInitPos(invList[activeSlot]);


            if (activeSlot > 0)
            {
                activeObjLeft = Instantiate(invList[activeSlot - 1], uiPanel.transform.position, Quaternion.identity, uiPanel);
                activeObjLeft.transform.localPosition = GetInitPos(invList[activeSlot - 1]) - new Vector3(posOffset, 0f, 0f);


            }
            if (activeSlot < invList.Count - 1)
            {
                activeObjRight = Instantiate(invList[activeSlot + 1], uiPanel.transform.position, Quaternion.identity, uiPanel);
                activeObjRight.transform.localPosition = GetInitPos(invList[activeSlot + 1]) + new Vector3(posOffset, 0f, 0f);


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
        SetAmountText(uiBonePrefab, boneStack);
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
