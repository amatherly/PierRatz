using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class NPCTalkingInteraction : MonoBehaviour
{

    public Queue messages;
    public DialogueController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller.messages = messages;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Talk()
    { 
        Debug.Log(messages.Dequeue());
    }
}
