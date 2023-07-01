using Cinemachine;
using UnityEngine;

public class AimStateManager : MonoBehaviour
{
    [SerializeField] private Transform myCameraFollowPos;
    public AxisState myXAxis, myYAxis;

    // Start is called before the first frame update
    private void start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void update()
    {
        _ = myXAxis.Update(Time.deltaTime);
        _ = myYAxis.Update(Time.deltaTime);
    }

    private void lateUpdate()
    {
        myCameraFollowPos.localEulerAngles = new Vector3(myYAxis.Value, myCameraFollowPos.localEulerAngles.y,
            myCameraFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, myXAxis.Value, transform.eulerAngles.z);
    }
}