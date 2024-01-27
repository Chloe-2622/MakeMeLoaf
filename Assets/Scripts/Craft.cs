using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    private Dictionary<int, string> inventory = new Dictionary<int, string>();
    private Dictionary<string, string[]> recettes = new Dictionary<string, string[]>();

    // Start is called before the first frame update
    void Start()
    {
        recettes.Add("pate_c", new string[] { "farine_c", "eau" });
        recettes.Add("pate_b", new string[] { "farine_b", "eau" });
        recettes.Add("pate_croissant", new string[] { "farine_b", "lait", "sucre", "beurre", "oeuf" });
        recettes.Add("pate_m", new string[] { "farine_b", "lait", "sucre", "beurre"});
        recettes.Add("pate_choux", new string[] { "farine_b", "eau", "sucre", "beurre"});

        recettes.Add("pain_c_cru", new string[] { "pate_c" });
        recettes.Add("pain_b_cru", new string[] { "pate_b" });
        recettes.Add("pain_chocolat_cru", new string[] { "pate_croissant", "chocolat" });
        recettes.Add("croissant_cru", new string[] { "pate_croissant" });
        recettes.Add("pain_m_cru", new string[] { "pate_m" });
        recettes.Add("eclaire_cru", new string[] { "pate_choux" });

        recettes.Add("eclaire", new string[] { "eclaire_cru" });




    }

    // Update is called once per frame
    void Update()
    {
        PrintDic(inventory);
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

    private void PrintDic(Dictionary<int, string> dic)
    {
        string s = "";
        foreach(KeyValuePair<int, string> item in inventory)
        {
            s += item.Value + ", ";
        }
        Debug.Log(s);
    }

    private bool GotAllItems(string[] recette)
    {
        bool isValid = true;
        foreach(string item in recette)
        {
            isValid &= inventory.ContainsValue(item);
        }

        return isValid;
    }
}
