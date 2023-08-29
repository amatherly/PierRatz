using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private int hoverSoundID = 0; 
    private SoundManager soundManager; 

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        Button[] buttons = FindObjectsOfType<Button>();
        
        foreach (Button button in buttons)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => { OnButtonHover(); });
            
            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
            trigger.triggers.Add(entry);
        }
    }

    void OnButtonHover()
    {
        if (soundManager != null)
        {
            soundManager.PlaySound(hoverSoundID);
        }
        else
        {
            Debug.LogError("SoundManager not found in the scene.");
        }
    }
}