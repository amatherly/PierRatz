using UnityEngine;
using UnityEngine.Rendering;

public class PickupItem : MonoBehaviour
{
    public Item myItem;
    public float mySpeed = 2f;
    public float myPickupRange = 1f;
    public AudioClip myPickupSound;
    public Rigidbody myRigidbody;
    public GameObject myPlayerController;
    public AudioSource myAudioSource;
    public InventoryManager myInventory;

    // Start is called before the first frame update
    private void Start()
    {
        myItem = new Item(name, Item.ItemType.Throwable);
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //myInventory.addToInventory(myItem);
            Destroy(gameObject);
        }
    }

}