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
        // Set the starting position of the item to the position of the Pickup object
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire1 pressed");
            myItem.ThrowItem(myInventory.myCurrentThrowable);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("The player is in range of " + name);

        if (other.gameObject.CompareTag("Player"))
        {
            myInventory.addToInventory(myItem);
            Destroy(gameObject);
        }
    }



}