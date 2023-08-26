using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private BoxCollider endPos;
    
    [SerializeField]
    private BoxCollider startPos;
    
    [SerializeField]
    private CinemachineFreeLook camera;

    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camera.Follow = null;
            camera.m_Orbits[1].m_Height = 10;
            player.IsLevelFinished = true;
            player.CarryOn();
        }
    }
}
