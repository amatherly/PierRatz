using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [Tooltip("Whether the character can jump")]
    public bool myAllowJump = false;

    public CapsuleCollider myCapsuleCollider;

    [Tooltip("Upward speed to apply when jumping in meters/second")]
    public float myJumpSpeed = 4f;

    [Tooltip("Move speed in meters/second")]
    public float myMoveSpeed = 5f;

    public Rigidbody myRigidbody;

    [Tooltip("Maximum slope the character can jump on")] [Range(5f, 60f)]
    public float mySlopeLimit = 45f;

    [Tooltip("Turn speed in degrees/second, left (+) or right (-)")]
    public float myTurnSpeed = 300;

    public bool IsGrounded { get; private set; }

    public float ForwardInput { get; set; }

    public float TurnInput { get; set; }

    public bool JumpInput { get; set; }

    private void awake()
    {
    }

    // Start is called before the first frame update

    private void fixedUpdate()
    {
        checkGrounded();
        processActions();
    }

    private void start()
    {
    }

    /// <summary>
    ///     Checks whether the character is on the ground and updates <see cref="IsGrounded" />
    /// </summary>
    private void checkGrounded()
    {
        IsGrounded = false;
        var CapsuleHeight = Mathf.Max(myCapsuleCollider.radius * 2f, myCapsuleCollider.height);
        var CapsuleBottom = transform.TransformPoint(myCapsuleCollider.center - Vector3.up * CapsuleHeight / 2f);
        var Radius = transform.TransformVector(myCapsuleCollider.radius, 0f, 0f).magnitude;
        Ray Ray = new(CapsuleBottom + transform.up * .01f, -transform.up);
        if (Physics.Raycast(Ray, out var Hit, Radius * 5f))
        {
            var NormalAngle = Vector3.Angle(Hit.normal, transform.up);
            if (NormalAngle < mySlopeLimit)
            {
                var MaxDist = Radius / Mathf.Cos(Mathf.Deg2Rad * NormalAngle) - Radius + .02f;
                if (Hit.distance < MaxDist) IsGrounded = true;
            }
        }
    }

    private void processActions()
    {
        // Process Turning
        if (TurnInput != 0f)
        {
            var Angle = Mathf.Clamp(TurnInput, -1f, 1f) * myTurnSpeed;
            transform.Rotate(Vector3.up, Time.fixedDeltaTime * Angle);
        }

        // Process Movement/Jumping
        if (IsGrounded)
        {
            // Reset the velocity
            myRigidbody.velocity = Vector3.zero;
            // Check if trying to jump
            if (JumpInput && myAllowJump)
                // Apply an upward velocity to jump
                myRigidbody.velocity += Vector3.up * myJumpSpeed;

            // Apply a forward or backward velocity based on player input
            myRigidbody.velocity += transform.forward * Mathf.Clamp(ForwardInput, -1f, 1f) * myMoveSpeed;
        }
        else
        {
            // Check if player is trying to change forward/backward movement while jumping/falling
            if (!Mathf.Approximately(ForwardInput, 0f))
            {
                // Override just the forward velocity with player input at half speed
                var VerticalVelocity = Vector3.Project(myRigidbody.velocity, Vector3.up);
                myRigidbody.velocity = VerticalVelocity +
                                       transform.forward * Mathf.Clamp(ForwardInput, -1f, 1f) * myMoveSpeed / 2f;
            }
        }
    }

    // Update is called once per frame
    private void update()
    {
    }
}