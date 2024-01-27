using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    private Dictionary<int, Ingredient> inventory = new Dictionary<int, Ingredient>();
    private Dictionary<string, string[]> recettes_P = new Dictionary<string, string[]>();
    private Dictionary<string, string[]> recettes_PdT = new Dictionary<string, string[]>();
    private Dictionary<string, string[]> recettes_F = new Dictionary<string, string[]>();
    private Dictionary<string, string[]> recettes_M = new Dictionary<string, string[]>();

    [SerializeField] private GameObject pate_cPref;

    public enum CraftType
    {
        Petrin,
        PlanDeTravail,
        Four,
        Melange
    }

    [SerializeField] private CraftType stationType; 

    // Start is called before the first frame update
    void Start()
    {
        recettes_P.Add("pate_c", new string[] { "farine_c", "eau" });
        recettes_P.Add("pate_b", new string[] { "farine_b", "eau" });
        recettes_M.Add("pate_croissant", new string[] { "farine_b", "lait", "sucre", "beurre", "oeuf" });
        recettes_P.Add("pate_m", new string[] { "farine_b", "lait", "sucre", "beurre"});
        recettes_M.Add("pate_choux", new string[] { "farine_b", "eau", "sucre", "beurre"});

        recettes_PdT.Add("pain_c_cru", new string[] { "pate_c" });
        recettes_PdT.Add("pain_b_cru", new string[] { "pate_b" });
        recettes_PdT.Add("pain_chocolat_cru", new string[] { "pate_croissant", "chocolat" });
        recettes_PdT.Add("croissant_cru", new string[] { "pate_croissant" });
        recettes_PdT.Add("pain_m_cru", new string[] { "pate_m" });
        recettes_PdT.Add("eclaire_cru", new string[] { "pate_choux" });

        recettes_F.Add("eclaire", new string[] { "eclaire_cru" });

        recettes_F.Add("pain_c", new string[] { "pain_c_cru" });
        recettes_F.Add("pain_b", new string[] { "pain_b_cru" });
        recettes_F.Add("pain_chocolat", new string[] { "pain_chocolat_cru" });
        recettes_F.Add("croissant", new string[] { "croissant_cru" });
        recettes_F.Add("pain_m", new string[] { "pain_m_cru" });

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Ingredient ingredient;
        if (other.gameObject.TryGetComponent<Ingredient>(out ingredient)) inventory.Add(other.gameObject.GetInstanceID(),ingredient);
    }

    private void OnTriggerExit(Collider other)
    {
        Ingredient ingredient;
        if (other.gameObject.TryGetComponent<Ingredient>(out ingredient)) inventory.Remove(other.gameObject.GetInstanceID());
    }

    private void PrintDic()
    {
        string s = "";
        foreach(KeyValuePair<int, Ingredient> item in inventory)
        {
            s += item.Value.label + ", ";
        }
        Debug.Log(s);
    }

    private string FindCraft(Dictionary<string, string[]> recettes)
    {
        foreach(KeyValuePair<string, string[]> recette in recettes)
        {
            if(GotAllItems(recette.Value)) return recette.Key;
        }
        return "rien";
    }

    private bool GotAllItems(string[] recette)
    {
        bool isValid = true;
        foreach(string item in recette)
        {
            isValid &= Contains(item);
        }

        return isValid;
    }

    private bool Contains(string ingredient)
    {
        foreach(KeyValuePair<int, Ingredient> item in inventory) if (item.Value.label == ingredient) return true;
        return false;
    }

    public void MakeCraft()
    {
        PrintDic();
        string resultat = "";
        switch (stationType)
        {
            case CraftType.Petrin:
                resultat = FindCraft(recettes_P);
                SpawnObject(resultat);
                DeletUsedItem(resultat, recettes_P);
                break;
            case CraftType.Melange:
                Debug.Log(FindCraft(recettes_M));
                break;
            case CraftType.PlanDeTravail:
                Debug.Log(FindCraft(recettes_PdT));
                break;
            case CraftType.Four:
                Debug.Log(FindCraft(recettes_F));
                break;
        }


    }

    private void SpawnObject(string name)
    {
        switch (name)
        {
            case "pate_c":
                Instantiate(pate_cPref, transform.position + Vector3.up, Quaternion.identity);
                return;
        }
    }

    private void DeletUsedItem(string name, Dictionary<string, string[]> recettes)
    {
        string[] recette = recettes[name];
        foreach(string ingredient in recette)
        {
            foreach (KeyValuePair<int, Ingredient> item in inventory)
            {
                if (item.Value.label == ingredient) Destroy(item.Value.gameObject);
            }
        }
    }
}
