using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private Hashtable soundTable = new Hashtable();

    public SoundManager Instance = null;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void Start()
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


    void Update()
    {
    }
}