using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public string myName = "Item";
    public string myDescription;
    public ItemType myType;
    public int myCount;
    public bool myIsThrowable;
    public GameObject myThrownObjectPrefab;
    public Transform myPlayerTransform;
    public float myThrowForce = 100;


    public Item(string name, string description)
    {
        myName = name;
        myDescription = description;
    }


    public enum ItemType
    {
        Collectable,
        Sellable
    }

    public bool ThrowItem(Item theItem)
    {
        if (theItem.myCount != 0 && theItem.myIsThrowable)
        {

            Vector3 throwDirection = myPlayerTransform.forward;
            GameObject thrownObject = Instantiate(myThrownObjectPrefab, myPlayerTransform.position, myPlayerTransform.rotation);
            Rigidbody thrownObjectRigidbody = thrownObject.GetComponent<Rigidbody>();
            thrownObjectRigidbody.useGravity = true;

            //myInventory.removeFromInventory(theItem);
            thrownObjectRigidbody.AddForce(throwDirection * 60, ForceMode.Impulse);
            thrownObjectRigidbody.AddTorque(throwDirection * myThrowForce, ForceMode.Impulse);
            return true;
        }
        return false;
    }
}
