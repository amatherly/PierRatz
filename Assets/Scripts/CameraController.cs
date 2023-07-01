using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform myOrientation;
    public float myXOffset;
    public float myXRot;

    public float myXSen;
    public float myYOffset;
    public float myYRot;
    public float myYSen;

    // Start is called before the first frame update
    private void start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void update()
    {
        var MouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * myXSen;
        var MouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * myYSen;
        ;

        myYRot += MouseX;
        myXRot -= MouseY;
        myXRot = Mathf.Clamp(myXRot, -90f, 90f);
        transform.rotation = Quaternion.Euler(myXRot + myXOffset, myYRot + myYOffset, 0);
        myOrientation.rotation = Quaternion.Euler(0, myYRot, 0);
    }
}