using UnityEngine;

public class SkateboardMovement : MonoBehaviour
{
    private readonly float myGravityValue = -9.81f;
    private readonly bool myGroundedPlayer;
    private readonly float myJumpHeight = 1.0f;
    public Transform myCameraTransform;
    public CharacterController myController;
    private Vector3 myMovementDirection;
    public float myPlayerSpeed = 2.0f;
    private Vector3 myPlayerVelocity;
    public Rigidbody myRb;
    public float myRotationSpeed;

    private void start()
    {
        myController = gameObject.AddComponent<CharacterController>();
    }

    private void update()
    {
        myMovementDirection = new Vector3(Input.GetAxis("Horizontal"), 0);
    }

    private void fixedUpdate()
    {
        moveCharacter(myMovementDirection);
    }

    public void moveCharacter(Vector3 theMovement)
    {
        myRb.AddForce(theMovement * myPlayerSpeed);
    }
}