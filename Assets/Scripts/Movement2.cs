using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerInput))]
public class Movement2 : MonoBehaviour
{

    [Header("Miscelionous")]
    [SerializeField] Camera cam;

    [Header("physics")]
    [SerializeField] Rigidbody rb;
    [SerializeField] CapsuleCollider playerCollider;

    [Header("movement")]
    [SerializeField] InputActionReference moveInput, lookInput;
    [SerializeField] float speed;
    [SerializeField] float drag;
    [SerializeField] float maxSpeed;
    private Vector3 movement;

    [Header("Cam Movement")]
    [SerializeField] private float sensi;
    private float baseCamHeight;
    [SerializeField] private float NodeSpeed;
    [SerializeField] private float NodeSize;
    private Vector2 mouse;
    private bool DebugFocus = true;

    private float xRotation;
    private float yRotation;


    [Header("Ground check")]
    private float height;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Slope check")]
    private RaycastHit slopeHit;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = transform.Find("Main Camera").GetComponent<Camera>();
        playerCollider = GetComponent<CapsuleCollider>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        height = playerCollider.height;
        baseCamHeight = cam.transform.localPosition.y;


        cam.transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookVector = lookInput.action.ReadValue<Vector2>();

        yRotation += lookVector.x * 0.07f;
        xRotation += lookVector.y * 0.07f;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);

        Debug.DrawLine(transform.position, transform.position - 2.1f * transform.up);

        if (!Physics.Raycast(transform.position, - transform.up,  2.1f))
        {
            rb.drag = 0;
        } else
        {
            rb.drag = 2;
        }

    }

    private void FixedUpdate()
    {
        Vector2 movVector = moveInput.action.ReadValue<Vector2>();

        rb.AddForce(speed * (movVector.y * transform.forward + movVector.x * transform.right).normalized, ForceMode.Acceleration);
    }


}
