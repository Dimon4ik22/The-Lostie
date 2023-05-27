using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    [Range(1, 20)] public float distance = 5f;
    [Range(1, 10)] public float height = 2f;
    [Range(0, 10)] public float smoothSpeed = 2f;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    [Header("Collision Settings")]
    public float collisionBuffer = 0.3f;
    public LayerMask collisionMask;

    [Header("Aiming Settings")]
    public KeyCode aimingModeKey = KeyCode.Mouse1; // Right button
    public Vector3 rightShoulderOffset = new Vector3(1, 0, 0);
    public bool aimingMode = false;


    private float pitch = 0;
    private float yaw = 0;
    private Vector3 currentSmoothVelocity;
    private Vector3 currentCameraPosition;

    [SerializeField]
    private InventoryUi inventoryUi;

    private void Start()
    {
        // Set initial camera rotation
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    private void Update()
    {
        if (!inventoryUi.InventoryOpen)
        {
            RotateView();
        }
    }

    private void RotateView()
    {
        // Rotate camera based on mouse input
        yaw += Input.GetAxis("Mouse X");
        pitch -= Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        // Check for aiming mode input
        if (Input.GetKeyDown(aimingModeKey))
        {
            aimingMode = !aimingMode;
        }

        // Update camera position and handle collision
        Vector3 targetCameraPosition = target.position - transform.forward * distance + Vector3.up * height;

        if (aimingMode)
        {
            targetCameraPosition += rightShoulderOffset;
        }

        RaycastHit hit;
        if (Physics.Linecast(target.position, targetCameraPosition, out hit, collisionMask))
        {
            distance = Mathf.Clamp((hit.point - target.position).magnitude - collisionBuffer, 0, 20);
        }
        else
        {
            distance = Mathf.Lerp(distance, 5f, Time.deltaTime);
        }

        // Smoothly move and rotate camera
        currentCameraPosition = Vector3.SmoothDamp(currentCameraPosition, targetCameraPosition, ref currentSmoothVelocity, smoothSpeed * Time.deltaTime);
        transform.position = currentCameraPosition;
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}