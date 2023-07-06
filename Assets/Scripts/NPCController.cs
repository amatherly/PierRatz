using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    private Animator myAnimator;
    private Animation myIdleAnimation;
    private NPC myNPC;

    // Start is called before the first frame update
    void Start()
    {
        myNPC = new(name, 100);
        myAnimator = GetComponent<Animator>();
        myIdleAnimation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!myNPC.IsAlive())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with NPC");
        {
            if (other.gameObject.CompareTag("PickupItem"))
            {
                Debug.Log("Collided with PickupItem");
                myAnimator.SetTrigger("IsHit");
                myNPC.Hit();
            }
        }
    }
}
