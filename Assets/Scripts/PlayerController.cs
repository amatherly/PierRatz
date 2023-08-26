using System;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float skateSpeed = 6f;
    public float slopeForce = 10f;
    public float slopeForceRayLength = 0.1f;
    public float maxSkateSpeed = 20f;
    public float minSkateSpeed = 2f;
    public float skateAcceleration = 3f;
    public float truckTightness = 2;
    private CharacterController charController;

    [Header("Gravity Settings")]
    public float gravity = 9.8f;

    [Header("Camera Settings")]
    public Transform cameraTransform;


    private bool isSkating = true;
    private Animator animator;
    private float currentSkateSpeed = 0f;
    private Vector3 velocity; // To keep track of the downward velocity

    void Start()
    {
        animator = GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isSkating = !isSkating;
            currentSkateSpeed = walkSpeed;
        }


    }

    private void FixedUpdate()
    {
        AlignWithGround();
        HandleMovement();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // Vector3 moveDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        moveDirection.y = 0f;
        moveDirection.Normalize();

        animator.SetFloat("InputMagnitude", direction.magnitude);
        animator.SetBool("isWalking", direction.magnitude > 0);
        animator.SetBool("isSkating", isSkating && direction.magnitude > 0);

        float movementSpeed = isSkating ? currentSkateSpeed : walkSpeed;
        bool isPushing = currentSkateSpeed < maxSkateSpeed;

        animator.SetBool("isPushing", isPushing);

        if (isPushing && currentSkateSpeed < maxSkateSpeed && isSkating)
        {
            currentSkateSpeed += skateAcceleration * Time.deltaTime;
            currentSkateSpeed = Mathf.Clamp(currentSkateSpeed, 0f, maxSkateSpeed);
        }

        moveDirection *= movementSpeed;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, slopeForceRayLength) && isSkating)
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            float slopeDirectionFactor = Vector3.Dot(moveDirection.normalized, hit.normal) > 0 ? 1 : -1;
            float slopeSpeedAdjustment = slopeAngle / 45f * slopeDirectionFactor * slopeForce;
            moveDirection += moveDirection.normalized * slopeSpeedAdjustment * movementSpeed;
        }

        if (horizontal != 0 || vertical != 0) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * truckTightness);
        }
        
        Debug.Log("is grounded: " + IsGrounded());
        
        if (IsGrounded())
        {
            moveDirection.y = 0f;  
        }
        else
        {
            moveDirection.y -= gravity;  
        }
        
        charController.Move(moveDirection * Time.deltaTime);
    }

    void AlignWithGround()
    {
        if (isSkating && Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, slopeForceRayLength))
        {
            Quaternion toRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime * 30f);
        }
    }


    bool IsGrounded()
    {
        // Define the origin and direction of the ray
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.down;

        // Perform the raycast
        if (isSkating && Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, slopeForceRayLength))
        {
            Debug.DrawRay(rayOrigin, rayDirection * slopeForceRayLength, Color.green);
            return true;
        }
        Debug.DrawRay(rayOrigin, rayDirection * slopeForceRayLength, Color.red);
        return false;
    }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("collision detected");
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger detected");
    }
}
