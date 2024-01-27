using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradesMenu : MonoBehaviour
{
    [Header("Scene Gold")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Color costColor;

    [Header("Upgrades Choice")]
    [SerializeField] private GameObject slidersObject;
    [SerializeField] private GameObject buttonsObject;

    private UpgradesManager upgradesManager;
    private List<UpgradesSliders> slidersList = new List<UpgradesSliders>();
    private List<UpgradesButtons> buttonsList = new List<UpgradesButtons>();



    public void OnEnable()
    {
        upgradesManager = UpgradesManager.Instance;

        upgradesManager.costUpdateEvent.AddListener(showGoldAndCost);

        for (int i = 0; i < slidersObject.transform.childCount; i++)
        {
            slidersList.Add(slidersObject.transform.GetChild(i).gameObject.GetComponent<UpgradesSliders>());
        }
        for (int i = 0; i < buttonsObject.transform.childCount; i++)
        {
            buttonsList.Add(buttonsObject.transform.GetChild(i).gameObject.GetComponent<UpgradesButtons>());
        }
    }

    public void OnDisable()
    {
        upgradesManager.costUpdateEvent.RemoveAllListeners();
    }

    public void startGame()
    {
        foreach(UpgradesSliders slider in slidersList) { slider.applyUpgardes(); }
        foreach(UpgradesButtons button in buttonsList) { button.applyUpgardes(); }
    }

    public void showGoldAndCost()
    {
        goldText.text = upgradesManager.getMoney().ToString() + " G " + " <color=#" + ColorUtility.ToHtmlStringRGB(costColor) + "> (- " + upgradesManager.getTotalCost() + ")";
    }







}
