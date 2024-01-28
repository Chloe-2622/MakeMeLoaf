using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradesMenu : MonoBehaviour
{
    [SerializeField] private GameObject confirmationPanel;

    [Header("Scene Gold")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Color costColor;

    [Header("Upgrades Choice")]
    [SerializeField] private GameObject slidersObject;
    [SerializeField] private GameObject buttonsObject;

    [Header("Buttons with conditions")]
    [SerializeField] GameObject tablette;


    private UpgradesManager upgradesManager;
    private List<UpgradesSliders> slidersList = new List<UpgradesSliders>();
    private List<UpgradesButtons> buttonsList = new List<UpgradesButtons>();


    public void OnEnable()
    {
        upgradesManager = UpgradesManager.Instance;

        upgradesManager.costUpdateEvent.AddListener(showGoldAndCost);
        confirmationPanel.SetActive(false);

        for (int i = 0; i < slidersObject.transform.childCount; i++)
        {
            slidersList.Add(slidersObject.transform.GetChild(i).gameObject.GetComponent<UpgradesSliders>());
        }
        for (int i = 0; i < buttonsObject.transform.childCount; i++)
        {
            buttonsList.Add(buttonsObject.transform.GetChild(i).gameObject.GetComponent<UpgradesButtons>());
        }
    }

    public void Start()
    {
        showGoldAndCost();
        /*if (upgradesManager.getUpgradeValue(UpgradesManager.MultipleUpgradesType.BabyCalm) == upgradesManager.getNumberOfUpgardes(UpgradesManager.MultipleUpgradesType.BabyCalm) - 1)
        {
            tablette.SetActive(true);
        }
        else { tablette.SetActive(false); }/*/
        verifyTablet();
    }

    public void OnDisable()
    {
        upgradesManager.costUpdateEvent.RemoveAllListeners();
    }

    public void askConfirmation() { confirmationPanel.SetActive(true); }
    public void cancel() { confirmationPanel.SetActive(false); }

    public void applyUpgrades()
    {
        confirmationPanel.SetActive(false);
        foreach(UpgradesSliders slider in slidersList) { slider.applyUpgardes(); }
        foreach(UpgradesButtons button in buttonsList) { button.applyUpgardes(); }
        upgradesManager.deduceCost();

        StartCoroutine(WaitTillSlidersEnd());
    }

    public void showGoldAndCost()
    {
        goldText.text = "G : " + upgradesManager.getMoney().ToString() + " <color=#" + ColorUtility.ToHtmlStringRGB(costColor) + "> (- " + upgradesManager.getTotalCost() + ")";
    }

    public void verifyTablet()
    {
        int babyCalmLevel = upgradesManager.getUpgradeValue(UpgradesManager.MultipleUpgradesType.BabyCalm);
        int numberOfBabyCalmLevel = upgradesManager.getNumberOfUpgardes(UpgradesManager.MultipleUpgradesType.BabyCalm);
        if (babyCalmLevel == numberOfBabyCalmLevel-1)
        {
            tablette.SetActive(true);
        }
        else { tablette.SetActive(false); }
    }

    public IEnumerator WaitTillSlidersEnd()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Upgrades");
    }
}
