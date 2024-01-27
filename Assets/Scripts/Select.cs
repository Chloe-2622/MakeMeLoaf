using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Select : MonoBehaviour
{
    private RaycastHit hit;
    [SerializeField] private Camera cam;
    [SerializeField] private float maxDistance;
    private GameObject selectedO;
    private Selectable selected;
    private Outline outline;
    [SerializeField] private TextMeshProUGUI labelUI;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistance)
         && (GameObject.ReferenceEquals(hit.transform.gameObject, selectedO) || selectedO == null)
         && hit.transform.gameObject.CompareTag("Selectionable"))
        {
            selectedO = hit.transform.gameObject;
            selected = selectedO.GetComponent<Selectable>();
            outline = selectedO.GetComponent<Outline>();
            outline.enabled = true;
            labelUI.text = selected.label;
        }
        else if(selectedO != null && outline.enabled) ResetSelection();
    }

    private void ResetSelection()
    {
        labelUI.text = "";
        outline.enabled = false;
    }
}
