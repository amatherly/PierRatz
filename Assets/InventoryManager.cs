using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public const int MAXSIZE = 24;
    public int myCurrentSize;
    public GameObject myInventoryItemPrefab;
    public List<string> myInventory;
    public TMP_Text myCountText;
    public Transform myPlayerTransform;
    public Item myCurrentThrowable;

    // Start is called before the first frame update
    private void Start()
    {
        myInventory = new
            List<string>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void addToInventory(Item theItem)
    {
        if (myCurrentSize < MAXSIZE)
        {
            if (!myInventory.Contains(theItem.name))
            {
                myInventory.Add(theItem.name);
                myCurrentSize++;
            }
            theItem.myCount++;
        }
        if (theItem.myIsThrowable && theItem == myCurrentThrowable)
        {
            myCountText.SetText(theItem.myCount.ToString());
        }
    }

    public void removeFromInventory(Item theItem)
    {
        if (theItem.myCount > 0)
        {
            myCurrentSize--;
            _ = myInventory.Remove(theItem.name);
            theItem.myCount--;
        }
        if (theItem.myIsThrowable)
        {
            myCountText.SetText(theItem.myCount.ToString());
        }
    }

}