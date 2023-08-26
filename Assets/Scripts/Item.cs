using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public string myName = "Item";

    public ItemType myType;
    public int myCount;
    public bool myIsThrowable;
    public GameObject myThrownObjectPrefab;
    public float myThrowForce = 100;




    public Item(string name, ItemType type)
    {
        myName = name;
    }


    public enum ItemType
    {
        Collectable,
        Throwable
    }

    //public bool ThrowItem(Item theItem, Transform myPlayerTransform)
    //{
    //    if (theItem.myCount != 0 && theItem.myIsThrowable)
    //    {

    //        Vector3 throwDirection = myPlayerTransform.forward;
    //        PickupItem thrownObject = Instantiate(myThrownObjectPrefab, myPlayerTransform.position, myPlayerTransform.rotation);
    //        Rigidbody thrownObjectRigidbody = thrownObject.GetComponent<Rigidbody>();
    //        thrownObjectRigidbody.useGravity = true;

    //        //myInventory.removeFromInventory(theItem);
    //        thrownObjectRigidbody.AddForce(throwDirection * 60, ForceMode.Impulse);
    //        thrownObjectRigidbody.AddTorque(throwDirection * myThrowForce, ForceMode.Impulse);
    //        return true;
    //    }
    //    return false;
    //}
}
