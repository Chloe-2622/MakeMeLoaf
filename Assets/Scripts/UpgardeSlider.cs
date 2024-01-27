using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgardeSlider : MonoBehaviour
{
    [SerializeField] UpgradesManager.MultipleUpgradesType upgradeType;
    [SerializeField] float fillTime;

    [Header("Prefab Intern Objects")]
    [SerializeField] private Image slider;
    [SerializeField] private Image previewSlider;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private TextMeshProUGUI nextUpgradeCost;

    [Header("Outside Prefabs")]
    [SerializeField] GameObject sliderBarsPrefab;

    private UpgradesManager upgradesManager;
    private int upgradeValue;
    private int numberOfUpgrades;    
    private int previewUpgradeValue;

    public void OnEnable()
    {
        upgradesManager = UpgradesManager.Instance;

        upgradesManager.costUpdateEvent.AddListener(checkNextUpgrades);
    }

    public void OnDisable()
    {
        upgradesManager.costUpdateEvent.RemoveAllListeners();
    }

    void Start()
    {
        upgradeValue = upgradesManager.getUpgradeValue(upgradeType);
        previewUpgradeValue = upgradeValue;
        numberOfUpgrades = upgradesManager.getNumberOfUpgardes(upgradeType) - 1;

        slider.fillAmount = (float)upgradeValue / (float)numberOfUpgrades;
        previewSlider.fillAmount = slider.fillAmount;

        setMinusButtonActive(false);
        setPlusButtonActive(!(upgradeValue == numberOfUpgrades));
        checkNextUpgrades();

        placeSiderBars();
    }

    // Slider Bars
    public void placeSiderBars()
    {
        for (int i = 1; i < numberOfUpgrades; i++)
        {
            GameObject bar = Instantiate(sliderBarsPrefab, slider.transform);

            bar.transform.localPosition = new Vector3(0, ((2 * i) / (float)numberOfUpgrades - 1) * 10 * slider.transform.lossyScale.y, 0);
            bar.transform.localScale = new Vector3(1, bar.transform.localScale.y / slider.transform.lossyScale.y, 1);
        }
    }

    // Preview Sliders
    public void increasePreview() { previewUpgradeValue++; upgradesManager.addToCostToPay(upgradesManager.getUpgradeCost(upgradeType, previewUpgradeValue)); previewUpgrade(); }

    public void decreasePreview() { upgradesManager.addToCostToPay(- upgradesManager.getUpgradeCost(upgradeType, previewUpgradeValue));  previewUpgradeValue--; previewUpgrade(); }

    public void previewUpgrade()
    {
        setMinusButtonActive(previewUpgradeValue != upgradeValue);
        setPlusButtonActive(previewUpgradeValue != numberOfUpgrades);

        checkNextUpgrades();

        StopAllCoroutines();
        StartCoroutine(FillPreview());
    }

    public IEnumerator FillPreview()
    {
        float t = 0f;
        float start = previewSlider.fillAmount;
        float end = (float)(previewUpgradeValue) / (float)numberOfUpgrades;
        while (t < fillTime)
        {
            t += Time.deltaTime;
            previewSlider.fillAmount = Mathf.Lerp(start, end, t / fillTime);
            yield return null;
        }
    }

    // When upgrade confirms
    public void applyUpgardes() 
    {
        Debug.Log("apply upgrades");
        upgradesManager.setUpgradeValue(upgradeType, previewUpgradeValue);
        setButtonsActive(false);
        StartCoroutine(FillSlider());
    }

    public IEnumerator FillSlider()
    {
        float t = 0f;
        float start = slider.fillAmount;
        float end = (float)(previewUpgradeValue) / (float)numberOfUpgrades;
        while (t < fillTime)
        {
            t += Time.deltaTime;
            slider.fillAmount = Mathf.Lerp(start, end, t / fillTime);
            yield return null;
        }
    }

    // Buttons management
    public void checkNextUpgrades()
    {
        if (previewUpgradeValue != numberOfUpgrades)
        {
            setPlusButtonActive(upgradesManager.getMoney() - upgradesManager.getTotalCost() - upgradesManager.getUpgradeCost(upgradeType, previewUpgradeValue + 1) >= 0);

            nextUpgradeCost.text = "Next upgrade : " + upgradesManager.getUpgradeCost(upgradeType, previewUpgradeValue + 1).ToString() + " G";
        }
        else
        {
            nextUpgradeCost.text = "Next upgrade : -";
        }
    }

    public void setPlusButtonActive(bool active) { plusButton.interactable = active; }

    public void setMinusButtonActive(bool active) { minusButton.interactable = active; }

    public void setButtonsActive(bool active) { minusButton.interactable = active; plusButton.interactable = active; }
}
