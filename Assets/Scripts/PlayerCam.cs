using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public Transform myCameraPosition;
    public float myXOffset;
    public float myYOffset;


    // Start is called before the first frame update
    private void start()
    {
    }

    // Update is called once per frame
    private void update()
    {
        //update the position
        transform.position = myCameraPosition.position + new Vector3(myXOffset, myYOffset, 0);
        //transform.position = cameraPosition.position;
    }
}