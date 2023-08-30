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
    
    private float force = 10f;
    private float radius = 3f;
    private float upForce = 5f;
    private Rigidbody rb;
    private float mass = 1;
    private float[] difficultyLevel = { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 };

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = COfMass;
        audioClip = audioSource.clip;
        rb.mass = mass;
    }

    // Update is called once per frame
    void Update()
    {
    }
    

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("hitPin");
            Vector3 explosionPos = other.contacts[0].point;
            // rb.AddExplosionForce(force * player.CurrentSkateSpeed, player.transform.forward, radius, upForce, ForceMode.Force);
            rb.AddRelativeForce(transform.forward * force, ForceMode.Impulse);
        }
        audioSource.PlayOneShot(audioClip);
    }

    public void ChangePinMass(int level)
    {
        if (level >= 0 && level < difficultyLevel.Length)
        {
            mass = difficultyLevel[level];
            rb.mass = mass; 
        }
    }

    public void ChangePinCenterOfMass(int level)
    {
        
    }
}