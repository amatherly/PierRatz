using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float mySkateSpeed = 10;
    private float myWalkSpeed = 7;
    public Animator myAnimator;
    public Transform myCameraTransform;
    public Transform myCapsuleTransform;
    public CharacterController myCharacterController;



    private bool myIsSkating = false;
    [SerializeField]
    private readonly float myJumpButtonGracePeriod;
    private float? myJumpButtonPressedTime; 
    private readonly float myJumpSpeed;
    private float? myLastGroundedTime;
    [SerializeField]
    private float myMaximumSpeed = 10;
    public float myOriginalStepOffset;
    private float myRotationSpeed = 1000;
    public GameObject mySkateboard;
    private float mySpeedMultiplier = 0.5f;
    private float myYSpeed;
    public float myThrowForce = 1000f;
    private float mySlope = 1f;
    public bool myIsThrowing = false;

    

    public InventoryManager myInventoryManager;

    // Start is called before the first frame update
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myCharacterController = GetComponent<CharacterController>();
        myOriginalStepOffset = myCharacterController.stepOffset;
    }

    // Update is called once per frame
    private void Update()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        if (VerticalInput != 0)
        {
            myAnimator.SetBool("IsMoving", true);
        }

        Vector3 MovementDirection = new(HorizontalInput, 0, VerticalInput);
        float InputMagnitude = Mathf.Clamp01(MovementDirection.magnitude);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            InputMagnitude /= 2;
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetButtonDown("Fire2"))
        {
            myIsSkating = !myIsSkating;
            Debug.Log("Skating: " + myIsSkating);
        }


        if (Input.GetKey(KeyCode.LeftControl))
        {

        }


        if (Input.GetButtonDown("Fire1"))
        {
            if (myInventoryManager.myCurrentThrowable.ThrowItem(myInventoryManager.myCurrentThrowable))
            {
                Debug.Log("Throwing item");
            }

            Debug.Log("Fire1 pressed");

        }

        mySkateboard.SetActive(myIsSkating);
        myAnimator.SetBool("isSkating", myIsSkating);

        myAnimator.SetFloat("InputMagnitude", InputMagnitude, 0.05f, Time.deltaTime);
        float Speed = InputMagnitude * myMaximumSpeed * mySpeedMultiplier;

        // Raycast downward to detect the plane
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit Hit))
        {
            Debug.Log("raycast success");

            // Calculate the rotation needed to align with the plane normal
            Quaternion TargetRotation = Quaternion.FromToRotation(myCapsuleTransform.up, Hit.normal) *
                                 myCapsuleTransform.rotation;
            Debug.Log("Angle: " + Hit.normal);
            //Debug.Log("Rotation: " + targetRotation);

            // Smoothly rotate the capsule towards the target rotation
            myCapsuleTransform.rotation = TargetRotation;

            mySpeedMultiplier = myIsSkating ? 2f : 1.5f;


            if (Physics.SphereCast(transform.position, .25f, Vector3.down, out RaycastHit hit, 3f))
            {
                mySlope = Vector3.Dot(transform.up, Vector3.Cross(Vector3.up, hit.normal));
            }



            MovementDirection = Quaternion.AngleAxis(myCameraTransform.rotation.eulerAngles.y, Vector3.up) *
                                MovementDirection;
            MovementDirection.Normalize();

            myYSpeed += Physics.gravity.y * Time.deltaTime;

            if (myCharacterController.isGrounded)
            {
                myLastGroundedTime = Time.time;
            }
            if (Input.GetButtonDown("Jump") || Input.GetKey(KeyCode.Space))
            {
                myJumpButtonPressedTime = Time.time;
                myAnimator.SetTrigger("Ollie");
            }


            if (Time.time - myLastGroundedTime <= myJumpButtonGracePeriod)
            
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

            if (MovementDirection != Vector3.zero)
            {
                Quaternion ToRotation = Quaternion.LookRotation(MovementDirection, Vector3.up);
                transform.rotation =
                    Quaternion.RotateTowards(transform.rotation, ToRotation, myRotationSpeed * Time.deltaTime);
            }

            Vector3 Velocity = MovementDirection * Speed;
            Velocity.y = myYSpeed;

            _ = myCharacterController.Move(Velocity * Time.deltaTime * (mySlope + 1));


        }
    }


