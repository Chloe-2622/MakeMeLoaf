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

    [Header("Sliders")]
    [SerializeField] private GameObject slidersObject;

    private UpgradesManager upgradesManager;
    private List<UpgardeSlider> slidersList = new List<UpgardeSlider>();


    public void OnEnable()
    {
        upgradesManager = UpgradesManager.Instance;

        upgradesManager.costUpdateEvent.AddListener(showGoldAndCost);

        for (int i = 0; i < slidersObject.transform.childCount; i++)
        {
            slidersList.Add(slidersObject.transform.GetChild(i).gameObject.GetComponent<UpgardeSlider>());
        }
    }

    public void OnDisable()
    {
        upgradesManager.costUpdateEvent.RemoveAllListeners();
    }

    public void startGame()
    {
        foreach(UpgardeSlider slider in slidersList) { Debug.Log("oui");  slider.applyUpgardes(); }
    }

    public void showGoldAndCost()
    {
        goldText.text = upgradesManager.getMoney().ToString() + " G " + " <color=#" + ColorUtility.ToHtmlStringRGB(costColor) + "> (- " + upgradesManager.getTotalCost() + ")";
    }
}
