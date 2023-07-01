using UnityEngine;

public class Skateboard : MonoBehaviour
{
    public Animator myAnim;
    public Transform myCameraTransform;
    public CharacterController myController;
    public bool myIsGrounded = false;
    public bool myIsPushing = false;
    public Rigidbody myPlayer;
    private float mySpeed = 7;
    public float myTruckTightness;
    private float myXDir;
    private float myYDir;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        getInput();
        Vector3 Movement = new(myXDir, 0, myYDir);
        animateCharacter(Movement);
        Debug.Log(Movement.magnitude);

        moveCharacter(Movement);
    }

    private void moveCharacter(Vector3 theMovement)
    {
        myPlayer.AddForce(theMovement * mySpeed, ForceMode.Force);
        //player.AddRelativeTorque(new Vector3(0, 0, movement.x * truckTightness), ForceMode.Force);
    }

    private void getInput()
    {
        myXDir = Input.GetAxis("Horizontal");
        myYDir = Input.GetAxis("Vertical");
    }

    private void animateCharacter(Vector3 theMovement)
    {
        if (theMovement != Vector3.zero)
            myAnim.SetBool("IsMoving", true);
        else
            myAnim.SetBool("IsMoving", false);

        if (myIsPushing == false && theMovement.magnitude > 0f)
        {
            myAnim.SetFloat("direction", 1);
        }
        else if (myIsPushing)
        {
            myAnim.SetFloat("direction", .4f);
            mySpeed += 1;
        }
        else
        {
            myAnim.SetFloat("direction", 0);
        }
    }

    private void OnApplicationFocus(bool theFocus)
    {
        Cursor.lockState = theFocus ? CursorLockMode.Locked : CursorLockMode.None;
    }
}