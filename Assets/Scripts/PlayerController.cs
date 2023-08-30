using System;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] private static int SPEED_BOOST_SOUND = 2;

    [Header("Speed")] [SerializeField] private float gravity;
    [SerializeField] private float myWalkSpeed = 2;
    [SerializeField] private float mySkateSpeed = 2;
    [SerializeField] private float myCurrentSkateSpeed = 0;
    [SerializeField] private float mySpeedMultiplier = 1f;
    [SerializeField] private float myMaximumSpeed = 10;
    [SerializeField] private float minSpeed = 2;
    [SerializeField] private float myRotationSpeed = 1000;
    [SerializeField] private float truckTightness = 1f;
    [SerializeField] private float myThrowForce = 1000;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private Vector3 offset = new Vector3(0, 1, 0); 
    [SerializeField] private float verticalOffset = -1.7f;  
    [SerializeField] private float horizontalOffset = 0.38f;


    [Header("Components")] [SerializeField]
    private GameObject mySkateboard;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Item myCurrentThrowable;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Transform myCameraTransform;
    [SerializeField] private Transform myCapsuleTransform;
    [SerializeField] private CharacterController myCharacterController;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private InventoryManager myInventoryManager;
    [SerializeField] private Transform leftFoot;  // Set these in the editor or find them by name/tag
    [SerializeField] private Transform rightFoot;
    private GameObject skateboardParent;

    [Header("Speed Boost")] [SerializeField]
    private float speedBoostDuration = 3f; // How long the speed boost lasts

    [SerializeField] private float speedBoostMultiplier = 2f;
    private bool isSpeedBoosted = false;
    private float speedBoostEndTime;

    private bool myIsSkating = false;
    private bool isLevelFinished = false;
    private bool canMove = true;
    private bool myIsThrowing = false;

    private float myJumpButtonGracePeriod;
    private float? myJumpButtonPressedTime;
    private readonly float myJumpSpeed;
    private float? myLastGroundedTime;

    private float myInputMagnitude;
    private float myYSpeed;

    #endregion

    #region Methods

    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCharacterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        skateboardParent = new GameObject("SkateboardParent");
        skateboardParent.transform.SetParent(transform); 
        UpdateSkateboardParentPosition();
        mySkateboard.transform.SetParent(skateboardParent.transform);
    }

    private void Update()
    {
        UpdateSkateboardParentPosition();

        AnimatorStateInfo stateInfo = myAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Skating"))
        {
            myIsSkating = true;
        }

        if (isSpeedBoosted && Time.time >= speedBoostEndTime)
        {
            DeactivateSpeedBoost();
        }

        if (myCharacterController.isGrounded)
        {
            myLastGroundedTime = Time.time;
            myYSpeed = -gravity * Time.deltaTime;
        }

        if (canMove)
        {
            CheckForInput();
            MoveCharacter();
        }

        if (isSpeedBoosted && Time.time >= speedBoostEndTime)
        {
            DeactivateSpeedBoost();
        }

        if (isLevelFinished) CarryOn();
    }
    
    private void UpdateSkateboardParentPosition()
    {
        Vector3 midpoint = (leftFoot.position + rightFoot.position) / 2;
        midpoint.y += verticalOffset;
        midpoint.x += horizontalOffset;
        skateboardParent.transform.position = midpoint;
    }

    private void AnimateCharacter()
    {
        myAnimator.SetFloat("InputMagnitude", myInputMagnitude);
        mySkateboard.SetActive(myIsSkating);

        _ = myCharacterController.velocity.magnitude;
        myAnimator.SetBool("isSkating", myIsSkating);
    }

    private void MoveCharacter()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        bool isMoving = Math.Abs(HorizontalInput) > 0.1f || Math.Abs(VerticalInput) > 0.1f;

        if (VerticalInput <= 0)
        {
            HorizontalInput = 0;
        }

        if (myIsSkating && isMoving)
        {
            myCurrentSkateSpeed = Mathf.Min(myCurrentSkateSpeed + Time.deltaTime, mySkateSpeed);
        }
        else
        {
            myCurrentSkateSpeed = minSpeed;
        }

        float Speed = myIsSkating ? myCurrentSkateSpeed : myWalkSpeed;
        Speed *= VerticalInput * mySpeedMultiplier;

        if (isSpeedBoosted)
        {
            Speed *= speedBoostMultiplier;
        }

        Vector3 MovementDirection = new Vector3(HorizontalInput * truckTightness, 0, VerticalInput * mySkateSpeed);
        MovementDirection.Normalize();
        myInputMagnitude = Mathf.Clamp01(MovementDirection.magnitude);

        AnimateCharacter();
        RotateToPlaneNormal();

        myYSpeed -= gravity * Time.deltaTime;
        if (myCharacterController.isGrounded)
        {
            myYSpeed = -gravity * Time.deltaTime;
        }

        Vector3 Velocity = MovementDirection * Speed;
        Velocity.y = myYSpeed;

        myCharacterController.Move(Velocity * Time.deltaTime);
        myAnimator.SetBool("isWalking", VerticalInput != 0);

        if (MovementDirection != Vector3.zero)
        {
            if (!audioSource.isPlaying && myIsSkating) audioSource.Play();
            Quaternion ToRotation = Quaternion.LookRotation(MovementDirection, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, ToRotation, myRotationSpeed * Time.deltaTime);
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void Jump()
    {
        // if (myCharacterController.isGrounded)
        // {
        //     myYSpeed = jumpSpeed; // Set vertical speed to jumpSpeed when grounded and jump is pressed
        // }
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myAnimator.SetTrigger("Throw");
            ThrowItem();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            myJumpButtonPressedTime = Time.time;
            myAnimator.SetTrigger("Ollie");
            Jump();
        }
    }

    public void ThrowItem()
    {
        Vector3 throwDirection = transform.forward;

        GameObject thrownObject =
            Instantiate(myCurrentThrowable.myThrownObjectPrefab, transform.position + offset, transform.rotation);
        Rigidbody thrownObjectRigidbody = thrownObject.GetComponent<Rigidbody>();
        thrownObjectRigidbody.useGravity = true;
        thrownObjectRigidbody.AddForce(throwDirection * myThrowForce, ForceMode.Impulse);
        thrownObjectRigidbody.AddTorque(throwDirection * myThrowForce, ForceMode.Impulse);
    }

    public void ActivateSpeedBoost()
    {
        GameManager.GAME.SoundManager.PlaySound(SPEED_BOOST_SOUND);
        isSpeedBoosted = true;
        mySpeedMultiplier += .5f;
        speedBoostEndTime = Time.time + speedBoostDuration;
        mySkateSpeed++;
        myMaximumSpeed++;
        minSpeed++;
    }

    private void DeactivateSpeedBoost()
    {
        isSpeedBoosted = false;
    }

    public void CarryOn()
    {
        canMove = false;
        transform.position += transform.forward * CurrentSkateSpeed * Time.deltaTime;
    }

    #endregion

    #region GetterSetters

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

    public bool IsThrowing
    {
        get => myIsThrowing;
        set => myIsThrowing = value;
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

    public bool IsLevelFinished
    {
        get => isLevelFinished;
        set => isLevelFinished = value;
    }

    public float Gravity
    {
        get => gravity;
        set => gravity = value;
    }

    public float CurrentSkateSpeed
    {
        get => myCurrentSkateSpeed;
        set => myCurrentSkateSpeed = value;
    }

    public float MinSpeed
    {
        get => minSpeed;
        set => minSpeed = value;
    }

    public float TruckTightness1
    {
        get => truckTightness;
        set => truckTightness = value;
    }

    public AudioSource AudioSource
    {
        get => audioSource;
        set => audioSource = value;
    }

    public Rigidbody Rb1
    {
        get => rb;
        set => rb = value;
    }

    public bool IsLevelFinished1
    {
        get => isLevelFinished;
        set => isLevelFinished = value;
    }

    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }

    #endregion
}