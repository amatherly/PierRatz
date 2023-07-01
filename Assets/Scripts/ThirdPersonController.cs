using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")] [Tooltip("Move speed of the character in m/s")]
        public float myMoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float mySprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")] [Range(0.0f, 0.3f)]
        public float myRotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float mySpeedChangeRate = 10.0f;

        public AudioClip myLandingAudioClip;
        public AudioClip[] myFootstepAudioClips;
        [Range(0, 1)] public float myFootstepAudioVolume = 0.5f;

        [Space(10)] [Tooltip("The height the player can jump")]
        public float myJumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float myGravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float myJumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float myFallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool myGrounded = true;

        [Tooltip("Useful for rough ground")] public float myGroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float myGroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask myGroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject myCinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float myTopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float myBottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float myCameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool myLockCameraPosition = false;

        // cinemachine
        private float myCinemachineTargetYaw;
        private float myCinemachineTargetPitch;

        // player
        private float mySpeed;
        private float myAnimationBlend;
        private float myTargetRotation;
        private float myRotationVelocity;
        private float myVerticalVelocity;
        private readonly float myTerminalVelocity = 53.0f;

        // timeout deltatime
        private float myJumpTimeoutDelta;
        private float myFallTimeoutDelta;

        // animation IDs
        private int myAnimIdSpeed;
        private int myAnimIdGrounded;
        private int myAnimIdJump;
        private int myAnimIdFreeFall;
        private int myAnimIdMotionSpeed;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator myAnimator;
        private CharacterController myController;
        private StarterAssetsInputs myInput;
        private GameObject myMainCamera;

        private const float THRESHOLD = 0.01f;

        private bool myHasAnimator;

        private bool IsCurrentDeviceMouse =>
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
            false;
#endif


        private void awake()
        {
            // get a reference to our main camera
            if (myMainCamera == null) myMainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        private void start()
        {
            myCinemachineTargetYaw = myCinemachineCameraTarget.transform.rotation.eulerAngles.y;

            myHasAnimator = TryGetComponent(out myAnimator);
            myController = GetComponent<CharacterController>();
            myInput = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError(
                "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            assignAnimationIDs();

            // reset our timeouts on start
            myJumpTimeoutDelta = myJumpTimeout;
            myFallTimeoutDelta = myFallTimeout;
        }

        private void update()
        {
            myHasAnimator = TryGetComponent(out myAnimator);

            jumpAndGravity();
            groundedCheck();
            move();
        }

        private void lateUpdate()
        {
            cameraRotation();
        }

        private void assignAnimationIDs()
        {
            myAnimIdSpeed = Animator.StringToHash("Speed");
            myAnimIdGrounded = Animator.StringToHash("Grounded");
            myAnimIdJump = Animator.StringToHash("Jump");
            myAnimIdFreeFall = Animator.StringToHash("FreeFall");
            myAnimIdMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void groundedCheck()
        {
            // set sphere position, with offset
            Vector3 SpherePosition = new(transform.position.x, transform.position.y - myGroundedOffset,
                transform.position.z);
            myGrounded = Physics.CheckSphere(SpherePosition, myGroundedRadius, myGroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (myHasAnimator) myAnimator.SetBool(myAnimIdGrounded, myGrounded);
        }

        private void cameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (myInput.myLook.sqrMagnitude >= THRESHOLD && !myLockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                var DeltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                myCinemachineTargetYaw += myInput.myLook.x * DeltaTimeMultiplier;
                myCinemachineTargetPitch += myInput.myLook.y * DeltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            myCinemachineTargetYaw = clampAngle(myCinemachineTargetYaw, float.MinValue, float.MaxValue);
            myCinemachineTargetPitch = clampAngle(myCinemachineTargetPitch, myBottomClamp, myTopClamp);

            // Cinemachine will follow this target
            myCinemachineCameraTarget.transform.rotation = Quaternion.Euler(
                myCinemachineTargetPitch + myCameraAngleOverride,
                myCinemachineTargetYaw, 0.0f);
        }

        private void move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            var TargetSpeed = myInput.mySprint ? mySprintSpeed : myMoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (myInput.myMove == Vector2.zero) TargetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            var CurrentHorizontalSpeed = new Vector3(myController.velocity.x, 0.0f, myController.velocity.z).magnitude;

            var SpeedOffset = 0.1f;
            var InputMagnitude = myInput.myAnalogMovement ? myInput.myMove.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (CurrentHorizontalSpeed < TargetSpeed - SpeedOffset ||
                CurrentHorizontalSpeed > TargetSpeed + SpeedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                mySpeed = Mathf.Lerp(CurrentHorizontalSpeed, TargetSpeed * InputMagnitude,
                    Time.deltaTime * mySpeedChangeRate);

                // round speed to 3 decimal places
                mySpeed = Mathf.Round(mySpeed * 1000f) / 1000f;
            }
            else
            {
                mySpeed = TargetSpeed;
            }

            myAnimationBlend = Mathf.Lerp(myAnimationBlend, TargetSpeed, Time.deltaTime * mySpeedChangeRate);
            if (myAnimationBlend < 0.01f) myAnimationBlend = 0f;

            // normalise input direction
            var InputDirection = new Vector3(myInput.myMove.x, 0.0f, myInput.myMove.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (myInput.myMove != Vector2.zero)
            {
                myTargetRotation = Mathf.Atan2(InputDirection.x, InputDirection.z) * Mathf.Rad2Deg +
                                   myMainCamera.transform.eulerAngles.y;
                var Rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, myTargetRotation, ref myRotationVelocity,
                    myRotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, Rotation, 0.0f);
            }


            var TargetDirection = Quaternion.Euler(0.0f, myTargetRotation, 0.0f) * Vector3.forward;

            // move the player
            _ = myController.Move(TargetDirection.normalized * (mySpeed * Time.deltaTime) +
                                  new Vector3(0.0f, myVerticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (myHasAnimator)
            {
                myAnimator.SetFloat(myAnimIdSpeed, myAnimationBlend);
                myAnimator.SetFloat(myAnimIdMotionSpeed, InputMagnitude);
            }
        }

        private void jumpAndGravity()
        {
            if (myGrounded)
            {
                // reset the fall timeout timer
                myFallTimeoutDelta = myFallTimeout;

                // update animator if using character
                if (myHasAnimator)
                {
                    myAnimator.SetBool(myAnimIdJump, false);
                    myAnimator.SetBool(myAnimIdFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (myVerticalVelocity < 0.0f) myVerticalVelocity = -2f;

                // Jump
                if (myInput.myJump && myJumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    myVerticalVelocity = Mathf.Sqrt(myJumpHeight * -2f * myGravity);

                    // update animator if using character
                    if (myHasAnimator) myAnimator.SetBool(myAnimIdJump, true);
                }

                // jump timeout
                if (myJumpTimeoutDelta >= 0.0f) myJumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // reset the jump timeout timer
                myJumpTimeoutDelta = myJumpTimeout;

                // fall timeout
                if (myFallTimeoutDelta >= 0.0f)
                {
                    myFallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (myHasAnimator) myAnimator.SetBool(myAnimIdFreeFall, true);
                }

                // if we are not grounded, do not jump
                myInput.myJump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (myVerticalVelocity < myTerminalVelocity) myVerticalVelocity += myGravity * Time.deltaTime;
        }

        private static float clampAngle(float theLfAngle, float theLfMin, float theLfMax)
        {
            if (theLfAngle < -360f) theLfAngle += 360f;

            if (theLfAngle > 360f) theLfAngle -= 360f;

            return Mathf.Clamp(theLfAngle, theLfMin, theLfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color TransparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
            Color TransparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

            Gizmos.color = myGrounded ? TransparentGreen : TransparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - myGroundedOffset, transform.position.z),
                myGroundedRadius);
        }

        private void OnFootstep(AnimationEvent theAnimationEvent)
        {
            if (theAnimationEvent.animatorClipInfo.weight > 0.5f)
                if (myFootstepAudioClips.Length > 0)
                {
                    var Index = Random.Range(0, myFootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(myFootstepAudioClips[Index],
                        transform.TransformPoint(myController.center), myFootstepAudioVolume);
                }
        }

        private void OnLand(AnimationEvent theAnimationEvent)
        {
            if (theAnimationEvent.animatorClipInfo.weight > 0.5f)
                AudioSource.PlayClipAtPoint(myLandingAudioClip, transform.TransformPoint(myController.center),
                    myFootstepAudioVolume);
        }
    }
}