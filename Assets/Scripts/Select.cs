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


    private GameManager gM = GameManager.Instance;

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gM);
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, gM.playerRange)
         && (GameObject.ReferenceEquals(hit.transform.gameObject, selectedO) || selectedO == null)
         && hit.transform.gameObject.TryGetComponent<Selectable>(out selected))
        {
            selectedO = hit.transform.gameObject;
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
