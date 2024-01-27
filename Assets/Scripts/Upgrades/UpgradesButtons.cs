using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpgradesButtons : MonoBehaviour
{
    [SerializeField] UpgradesManager.SingleUpgradesType upgradeType;
    [SerializeField] Sprite iconSprite;

    [Header("Colors")]
    [SerializeField] private Color availableColor;
    [SerializeField] private Color previewColor;
    [SerializeField] private Color boughtColor;

    [Header("Button")]
    [SerializeField] private Button button;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI nextUpgradeCost;
    [SerializeField] private TextMeshProUGUI upgradeTitle;
    [SerializeField] private TextMeshProUGUI upgradeDescription;

    [Header("Others")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Image icon;

    private UpgradesManager upgradesManager;
    private bool isPreviewOn;
    private bool alreadyBought;
    private Image buttonImage;

    public void OnDisable()
    {
        upgradesManager.costUpdateEvent.RemoveAllListeners();
    }
    
    void Start()
    {
        upgradesManager = UpgradesManager.Instance;
        buttonImage = button.GetComponent<Image>();

        upgradesManager.costUpdateEvent.AddListener(checkUpgrades);

        alreadyBought = upgradesManager.isUpgradeBought(upgradeType);
        button.interactable = !alreadyBought;

        setButtonColor();

        infoPanel.SetActive(false);
        upgradeTitle.text = upgradesManager.getUpgradeTitle(upgradeType);
        upgradeDescription.text = upgradesManager.getUpgradeDescription(upgradeType);

        if (iconSprite != null) { icon.sprite = iconSprite; }
        else { icon.gameObject.SetActive(false); }


        isPreviewOn = false;
        checkUpgrades();
    }

    public void selectUpgrade()
    {
        if (!isPreviewOn)
        {
            upgradesManager.addToCostToPay(upgradesManager.getUpgradeCost(upgradeType));
        }
        else
        {
            upgradesManager.addToCostToPay(-upgradesManager.getUpgradeCost(upgradeType));
            buttonImage.color = availableColor;
        }
        isPreviewOn = !isPreviewOn;
        setButtonColor();
        checkUpgrades();
    }

    public void applyUpgardes()
    {
        Debug.Log("apply upgrades");
        upgradesManager.buy(upgradeType, isPreviewOn || alreadyBought);
        button.interactable = false;
    }

    public void setButtonColor()
    {
        if (alreadyBought) { buttonImage.color = boughtColor; }
        else if (isPreviewOn) { buttonImage.color = previewColor; }
        else { buttonImage.color = availableColor; }
    }

    public void checkUpgrades()
    {
        if (!alreadyBought && !isPreviewOn)
        {
            button.interactable = upgradesManager.getMoney() - upgradesManager.getTotalCost() - upgradesManager.getUpgradeCost(upgradeType) >= 0;

            nextUpgradeCost.text = "Upgrade Cost : " + upgradesManager.getUpgradeCost(upgradeType).ToString() + " G";
        }
        else
        {
            nextUpgradeCost.text = "Already bought";
        }
    }

    // Info panel
    public void showInfo() { infoPanel.SetActive(true); }
    public void hideInfo() { infoPanel.SetActive(false); }
}
