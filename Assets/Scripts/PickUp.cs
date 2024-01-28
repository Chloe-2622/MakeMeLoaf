using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    [SerializeField] private InputActionReference takeInput;
    [SerializeField] private Camera cam;
    private RaycastHit hit;
    public static PickUpable pickUped;
    [SerializeField] private Vector3 handPostion;


    private GameManager gM;

    // Start is called before the first frame update
    void Start()
    {
        gM = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        takeInput.action.started += PickUpAction;
        takeInput.action.canceled += DropAction;
    }

    private void OnDisable()
    {
        takeInput.action.started -= PickUpAction;
        takeInput.action.canceled -= DropAction;
    }

    private void PickUpAction(InputAction.CallbackContext contect)
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, gM.playerRange)
         && hit.transform.gameObject.TryGetComponent<PickUpable>(out pickUped))
        {
            GameObject o = pickUped.gameObject;
            Rigidbody rb = o.GetComponent<Rigidbody>();

            
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            o.transform.parent = cam.transform;
            o.transform.localPosition = handPostion;
            
            
        }
    }

    private void DropAction(InputAction.CallbackContext context)
    {
        if (pickUped == null) return;
        GameObject o = pickUped.gameObject;
        Rigidbody rb = o.GetComponent<Rigidbody>();

        rb.useGravity = true;
        rb.isKinematic = false;

        o.transform.parent = null;
        
    }
}
