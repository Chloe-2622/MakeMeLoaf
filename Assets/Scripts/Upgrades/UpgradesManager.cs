using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Events;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance { get; private set; }

    public enum MultipleUpgradesType
    {
        BabyCalm = 0,
        PlayerHumor = 1,
        FurnaceSpeed = 2,
        ClientsPatience = 3,
        ClientsMoney = 4
    }

    [SerializeField] private int money;
    [SerializeField] private int totalCost;

    [Header("Baby Calm")]
    [SerializeField] private List<float> babyCalmFactors;
    [SerializeField] private List<int> babyCalmCosts;
    [SerializeField] private int babyCalm = 0;
    [SerializeField] private string babyCalmTitle;
    [SerializeField] private string babyCalmDescription;

    [Header("Playe rHumor")]
    [SerializeField] private List<float> playerHumorFactors;
    [SerializeField] private List<int> playerHumorCosts;
    [SerializeField] private int playerHumor = 0;
    [SerializeField] private string playerHumorTitle;
    [SerializeField] private string playerHumorDescription;

    [Header("Furnace Speed")]
    [SerializeField] private List<float> furnaceSpeedFactors;
    [SerializeField] private List<int> furnaceSpeedCosts;
    [SerializeField] private int furnaceSpeed = 0;
    [SerializeField] private string furnaceSpeedTitle;
    [SerializeField] private string furnaceSpeedDescription;

    [Header("Clients Patience")]
    [SerializeField] private List<float> clientsPatienceFactors;
    [SerializeField] private List<int> clientsPatienceCosts;
    [SerializeField] private int clientsPatience = 0;
    [SerializeField] private string clientsPatienceTitle;
    [SerializeField] private string clientsPatienceDescription;

    [Header("Clients Money")]
    [SerializeField] private List<float> clientsMoneyFactors;
    [SerializeField] private List<int> clientsMoneyCosts;
    [SerializeField] private int clientsMoney = 0;
    [SerializeField] private string clientsMoneyTitle;
    [SerializeField] private string clientsMoneyDescription;


    [HideInInspector] public UnityEvent costUpdateEvent;


    public void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);

        if (costUpdateEvent == null ) { costUpdateEvent = new UnityEvent(); }

        checkErrors();
    }

    public void Start()
    {
        costUpdateEvent.Invoke();
    }

    public int setUpgradeValue(MultipleUpgradesType upgradeType, int value)
    {
        switch (upgradeType)
        {
            case MultipleUpgradesType.BabyCalm:
                return babyCalm = value;
                break;
            case MultipleUpgradesType.PlayerHumor:
                return babyCalm = value;
                break;
            case MultipleUpgradesType.FurnaceSpeed:
                return furnaceSpeed = value;
                break;
            case MultipleUpgradesType.ClientsPatience:
                return clientsPatience = value;
                break;
            case MultipleUpgradesType.ClientsMoney:
                return clientsMoney = value;
                break;
        }
        return 0;
    }

    // Get upgrades information
    public int getUpgradeValue(MultipleUpgradesType upgradeType)
    {
        switch (upgradeType)
        {
            case MultipleUpgradesType.BabyCalm:
                return babyCalm;
            case MultipleUpgradesType.PlayerHumor:
                return playerHumor;
            case MultipleUpgradesType.FurnaceSpeed:
                return furnaceSpeed;
            case MultipleUpgradesType.ClientsPatience:
                return clientsPatience;
            case MultipleUpgradesType.ClientsMoney:
                return clientsMoney;
        }
        return 0;
    }

    public int getNumberOfUpgardes(MultipleUpgradesType upgradeType)
    {
        switch (upgradeType)
        {
            case MultipleUpgradesType.BabyCalm:
                return babyCalmFactors.Count;
            case MultipleUpgradesType.PlayerHumor:
                return playerHumorFactors.Count;
            case MultipleUpgradesType.FurnaceSpeed:
                return furnaceSpeedFactors.Count;
            case MultipleUpgradesType.ClientsPatience:
                return clientsPatienceFactors.Count;
            case MultipleUpgradesType.ClientsMoney:
                return clientsMoneyFactors.Count;
        }
        return 0;
    }

    public int getUpgradeCost(MultipleUpgradesType upgradeType, int nextUpgrade)
    {
        switch (upgradeType)
        {
            case MultipleUpgradesType.BabyCalm:
                return babyCalmCosts[nextUpgrade];
            case MultipleUpgradesType.PlayerHumor:
                return playerHumorCosts[nextUpgrade];
            case MultipleUpgradesType.FurnaceSpeed:
                return furnaceSpeedCosts[nextUpgrade];
            case MultipleUpgradesType.ClientsPatience:
                return clientsPatienceCosts[nextUpgrade];
            case MultipleUpgradesType.ClientsMoney:
                return clientsMoneyCosts[nextUpgrade];
        }
        return 0;
    }

    public string getUpgradeTitle(MultipleUpgradesType upgradeType)
    {
        switch (upgradeType)
        {
            case MultipleUpgradesType.BabyCalm:
                return babyCalmTitle;
                break;
            case MultipleUpgradesType.PlayerHumor:
                return playerHumorTitle;
                break;
            case MultipleUpgradesType.FurnaceSpeed:
                return furnaceSpeedTitle;
                break;
            case MultipleUpgradesType.ClientsPatience:
                return clientsPatienceTitle;
                break;
            case MultipleUpgradesType.ClientsMoney:
                return clientsMoneyTitle;
                break;
        }
        return null;
    }

    public string getUpgradeDescription(MultipleUpgradesType upgradeType)
    {
        switch (upgradeType)
        {
            case MultipleUpgradesType.BabyCalm:
                return babyCalmDescription;
            case MultipleUpgradesType.PlayerHumor:
                return playerHumorDescription;
            case MultipleUpgradesType.FurnaceSpeed:
                return furnaceSpeedDescription;
            case MultipleUpgradesType.ClientsPatience:
                return clientsPatienceDescription;
            case MultipleUpgradesType.ClientsMoney:
                return clientsMoneyDescription;
        }
        return null;
    }

    // Money
    public int getMoney() { return money; }

    public void addToCostToPay(int costDelta) {  totalCost += costDelta; costUpdateEvent.Invoke();  Debug.Log(totalCost); }

    public int getTotalCost() { return totalCost; }


    // Debug
    public void checkErrors()
    {
        if (babyCalmFactors.Count != babyCalmCosts.Count) { Debug.LogError("babyCalmFactors et babyCalmCosts n'ont pas la même longueur"); }
        if (babyCalmFactors.Count != babyCalmCosts.Count) { Debug.LogError("playerHumorFactors et playerHumorCosts n'ont pas la même longueur"); }
        if (babyCalmFactors.Count != babyCalmCosts.Count) { Debug.LogError("furnaceSpeedFactors et furnaceSpeedCosts n'ont pas la même longueur"); }
        if (babyCalmFactors.Count != babyCalmCosts.Count) { Debug.LogError("clientsPatienceFactors et clientsPatienceCosts n'ont pas la même longueur"); }
        if (babyCalmFactors.Count != babyCalmCosts.Count) { Debug.LogError("clientsMoneyFactors et clientsMoneyCosts n'ont pas la même longueur"); }
    }

    // Get factors
    public float getBabyCalmFactor() { return babyCalmFactors[babyCalm]; }
    public float getPlayerHumorFactor() { return playerHumorFactors[playerHumor]; }
    public float getfurnaceSppedFactor() { return furnaceSpeedFactors[furnaceSpeed]; }
    public float getClientsPatienceFactor() { return clientsPatienceFactors[clientsPatience]; }
    public float getClientsMoneyFactor() { return clientsMoneyFactors[clientsMoney]; }
}
