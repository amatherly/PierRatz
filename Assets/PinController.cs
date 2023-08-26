using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{
    [SerializeField]
    public PlayerController player;
    public float force = 10;
    public float radius = 5;
    public float upForce = 5;
    public Vector3 COfMass;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // rb.centerOfMass = COfMass;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.AddRelativeForce(player.transform.localPosition * force);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("EXPLOSIONFORCE");
            rb.AddForce(force * other.gameObject.GetComponent<Rigidbody>().angularVelocity);
        }
    }
}
