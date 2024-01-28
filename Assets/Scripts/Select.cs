using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Select : MonoBehaviour
{
    private RaycastHit hit;
    [SerializeField] private Camera cam;
    private GameObject selectedO;
    private Selectable selected;
    private Outline outline;
    [SerializeField] private TextMeshProUGUI labelUI;

    public static Select instance;


    private GameManager gM;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        gM = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, gM.playerRange)
         && hit.transform.gameObject.TryGetComponent<Selectable>(out selected))
        {
            selectedO = hit.transform.gameObject;
            outline = selectedO.GetComponent<Outline>();
            outline.enabled = true;
            labelUI.text = selected.label;
        }
        else if(selectedO != null && outline.enabled) ResetSelection();
    }

    public void ResetSelection()
    {
        labelUI.text = "";
        outline.enabled = false;
    }
}
