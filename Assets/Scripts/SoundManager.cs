using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private Hashtable soundTable = new Hashtable();

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < sounds.Length; i++)
        {
            soundTable.Add(sounds[i].name, sounds[i]);
        }
    }

    public void PlaySound(int soundID)
    {
        audioSource.PlayOneShot(sounds[soundID]);
    }


    void Start()
    {
    }

    void Update()
    {
    }
}