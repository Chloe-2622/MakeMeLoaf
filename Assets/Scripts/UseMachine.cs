using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseMachine : MonoBehaviour
{
    [SerializeField] private InputActionReference UseInput;
    [SerializeField] private Camera cam;
    private RaycastHit hit;
    private Craft machine;

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
        UseInput.action.performed += UseAction;
    }

    private void OnDisable()
    {
        UseInput.action.performed -= UseAction;
    }
    private void Awake()
    {
    }

    private void UseAction(InputAction.CallbackContext context)
    {

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, gM.playerRange)
         && hit.transform.parent != null && hit.transform.parent.TryGetComponent<Craft>(out machine))
        {
            StartCoroutine(machine.MakeCraft());
            Debug.Log("t1");
        }
    }
}
