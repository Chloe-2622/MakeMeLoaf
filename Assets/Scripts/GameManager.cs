using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Variable repr�sentant le temps de la journ�e, un float entre 0 (8h00 du mat d�but de journ�e) et 720 (720 minutes, 12h de plus, donc 18h fin de journ�e)
    public int timeOfDay;

    public GameObject baby;
    public Camera mainCamera;

    public bool hasDayStarted;
    public bool hasDayEnded;
    public bool isTimePassing;
    public float playerRange;

    public int minutesPerSecond;

    public float frustration_time_factor;

    public float debugTimeScale;

    public GameObject TimeUI;
    public GameObject GameOverPanel;
    public GameObject DayEndedPanel;

    public bool Focus;

    [Header("Upgrades")]
    public float babyCalm;
    public float playerHumor;
    public float furnaceSpeed;
    public float clientPatience;
    public float clientMoney;
    public bool isToyAvailable;
    public bool isCamera1Available;
    public bool isCamera2Available;
    public bool isCamera3Available;
    public bool isBellAvailable;
    public bool isTabletAvailable;

    public bool isBabyAngry;
    private UpgradesManager upgradeManager;

    // Instance statique du GameManager
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // baby = GameObject.Find("Baby");

        upgradeManager = UpgradesManager.Instance;
        mainCamera = GameObject.Find("Player").transform.Find("Main Camera").GetComponent<Camera>();

        TimeUI.GetComponent<TextMeshProUGUI>().text = "08:00";

        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Gardez le GameManager lors des changements de sc�ne
        }
        else
        {
            Destroy(gameObject); // D�truisez les doublons
        }

        if (timeOfDay == 720)
        {
            EndDay();
        }

        GetUpgrades();

        StartCoroutine(PassTime());
        hasDayStarted = true;

        Time.timeScale = debugTimeScale;
    }

    // Get deds upgrades du Upgrade Manager
    private void GetUpgrades()
    {
        babyCalm = upgradeManager.getBabyCalmFactor();
        playerHumor = upgradeManager.getPlayerHumorFactor();
        furnaceSpeed = upgradeManager.getfurnaceSpeedFactor();
        clientPatience = upgradeManager.getClientsPatienceFactor();
        clientMoney = upgradeManager.getClientsMoneyFactor();
        isToyAvailable = upgradeManager.isToysAvailable();
        isCamera1Available = upgradeManager.isCamera_1Available();
        isCamera2Available = upgradeManager.isCamera_2Available();
        isCamera3Available = upgradeManager.isCamera_3Available();
        isBellAvailable = upgradeManager.isBellAvailable();
        isTabletAvailable = upgradeManager.isTabletAvailable();

    }

    void Update()
    {
        if (hasDayStarted && !isTimePassing)
        {
            // StartCoroutine(PassTime());
            isTimePassing = true;
        }
    }

    public void EndDay()
    {
        if (isBabyAngry)
        {
            hasDayEnded = true;
            GameOverPanel.SetActive(true);
            Focus = false;

            Time.timeScale = 0f;
        } else
        {
            hasDayEnded = true;
            DayEndedPanel.SetActive(true);
            Focus = false;

            Time.timeScale = 0f;
        }

        
    }

    public void GoToUpgrade()
    {
        SceneManager.LoadScene("Upgrades");
    }

    private IEnumerator PassTime()
    {
        int i = 0;

        while (!hasDayEnded)
        {
           // Debug.Log(babyCalm);
            timeOfDay += minutesPerSecond;
            if (i % 2 == 0)
            {
                TimeUI.GetComponent<TextMeshProUGUI>().text = (6 + timeOfDay / 60).ToString("D2") + ":" + (timeOfDay % 60).ToString("D2");
            } else
            {
                TimeUI.GetComponent<TextMeshProUGUI>().text = (6 + timeOfDay / 60).ToString("D2") + " " + (timeOfDay % 60).ToString("D2");
            }
            i++;
            baby.GetComponent<Baby>().AddFrustration(babyCalm * frustration_time_factor * (isTabletAvailable ? 0 : 1) * minutesPerSecond);
            yield return new WaitForSeconds(1);
        }
        
    }
}
