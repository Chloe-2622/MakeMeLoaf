using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    private Dictionary<int, string> inventory = new Dictionary<int, string>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(inventory);
    }

    private void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient;
        if (other.gameObject.TryGetComponent<Ingredient>(out ingredient)) inventory.Add(other.gameObject.GetInstanceID(),ingredient.label);
    }

    private void OnTriggerExit(Collider other)
    {
        Ingredient ingredient;
        if (other.gameObject.TryGetComponent<Ingredient>(out ingredient)) inventory.Remove(other.gameObject.GetInstanceID());
    }
}
