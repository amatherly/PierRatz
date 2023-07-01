using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private readonly float mySkateSpeed;
    private readonly float myWalkSpeed;
    private Animator myAnimator;

    [SerializeField] private Transform myCameraTransform;

    public Transform myCapsuleTransform;
    private CharacterController myCharacterController;
    private bool myIsSkating;

    [SerializeField] private float myJumpButtonGracePeriod;

    private float? myJumpButtonPressedTime;

    [SerializeField] private float myJumpSpeed;

    private float? myLastGroundedTime;

    [SerializeField] private float myMaximumSpeed;

    private float myOriginalStepOffset;

    [SerializeField] private float myRotationSpeed;

    public GameObject mySkateboard;

    private float mySpeedMultiplier = 0.5f;
    private float myYSpeed;

    // Start is called before the first frame update
    private void start()
    {
        myAnimator = GetComponent<Animator>();
        myCharacterController = GetComponent<CharacterController>();
        myOriginalStepOffset = myCharacterController.stepOffset;
    }

    // Update is called once per frame
    private void update()
    {
        var HorizontalInput = Input.GetAxis("Horizontal");
        var VerticalInput = Input.GetAxis("Vertical");

        if (VerticalInput != 0) myAnimator.SetBool("IsMoving", true);

        Vector3 MovementDirection = new(HorizontalInput, 0, VerticalInput);
        var InputMagnitude = Mathf.Clamp01(MovementDirection.magnitude);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) InputMagnitude /= 2;

        if (Input.GetKey(KeyCode.Tab) || Input.GetButton("Fire1"))
        {
            myIsSkating = !myIsSkating;
            Debug.Log("Skating: " + myIsSkating);
        }

        mySkateboard.SetActive(myIsSkating);
        myAnimator.SetBool("isSkating", myIsSkating);

        myAnimator.SetFloat("InputMagnitude", InputMagnitude, 0.05f, Time.deltaTime);
        var Speed = InputMagnitude * myMaximumSpeed * mySpeedMultiplier;

        // Raycast downward to detect the plane
        if (Physics.Raycast(transform.position, -transform.up, out var Hit))
        {
            Debug.Log("raycast success");

            // Calculate the rotation needed to align with the plane normal
            var TargetRotation = Quaternion.FromToRotation(myCapsuleTransform.up, Hit.normal) *
                                 myCapsuleTransform.rotation;
            // Debug.Log("Angle: " + hit.normal);
            //Debug.Log("Rotation: " + targetRotation);

            // Smoothly rotate the capsule towards the target rotation
            myCapsuleTransform.rotation = TargetRotation;
        }

        mySpeedMultiplier = myIsSkating ? 2f : 1.5f;

        MovementDirection = Quaternion.AngleAxis(myCameraTransform.rotation.eulerAngles.y, Vector3.up) *
                            MovementDirection;
        MovementDirection.Normalize();

        myYSpeed += Physics.gravity.y * Time.deltaTime;

        if (myCharacterController.isGrounded) myLastGroundedTime = Time.time;

        if (Input.GetButtonDown("Jump") || Input.GetKey(KeyCode.Space))
        {
            myJumpButtonPressedTime = Time.time;
            myAnimator.SetTrigger("Ollie");
        }

        if (Time.time - myLastGroundedTime <= myJumpButtonGracePeriod)
        {
            myCharacterController.stepOffset = myOriginalStepOffset;
            myYSpeed = -0.9f;

            if (Time.time - myJumpButtonPressedTime <= myJumpButtonGracePeriod)
            {
                myYSpeed = myJumpSpeed;
                myJumpButtonPressedTime = null;
                myLastGroundedTime = null;
            }
        }
        else
        {
            myCharacterController.stepOffset = 0;
        }

        var Velocity = MovementDirection * Speed;
        Velocity.y = myYSpeed;

        _ = myCharacterController.Move(Velocity * Time.deltaTime);

        if (MovementDirection != Vector3.zero)
        {
            var ToRotation = Quaternion.LookRotation(MovementDirection, Vector3.up);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, ToRotation, myRotationSpeed * Time.deltaTime);
        }
    }

    private void OnApplicationFocus(bool theFocus)
    {
        Cursor.lockState = theFocus ? CursorLockMode.Locked : CursorLockMode.None;
    }
}