using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private static int SPEED_BOOST_SOUND = 2;
    [SerializeField] private static int THROW_SOUND = 3;
    [SerializeField] private static int JUMP_SOUND = 2;
    
    [Header("Movement")] [SerializeField] private float currentSkateSpeed;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float horizontalOffset = 0.38f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float speedBoostMultiplier;
    [SerializeField] private Vector3 movement;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float skateSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float throwForce;
    [SerializeField] private float truckTightness;
    [SerializeField] private float verticalOffset = -1.7f;
    [SerializeField] private float walkSpeed;


    [Header("Components")] [SerializeField]
    private GameObject skateboard;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Item currentThrowable;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Transform leftFoot; // Set these in the editor or find them by name/tag
    [SerializeField] private Transform rightFoot;
    private GameObject skateboardParent;
    
    [Header("Ground Check Settings")] [SerializeField]
    private Transform groundCheck;

    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private bool isSkating = false;
    private bool isGrounded = false;
    private bool canMove = true;
    private bool isLevelFinished = false;
    private float inputMagnitude;
    private Vector3 velocity;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        skateboardParent = new GameObject("SkateboardParent");
        skateboardParent.transform.SetParent(transform);
        UpdateSkateboardParentPosition();
        skateboard.transform.SetParent(skateboardParent.transform);
    }

    private void Update()
    {
        UpdateSkateboardParentPosition();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        skateboard.SetActive(!stateInfo.IsName("Moving"));

        if (canMove)
        {
            CheckForInput();
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (CanMove)
        {
            MoveCharacter();
        }
    }

    private void UpdateSkateboardParentPosition()
    {
        Vector3 midpoint = (leftFoot.position + rightFoot.position) / 2;
        midpoint.y += verticalOffset;
        midpoint.x += horizontalOffset;
        skateboardParent.transform.position = midpoint;
    }

    private void MoveCharacter()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 forceToAdd = new Vector3(horizontalInput * TruckTightness, 0f, SkateSpeed);

        forceToAdd.Normalize();
        forceToAdd *= SkateSpeed;

        rb.AddForce(forceToAdd, ForceMode.VelocityChange);
        rb.AddForce(new Vector3(0, -gravity, 0), ForceMode.Acceleration);


        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, rb.velocity.normalized * maxSpeed, 0.2f);
        }

        if (forceToAdd != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(forceToAdd, Vector3.up);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
        }
    }

    private void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Throw");
            ThrowItem();
        }

        if (Input.GetKeyDown(KeyCode.E) && isGrounded)
        {
            animator.SetTrigger("Ollie");
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            GameManager.GAME.SoundManager.PlaySound(JUMP_SOUND);
        }
    }

    private void ThrowItem()
    {
        Vector3 throwDirection = transform.forward;
        GameObject thrownObject =
            Instantiate(currentThrowable.myThrownObjectPrefab, transform.position, transform.rotation);
        Rigidbody thrownObjectRigidbody = thrownObject.GetComponent<Rigidbody>();
        thrownObjectRigidbody.useGravity = true;
        thrownObjectRigidbody.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }

    public void ActivateSpeedBoost()
    {
        rb.AddForce(transform.forward * speedBoostMultiplier, ForceMode.Impulse);
       GameManager.GAME.SoundManager.PlaySound(SPEED_BOOST_SOUND);
    }
    

    public void CarryOn()
    {
        canMove = false;
        transform.position += transform.forward * CurrentSkateSpeed * Time.deltaTime;
    }

    public bool IsLevelFinished
    {
        get => isLevelFinished;
        set => isLevelFinished = value;
    }
    
    public float SpeedMultiplier
    {
        get => speedMultiplier;
        set => speedMultiplier = value;
    }
    
    public float SkateSpeed
    {
        get => skateSpeed;
        set => skateSpeed = value;
    }

    public float CurrentSkateSpeed
    {
        get => currentSkateSpeed;
        set => currentSkateSpeed = value;
    }
    public float MinSpeed
    {
        get => minSpeed;
        set => minSpeed = value;
    }
    public float RotationSpeed
    {
        get => rotationSpeed;
        set => rotationSpeed = value;
    }
    public float TruckTightness
    {
        get => truckTightness;
        set => truckTightness = value;
    }
    
    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

    public Rigidbody Rb
    {
        get => rb;
        set => rb = value;
    }
    
    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }
}