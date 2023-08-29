using UnityEngine;
using UnityEngine.Events;

public class TalkInteraction : MonoBehaviour
{
    public bool isInRange = false;
    public KeyCode keyCode;
    public UnityEvent action;
    public GameObject canvas;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(keyCode))
            {
                action.Invoke();
                canvas.SetActive(true);
                
            }

        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("In range of conversation, press E to talk.");
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            canvas.SetActive(false);
            Debug.Log("Out of range of interaction.");
        }
    }
}
