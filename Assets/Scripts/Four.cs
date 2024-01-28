using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class Four : MonoBehaviour
{

    [SerializeField] private GameObject porteFour;
    [SerializeField] private float foorDoorSpeed = 90.0f;

    [SerializeField] private float cookingTime = 30.0f;
    [SerializeField] private float coalTime = 60.0f;

    private bool isOpen = false;
    private Coroutine currentCoroutine = null;

    private Dictionary<string, string> recettes_F = new Dictionary<string, string>();

    public class CookingIngredient
    {
        public Ingredient ingredient;
        public float time;
        public bool resolved = false;
    }

    private List<CookingIngredient> cookingIngredients = new List<CookingIngredient>();

    [Header("prefab final")]
    [SerializeField] private GameObject pain_cPref;
    [SerializeField] private GameObject pain_bPref;
    [SerializeField] private GameObject pain_mPref;
    [SerializeField] private GameObject croissantPref;
    [SerializeField] private GameObject pain_chocoPref;
    [SerializeField] private GameObject eclairePref;
    [SerializeField] private GameObject charbonPref;

    public void Start()
    {
        recettes_F.Add("eclaire", "eclair_cru");
        recettes_F.Add("pain_c", "pain_c_cru");
        recettes_F.Add("pain_b", "pain_b_cru");
        recettes_F.Add("pain_chocolat", "pain_chocolat_cru");
        recettes_F.Add("croissant", "croissant_cru");
        recettes_F.Add("pain_m", "pain_m_cru");

        /*recettes_F.Add("coal", "eclaire_cru");
        recettes_F.Add("pain_c", "pain_c_cru");
        recettes_F.Add("pain_b", "pain_b_cru");
        recettes_F.Add("pain_chocolat", "pain_chocolat_cru");
        recettes_F.Add("croissant", "croissant_c");
        recettes_F.Add("pain_m", "pain_m_cru");*/


    }

    public void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, GameManager.Instance.playerRange)
         && hit.transform.gameObject == porteFour)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SwitchState();
            }
        }

        if (!isOpen)
        {
            foreach (CookingIngredient ci in cookingIngredients)
            {
                ci.time += Time.deltaTime * UpgradesManager.Instance.getfurnaceSpeedFactor();

                if (ci.time > cookingTime && !ci.resolved)
                {
                    Debug.Log(ci.ingredient.label);
                    GameObject prefab = null;


                    switch (ci.ingredient.label)
                    {
                        case "pain_c_cru":
                            prefab = pain_cPref;
                            break;
                        case "pain_b_cru":
                            prefab = pain_bPref;
                            break;
                        case "pain_m_cru":
                            prefab = pain_mPref;
                            break;
                        case "pain_chocolat_cru":
                            prefab = pain_chocoPref;
                            break;
                        case "croissant_cru":
                            prefab = croissantPref;
                            break;
                        case "eclair_cru":
                            prefab = eclairePref;
                            break;
                    }

                    GameObject newObject = Instantiate(prefab, ci.ingredient.transform.position, ci.ingredient.transform.rotation);
                    Destroy(ci.ingredient.gameObject);
                    ci.ingredient = newObject.GetComponent<Ingredient>();
                    ci.resolved = true;
                }

                if(ci.resolved && ci.time > coalTime)
                {
                    GameObject newObject = Instantiate(charbonPref, ci.ingredient.transform.position, ci.ingredient.transform.rotation);
                    Destroy(ci.ingredient.gameObject);
                    ci.ingredient = newObject.GetComponent<Ingredient>();
                }
            }
        }
    }

    private void SwitchState()
    {
        isOpen = !isOpen;
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        switch (isOpen)
        {
            case true:
                StartCoroutine(OpenCouroutine());
                break;
            case false:
                StartCoroutine(CloseCouroutine());
                break;
        }
    }

    IEnumerator OpenCouroutine()
    {

        float totalRotation = 0.0f;
        while (true)
        {
            porteFour.transform.Rotate(0, 0, foorDoorSpeed * Time.deltaTime);
            totalRotation += foorDoorSpeed * Time.deltaTime;
            if (totalRotation >= 135)
            {
                //porteFour.transform.localRotation = Quaternion.Euler(135, 0, 0);
                break;
            }

            yield return null;
        }
    }

    IEnumerator CloseCouroutine()
    {

        float totalRotation = 0.0f;
        while (true)
        {
            porteFour.transform.Rotate(0, 0, -foorDoorSpeed * Time.deltaTime);
            totalRotation += foorDoorSpeed * Time.deltaTime;
            if (totalRotation >= 135)
            {
                //porteFour.transform.localRotation = Quaternion.Euler(135, 0, 0);
                //porteFour.transform.localRotation = Quaternion.identity;
                break;
            }

            yield return null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Ingredient select = other.GetComponent<Ingredient>();
        if (select != null)
        {
            Debug.Log("Detected ! " + select.label);
            if (recettes_F.ContainsValue(select.label))
            {
                for (int i = 0; i < cookingIngredients.Count; i++)
                {
                    if (cookingIngredients[i].ingredient.gameObject.Equals(other.gameObject))
                    {
                        return;
                    }
                }

                cookingIngredients.Add(new CookingIngredient { ingredient = select, time = 0.0f, resolved = false });
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        
        Ingredient select = other.GetComponent<Ingredient>();
        if (select != null)
        {
            for(int i = 0; i < cookingIngredients.Count; i++)
            {
                if (cookingIngredients[i].ingredient == select)
                {
                    cookingIngredients.RemoveAt(i);

                    break;
                }
            }
        }
    }

}