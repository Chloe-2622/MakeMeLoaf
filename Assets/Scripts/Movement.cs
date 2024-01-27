using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerInput))]
public class Movement : MonoBehaviour
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


    [Header("Ground check")]
    private float height;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    [Header("Slope check")]
    private RaycastHit slopeHit;


    private float totalDist = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        height = playerCollider.height;
        baseCamHeight = cam.transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        // deplacement
        movement = MoveDir(moveInput.action.ReadValue<Vector2>());

        // anti patinoire
        isGrounded = Physics.Raycast(transform.position, Vector3.down, height + 0.2f, whatIsGround);
        if (isGrounded) rb.drag = drag;
        else rb.drag = 0;

        // pour bien marcher dans la pente
        if(OnSlope()) rb.AddForce(SlopeMoveDir(movement) * speed);
        else rb.AddForce(movement*speed);
        rb.velocity = rb.velocity.normalized * Mathf.Clamp(rb.velocity.magnitude, 0, maxSpeed);
        // rb.useGravity = !OnSlope();

        // rotation cam

        if (Input.GetKeyDown(KeyCode.Escape)) DebugFocus = !DebugFocus;
        if(DebugFocus) mouse = lookInput.action.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, mouse.x * sensi / 100, Space.World);
        cam.transform.Rotate(Vector3.right, mouse.y * sensi / 100, Space.Self);
        if(!(cam.transform.localEulerAngles.x < 65 || cam.transform.localEulerAngles.x > 295)) cam.transform.Rotate(Vector3.left, mouse.y * sensi / 100, Space.Self);

        if (rb.velocity.magnitude != 0f)
        {
            Vector3 pos = cam.transform.localPosition;
            totalDist += rb.velocity.magnitude * Time.deltaTime * NodeSpeed;
            pos.y = baseCamHeight + Mathf.Sin(totalDist * 2*Mathf.PI) * NodeSize;
            cam.transform.localPosition = pos;
        }
        else
        {
            Vector3 pos = cam.transform.localPosition;
            pos.y = pos.y * 0.999f + baseCamHeight*0.001f;
            cam.transform.localPosition = pos;
        }



    }

    private Vector3 MoveDir(Vector2 vec2)
    {
        return transform.forward * vec2.y + transform.right * vec2.x;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, height + 0.2f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0;
        }

        return false;
    }

    private Vector3 SlopeMoveDir(Vector3 dir)
    {
        return Vector3.ProjectOnPlane(dir, slopeHit.normal);
    }
}
