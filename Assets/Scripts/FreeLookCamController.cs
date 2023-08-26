using UnityEngine;
using Cinemachine;

public class FreeLookCamController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField]
    private CinemachineFreeLook freeLookCam;

    [Header("Controls")]
    [SerializeField, Range(0, 10)]
    private float lookSpeedX = 1.0f;

    [SerializeField, Range(0, 10)]
    private float lookSpeedY = 1.0f;

    private void Start()
    {
        // Ensure the cursor is locked to the game window and hidden
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Apply mouse input to the FreeLook Camera's axes
        if (freeLookCam)
        {
            freeLookCam.m_XAxis.Value += mouseX * lookSpeedX * Time.deltaTime;
            freeLookCam.m_YAxis.Value -= mouseY * lookSpeedY * Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        // Ensure the cursor is unlocked and visible when leaving the game
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}