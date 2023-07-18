using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Throwable"))
        {
            Debug.Log("Hit the Target!!!");
            myAnimator.SetTrigger("Hit");
            Destroy(other.gameObject);
        }
    }
}
