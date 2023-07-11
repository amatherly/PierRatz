using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Fields
    [SerializeField]
    private float mySkateSpeed = 18;
    private float myWalkSpeed = 7;
    public Animator myAnimator;
    public Transform myCameraTransform;
    public Transform myCapsuleTransform;
    public CharacterController myCharacterController;
    private bool myIsSkating = false;
    private float InputMagnitude;


    [SerializeField]
    private readonly float myJumpButtonGracePeriod;
    private float? myJumpButtonPressedTime;
    private readonly float myJumpSpeed;
    private float? myLastGroundedTime;


    [SerializeField]
    private float myMaximumSpeed = 10;
    public float myOriginalStepOffset;
    private float myRotationSpeed = 1000;
    [SerializeField]
    public GameObject mySkateboard;
    private float mySpeedMultiplier = 0.5f;
    private float myYSpeed;
    public float myThrowForce = 1000f;
    private float mySlope = 1f;
    public bool myIsThrowing = false;
    public float myPushCounter = 0;
    public Item myCurrentThrowable;
    [SerializeField]
    public InventoryManager myInventoryManager;
    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCharacterController = GetComponent<CharacterController>();
        myOriginalStepOffset = myCharacterController.stepOffset;
        myCurrentThrowable = myInventoryManager.myCurrentThrowable;
    }

    // Update is called once per frame
    private void Update()
    {
        mySpeedMultiplier = myIsSkating ? 2f : 1.5f;
        myYSpeed += Physics.gravity.y * Time.deltaTime * 10;

        if (myCharacterController.isGrounded)
            myLastGroundedTime = Time.time;

        CheckForInput();
        MoveCharacter();
    }


    private void AnimateCharacter()
    {
        mySkateboard.SetActive(myIsSkating);
        myAnimator.SetBool("isSkating", myIsSkating);

        float currentSpeed = myCharacterController.velocity.magnitude;

        if (myPushCounter < mySkateSpeed && myIsSkating)
        {
            myPushCounter++;
            myAnimator.SetFloat("SkateSpeed", myPushCounter);
            myAnimator.SetBool("IsPushing", true);
        }
        else
        {
            myAnimator.SetBool("IsPushing", false);
            myPushCounter = 0;
        }
    }

    private void MoveCharacter()
    {
        var Speed = InputMagnitude * myMaximumSpeed * mySpeedMultiplier;
        var HorizontalInput = Input.GetAxis("Horizontal");
        var VerticalInput = Input.GetAxis("Vertical");

        Vector3 cameraForward = myCameraTransform.forward;
        Vector3 cameraRight = myCameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 MovementDirection = (cameraForward * VerticalInput) + (cameraRight * HorizontalInput);
        MovementDirection.Normalize();
        InputMagnitude = Mathf.Clamp01(MovementDirection.magnitude);
        myAnimator.SetFloat("InputMagnitude", InputMagnitude, 0.05f, Time.deltaTime);

        AnimateCharacter();
        RotateToPlaneNormal();

        var Velocity = MovementDirection * Speed;
        Velocity.y = myYSpeed;

        _ = myCharacterController.Move(Velocity * Time.deltaTime);

        if (VerticalInput != 0)
            myAnimator.SetBool("IsMoving", true);

        if (MovementDirection != Vector3.zero)
        {
            var ToRotation = Quaternion.LookRotation(MovementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, ToRotation, myRotationSpeed * Time.deltaTime);
        }
    }


    private void Jump()
    {
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
    }


    private void RotateToPlaneNormal()
    {
        // Raycast downward to detect the plane
        if (Physics.Raycast(transform.position, -transform.up, out var Hit))
        {

            // Calculate the rotation needed to align with the plane normal
            var TargetRotation = Quaternion.FromToRotation(myCapsuleTransform.up, Hit.normal) *
                                 myCapsuleTransform.rotation;

            // Smoothly rotate the capsule towards the target rotation
            myCapsuleTransform.rotation = TargetRotation;
        }
    }



    private void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            InputMagnitude /= 2;

        else if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Fire1"))
        {
            myIsSkating = !myIsSkating;
            myAnimator.SetBool("isSkating", myIsSkating);
        }

        else if (Input.GetButtonDown("Fire2"))
        {
            ThrowItem(myCurrentThrowable);
            myAnimator.SetTrigger("Throw");
        }

        else if (Input.GetButtonDown("Jump") || Input.GetKey(KeyCode.Space))
        {
            myJumpButtonPressedTime = Time.time;
            myAnimator.SetTrigger("Ollie");
        }
    }


    public void ThrowItem(Item theItem)
    {
        if (theItem != null && theItem.myCount != 0 && theItem.myIsThrowable)
        {
            Vector3 throwDirection = transform.forward;
            GameObject thrownObject = Instantiate(theItem.myThrownObjectPrefab, transform.position, transform.rotation);
            Rigidbody thrownObjectRigidbody = thrownObject.GetComponent<Rigidbody>();
            thrownObjectRigidbody.useGravity = true;
            thrownObjectRigidbody.AddForce(throwDirection * 60, ForceMode.Impulse);
            thrownObjectRigidbody.AddTorque(throwDirection * theItem.myThrowForce, ForceMode.Impulse);
        }
    }


    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}