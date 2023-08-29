using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{

    public Queue messages;
    public Text nameText;
    public Text dialogueText;



    // Start is called before the first frame update
    void Start()
    {
        messages = new Queue();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Chatting with " + dialogue.name);

        nameText.text = dialogue.name;

        messages.Clear();

        foreach (string message in dialogue.messages)
        {
            messages.Enqueue(message);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(messages.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = (string)messages.Dequeue();
        Debug.Log(sentence);
        dialogueText.text = sentence;

    }

    public void EndDialogue()
    {
        Debug.Log("End of Conversation");


    }


}
