using UnityEngine;

public class InputController : MonoBehaviour
{
    public SimplePlayerController myCharController;

    private void Awake()
    {
    }

    private void FixedUpdate()
    {
        // Get input values
        var Vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        var Horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        var Jump = Input.GetKey(KeyCode.Space);
        myCharController.ForwardInput = Vertical;
        myCharController.TurnInput = Horizontal;
        myCharController.JumpInput = Jump;
    }
}