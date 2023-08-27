using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Vector3 COfMass;
    
    [SerializeField] private AudioSource audioSource;
    private AudioClip audioClip;
    
    private float force = 5;
    private float radius = 2;
    private float upForce = 0f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = COfMass;
        audioClip = audioSource.clip;
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
            Vector3 explosionPos = other.contacts[0].point;
            rb.AddExplosionForce(force, explosionPos, radius, upForce, ForceMode.Impulse);
            rb.AddRelativeForce(transform.forward * force, ForceMode.Impulse);
            
        }
        audioSource.PlayOneShot(audioClip);
    }
}