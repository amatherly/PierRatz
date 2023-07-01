using UnityEngine;

public class Sliding : MonoBehaviour
{
    private readonly bool mySkating;
    private readonly float mySlideTimer;

    public float myHorizontalInput;

    [Header("Sliding")] public float myMaxSlideTime;

    public PlayerMovement myMovement;

    [Header("References")] public Transform myOrientation;

    public Transform myPlayer;
    public Rigidbody myRb;

    public float mySlideForce;

    [Header("Input")] public KeyCode mySlideKey = KeyCode.LeftControl;

    public float mySlideYscale;

    private bool mySliding;
    private float myStartYscale;
    public float myVerticalInput;

    // Start is called before the first frame update
    private void start()
    {
        myStartYscale = myPlayer.localScale.y;
    }

    private void startSlide()
    {
        mySliding = true;
    }

    private void slidingMovement()
    {
    }

    private void stopSlide()
    {
    }

    // Update is called once per frame
    private void update()
    {
        myHorizontalInput = Input.GetAxisRaw("Horizontal");
        myVerticalInput = Input.GetAxisRaw("Vertical");
    }
}