using UnityEngine;

public class BallController : MonoBehaviour
{
    public float myGravity;
    public Rigidbody myRb;
    public float myRotationSpeed;
    public float mySpeed;

    // Start is called before the first frame update
    private void start()
    {
    }

    // Update is called once per frame
    private void update()
    {
        var HorInput = Input.GetAxisRaw("Horizontal") * myRotationSpeed;
        var VerInput = Input.GetAxisRaw("Vertical") * mySpeed;

        myRb.AddForce(new Vector3(HorInput, myGravity, VerInput));
    }
}