// using UnityEngine;
//
// public class CameraController : MonoBehaviour
// {
//     public Transform playerTransform;
//     public Vector3 offset;                  // Offset from player to camera
//     public float zoomSpeed = 4f;            // How quickly we zoom
//     public float minZoom = 5f;              // Minimum zoom distance
//     public float maxZoom = 15f;             // Maximum zoom distance
//     public float yawSpeed = 100f;           // Speed of camera rotation around player
//
//     private float currentZoom = 10f;        // Current zoom distance
//     private float currentYaw = 0f;          // Current camera rotation
//     public float rotationSensitivity = 100f;  // Sensitivity of the mouse rotation
//
//     private float yaw = 0f;     // Rotation around the player
//     private float pitch = 0f;   // Camera's vertical angle
//
//     void Update()
//     {
//         // Capture mouse movement to rotate the camera
//         currentYaw -= Input.GetAxis("Mouse X") * rotationSensitivity * Time.deltaTime;
//     }
//
//     void LateUpdate()
//     {
//         // Apply the rotation and position the camera
//         transform.position = playerTransform.position + offset;
//         transform.RotateAround(playerTransform.position, Vector3.up, currentYaw);
//         transform.LookAt(playerTransform);
//     }
//
// }