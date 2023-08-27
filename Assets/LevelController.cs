using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Cinemachine;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private BoxCollider endPos;
    
    [SerializeField]
    private BoxCollider startPos;

    [SerializeField]
    private PinController pinController;

    [SerializeField] private int waitTime = 3;
    
    private int count = 0;

    

    void Start()
    {
        InitializePins();

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PinCollider"))
        {
            GameManager.GAME.Camera.Follow = null;
            GameManager.GAME.Camera.m_Orbits[1].m_Height = 10;
            GameManager.GAME.Player.IsLevelFinished = true;
            GameManager.GAME.Player.CarryOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(WaitAndCheckPins());
    }

    private IEnumerator WaitAndCheckPins()
    {
        count++;
        Debug.Log("called: " + count + " times");
        yield return new WaitForSeconds(waitTime);
        pinController.CheckPins();
    }
    public void Pause()
    {
        
    }
    
    public void Resume()
    {
        
    }

    public void InitializePins()
    {
        
    }
}
