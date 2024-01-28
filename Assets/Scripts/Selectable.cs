using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Outline))]
public class Selectable : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public string label;
    [SerializeField] private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
