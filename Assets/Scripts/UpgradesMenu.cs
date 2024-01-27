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
    [SerializeField] private List<UpgardeSlider> sliders;

    private UpgradesManager upgradesManager;

    public void OnEnable()
    {
        upgradesManager = UpgradesManager.Instance;

        upgradesManager.costUpdateEvent.AddListener(showGoldAndCost);
    }

    public void OnDisable()
    {
        upgradesManager.costUpdateEvent.RemoveAllListeners();
    }

    public void startGame()
    {
        foreach(UpgardeSlider slider in sliders) { Debug.Log("oui");  slider.applyUpgardes(); }
    }

    public void showGoldAndCost()
    {
        goldText.text = upgradesManager.getMoney().ToString() + " G " + " <color=#" + ColorUtility.ToHtmlStringRGB(costColor) + "> (- " + upgradesManager.getTotalCost() + ")";
    }
}
