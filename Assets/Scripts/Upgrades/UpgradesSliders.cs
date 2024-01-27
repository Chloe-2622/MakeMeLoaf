using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UpgradesSliders : MonoBehaviour
{
    [SerializeField] UpgradesManager.MultipleUpgradesType upgradeType;
    [SerializeField] float fillTime;
    [SerializeField] Sprite iconSprite;

    [Header("Colors")]
    [SerializeField] private Color outlineColor;
    [SerializeField] private Color backgroundColor;
    [SerializeField] private Color previewColor;
    [SerializeField] private Color sliderColor;

    [Header("Prefabs")]
    [SerializeField] GameObject sliderBarsPrefab;

    [Header("Sliders")]
    [SerializeField] private Image previewSliderMain;
    [SerializeField] private Image previewSliderTop;
    [SerializeField] private Image sliderMain;
    [SerializeField] private Image sliderTop;

    [Header("Buttons")]
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI nextUpgradeCost;
    [SerializeField] private TextMeshProUGUI upgradeTitle;
    [SerializeField] private TextMeshProUGUI upgradeDescription;

    [Header("Others")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Image icon;


    private UpgradesManager upgradesManager;
    private int upgradeValue;
    private int numberOfUpgrades;
    private int previewUpgradeValue;
    private float topFillTime;


    public void OnDisable()
    {
        upgradesManager.costUpdateEvent.RemoveAllListeners();
    }

    void Start()
    {
        upgradesManager = UpgradesManager.Instance;

        upgradesManager.costUpdateEvent.AddListener(checkNextUpgrades);

        upgradeValue = upgradesManager.getUpgradeValue(upgradeType);
        previewUpgradeValue = upgradeValue;
        numberOfUpgrades = upgradesManager.getNumberOfUpgardes(upgradeType) - 1;

        sliderMain.fillAmount = (float)upgradeValue / (float)numberOfUpgrades;

        if (upgradeValue == numberOfUpgrades) { sliderTop.fillAmount = 1; }
        else { sliderTop.fillAmount = 0; }
        previewSliderMain.fillAmount = sliderMain.fillAmount;

        topFillTime = fillTime * 10 * sliderTop.transform.lossyScale.y * numberOfUpgrades / (10f * sliderMain.transform.lossyScale.y);

        infoPanel.SetActive(false);
        upgradeTitle.text = upgradesManager.getUpgradeTitle(upgradeType);
        upgradeDescription.text = upgradesManager.getUpgradeDescription(upgradeType);

        if (iconSprite != null) { icon.sprite = iconSprite; }
        else { icon.gameObject.SetActive(false); }

        placeSiderBars();
        setAllColors();

        setMinusButtonActive(false);
        setPlusButtonActive(upgradeValue != numberOfUpgrades);
        checkNextUpgrades();
    }

    // Slider Bars
    public void placeSiderBars()
    {
        float step = 20 * sliderMain.transform.localScale.y / (float)numberOfUpgrades;
        float start = -10 * sliderMain.transform.localScale.y;

        for (int i = 0; i < numberOfUpgrades; i++)
        {
            GameObject bar = Instantiate(sliderBarsPrefab, sliderMain.transform);
            bar.transform.localPosition = new Vector3(0, start + i*step, 0);
            bar.transform.localScale = new Vector3(1, bar.transform.localScale.y / sliderMain.transform.localScale.y, 1);
        }
    }

    // Preview Sliders
    public void increasePreview()
    {
        previewUpgradeValue++;

        setMinusButtonActive(previewUpgradeValue != upgradeValue);
        setPlusButtonActive(previewUpgradeValue != numberOfUpgrades);

        upgradesManager.addToCostToPay(upgradesManager.getUpgradeCost(upgradeType, previewUpgradeValue));
        previewRender();
    }

    public void decreasePreview()
    {
        previewUpgradeValue--;

        setMinusButtonActive(previewUpgradeValue != upgradeValue);
        setPlusButtonActive(previewUpgradeValue != numberOfUpgrades);

        upgradesManager.addToCostToPay(-upgradesManager.getUpgradeCost(upgradeType, previewUpgradeValue+1));
        previewRender();
    }

    public void previewRender()
    {
        StopAllCoroutines();
        StartCoroutine(FillPreview());
    }

    public IEnumerator FillPreview()
    {
        if (previewUpgradeValue + 1 == numberOfUpgrades && previewSliderTop.fillAmount != 0)
        {
            float topStart = previewSliderTop.fillAmount;
            float t = 0f;
            while (t < topFillTime)
            {
                t += Time.deltaTime;
                previewSliderTop.fillAmount = Mathf.Lerp(topStart, 0f, t / topFillTime);
                yield return null;
            }
        }

        float start = previewSliderMain.fillAmount;
        float end = (float)(previewUpgradeValue) / (float)numberOfUpgrades;
        float ti = 0f;
        while (ti < fillTime)
        {
            ti += Time.deltaTime;
            previewSliderMain.fillAmount = Mathf.Lerp(start, end, ti / fillTime);
            yield return null;
        }

        if (previewUpgradeValue == numberOfUpgrades && previewSliderTop.fillAmount != 1)
        {
            float topStart = previewSliderTop.fillAmount;
            float tim = 0f;
            while (tim < topFillTime)
            {
                tim += Time.deltaTime;
                previewSliderTop.fillAmount = Mathf.Lerp(topStart, 1f, tim / topFillTime);
                yield return null;
            }
        }
    }

    // When upgrade confirms
    public void applyUpgardes()
    {
        Debug.Log("apply upgrades");
        Debug.Log(upgradeType + " " + previewUpgradeValue);
        upgradesManager.setUpgradeValue(upgradeType, previewUpgradeValue);
        setButtonsActive(false);
        StartCoroutine(FillSlider());
    }

    public IEnumerator FillSlider()
    {
        float start = sliderMain.fillAmount;
        float end = (float)(previewUpgradeValue) / (float)numberOfUpgrades;
        float t = 0f;
        while (t < fillTime)
        {
            t += Time.deltaTime;
            sliderMain.fillAmount = Mathf.Lerp(start, end, t / fillTime);
            yield return null;
        }

        if (previewUpgradeValue == numberOfUpgrades)
        {
            float topStart = sliderTop.fillAmount;
            float time = 0f;
            while (time < topFillTime)
            {
                time += Time.deltaTime;
                sliderTop.fillAmount = Mathf.Lerp(topStart, 1f, time / topFillTime);
                yield return null;
            }
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

    // Info panel
    public void showInfo() { infoPanel.SetActive(true); }
    public void hideInfo() { infoPanel.SetActive(false); }
    
    // Colors
    public void setAllColors()
    {
        Transform searchBase = transform.Find("Slider Image");

        Transform outlineSearchBase = searchBase.transform.Find("Slider Outline");
        outlineSearchBase.Find("Base Outline").GetComponent<Image>().color = outlineColor;
        outlineSearchBase.Find("Junction Outline").GetComponent<Image>().color = outlineColor;
        outlineSearchBase.Find("Main Outline").GetComponent<Image>().color = outlineColor;
        outlineSearchBase.Find("Top Outline").GetComponent<Image>().color = outlineColor;

        Transform backgroundSearchBase = searchBase.transform.Find("Slider Background");
        backgroundSearchBase.Find("Base Background").GetComponent<Image>().color = backgroundColor;
        backgroundSearchBase.Find("Junction Background").GetComponent<Image>().color = backgroundColor;
        backgroundSearchBase.Find("Main Background").GetComponent<Image>().color = backgroundColor;
        backgroundSearchBase.Find("Top Background").GetComponent<Image>().color = backgroundColor;

        previewSliderMain.color = previewColor;
        previewSliderTop.color = previewColor;

        Transform sliderSearchBase = searchBase.transform.Find("Slider");
        sliderSearchBase.Find("Base").GetComponent<Image>().color = sliderColor;
        sliderSearchBase.Find("Junction").GetComponent<Image>().color = sliderColor;
        sliderMain.color = sliderColor;
        sliderTop.color = sliderColor;
    }
}
