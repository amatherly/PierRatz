using System;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsLevelFinished
    {
        get => isLevelFinished;
        set => isLevelFinished = value;
    }

    #region Fields

    [SerializeField] private readonly float mySkateSpeed = 18;
    private readonly float myWalkSpeed = 7;
    public Animator myAnimator;
    public Transform myCameraTransform;
    public Transform myCapsuleTransform;
    public CharacterController myCharacterController;
    private bool myIsSkating = false;
    private float myInputMagnitude;
    [SerializeField] private float truckTightness = 1f;
    public Rigidbody rb;
    private bool isLevelFinished = false;
    private float carryOnSpeed = 100f;
    private bool canMove = true;

    [SerializeField] private readonly float myJumpButtonGracePeriod;
    private float? myJumpButtonPressedTime;
    private readonly float myJumpSpeed;
    private float? myLastGroundedTime;


    [SerializeField] private readonly float myMaximumSpeed = 10;
    public float myOriginalStepOffset;
    private readonly float myRotationSpeed = 1000;


    [SerializeField] public GameObject mySkateboard;
    private float mySpeedMultiplier = 0.5f;
    private float myYSpeed;
    public float myThrowForce = 1000f;
    private readonly float mySlope = 1f;
    public bool myIsThrowing = false;
    public float myPushCounter = 0;
    public Item myCurrentThrowable;
    [SerializeField] public InventoryManager myInventoryManager;

    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCharacterController = GetComponent<CharacterController>();
        myOriginalStepOffset = myCharacterController.stepOffset;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        mySpeedMultiplier = myIsSkating ? 2f : 1.5f;
        myYSpeed += Physics.gravity.y * Time.deltaTime * 10;

        if (myCharacterController.isGrounded)
        {
            myLastGroundedTime = Time.time;
        }

        if (canMove)
        {
            CheckForInput();
            MoveCharacter();
        }

        if (isLevelFinished) CarryOn();
    }


    private void AnimateCharacter()
    {
        Debug.Log("Current Speed: " + rb.velocity.magnitude);
        myAnimator.SetFloat("InputMagnitude", myInputMagnitude);
        mySkateboard.SetActive(myIsSkating);

        _ = myCharacterController.velocity.magnitude;

        if (myIsSkating && rb.velocity.magnitude < myMaximumSpeed)
        {
            myAnimator.SetFloat("SkateSpeed", myPushCounter);
            myAnimator.SetBool("isPushing", true);
        }
        else
        {
            myAnimator.SetBool("isPushing", false);
        }

        myAnimator.SetBool("isSkating", myIsSkating);
    }

    private void MoveCharacter()
    {
        float Speed = myInputMagnitude * myMaximumSpeed * mySpeedMultiplier;
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        Vector3 MovementDirection = new Vector3(HorizontalInput * truckTightness, 0, VerticalInput * mySkateSpeed);
        MovementDirection.Normalize();
        myInputMagnitude = Mathf.Clamp01(MovementDirection.magnitude);

        AnimateCharacter();
        RotateToPlaneNormal();

        Vector3 Velocity = MovementDirection * Speed;
        Velocity.y = myYSpeed;

        _ = myCharacterController.Move(Velocity * Time.deltaTime);

        myAnimator.SetBool("isWalking", VerticalInput != 0);

        if (MovementDirection != Vector3.zero)
        {
            Quaternion ToRotation = Quaternion.LookRotation(MovementDirection, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, ToRotation, myRotationSpeed * Time.deltaTime);
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
        // // Raycast downward to detect the plane
        // if (Physics.Raycast(transform.position, -transform.up, out RaycastHit Hit))
        // {
        //     if (Hit.transform.tag == "Ground")
        //     {
        //         // Calculate the rotation needed to align with the plane normal
        //         Quaternion TargetRotation = Quaternion.FromToRotation(myCapsuleTransform.up, Hit.normal) *
        //                              myCapsuleTransform.rotation;
        //
        //         // Smoothly rotate the capsule towards the target rotation
        //         myCapsuleTransform.rotation = TargetRotation;
        //     }
        // }
    }


    private void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            myInputMagnitude /= 2;
        }

        else if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Fire1"))
        {
            myIsSkating = !myIsSkating;
            myAnimator.SetBool("isSkating", myIsSkating);
        }

        else if (Input.GetButtonDown("Jump"))
        {
            ThrowItem();
            myAnimator.SetTrigger("Throw");
        }

        else if (Input.GetButtonDown("Jump") || Input.GetKey(KeyCode.Space))
        {
            myJumpButtonPressedTime = Time.time;
            myAnimator.SetTrigger("Ollie");
        }
    }


    public void ThrowItem()
    {
        //if (theItem != null  && theItem.myIsThrowable)
        //{
        Vector3 throwDirection = transform.forward;
        GameObject thrownObject =
            Instantiate(myCurrentThrowable.myThrownObjectPrefab, transform.position, transform.rotation);
        Rigidbody thrownObjectRigidbody = thrownObject.GetComponent<Rigidbody>();
        thrownObjectRigidbody.useGravity = true;
        thrownObjectRigidbody.AddForce(throwDirection * 60, ForceMode.Impulse);
        thrownObjectRigidbody.AddTorque(throwDirection * myCurrentThrowable.myThrowForce, ForceMode.Impulse);
        //}
    }

    public void CarryOn()
    {
        canMove = false;
        transform.position += transform.forward * carryOnSpeed * Time.deltaTime;
    }


    private void OnApplicationFocus(bool focus)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    public float SkateSpeed => mySkateSpeed;

    public float WalkSpeed => myWalkSpeed;

    public Animator Animator
    {
        get => myAnimator;
        set => myAnimator = value;
    }

    public Transform CameraTransform
    {
        get => myCameraTransform;
        set => myCameraTransform = value;
    }

    public Transform CapsuleTransform
    {
        get => myCapsuleTransform;
        set => myCapsuleTransform = value;
    }

    public CharacterController CharacterController
    {
        get => myCharacterController;
        set => myCharacterController = value;
    }

    public bool IsSkating
    {
        get => myIsSkating;
        set => myIsSkating = value;
    }

    public float InputMagnitude
    {
        get => myInputMagnitude;
        set => myInputMagnitude = value;
    }

    public float TruckTightness
    {
        get => truckTightness;
        set => truckTightness = value;
    }

    public Rigidbody Rb
    {
        get => rb;
        set => rb = value;
    }

    public float JumpButtonGracePeriod => myJumpButtonGracePeriod;

    public float? JumpButtonPressedTime
    {
        get => myJumpButtonPressedTime;
        set => myJumpButtonPressedTime = value;
    }

    public float JumpSpeed => myJumpSpeed;

    public float? LastGroundedTime
    {
        get => myLastGroundedTime;
        set => myLastGroundedTime = value;
    }

    public float MaximumSpeed => myMaximumSpeed;

    public float OriginalStepOffset
    {
        get => myOriginalStepOffset;
        set => myOriginalStepOffset = value;
    }

    public float RotationSpeed => myRotationSpeed;

    public GameObject Skateboard
    {
        get => mySkateboard;
        set => mySkateboard = value;
    }

    public float SpeedMultiplier
    {
        get => mySpeedMultiplier;
        set => mySpeedMultiplier = value;
    }

    public float YSpeed
    {
        get => myYSpeed;
        set => myYSpeed = value;
    }

    public float ThrowForce
    {
        get => myThrowForce;
        set => myThrowForce = value;
    }

    public float Slope => mySlope;

    public bool IsThrowing
    {
        get => myIsThrowing;
        set => myIsThrowing = value;
    }

    public float PushCounter
    {
        get => myPushCounter;
        set => myPushCounter = value;
    }

    public Item CurrentThrowable
    {
        get => myCurrentThrowable;
        set => myCurrentThrowable = value;
    }

    public InventoryManager InventoryManager
    {
        get => myInventoryManager;
        set => myInventoryManager = value;
    }
}