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
    [SerializeField] private string clientsMoneyFescription;


    [HideInInspector] public UnityEvent costUpdateEvent;


    public void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);

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
                break;
            case MultipleUpgradesType.PlayerHumor:
                return playerHumor;
                break;
            case MultipleUpgradesType.FurnaceSpeed:
                return furnaceSpeed;
                break;
            case MultipleUpgradesType.ClientsPatience:
                return clientsPatience;
                break;
            case MultipleUpgradesType.ClientsMoney:
                return clientsMoney;
                break;
        }
        return 0;
    }

    public int getNumberOfUpgardes(MultipleUpgradesType upgradeType)
    {
        switch (upgradeType)
        {
            case MultipleUpgradesType.BabyCalm:
                return babyCalmFactors.Count;
                break;
            case MultipleUpgradesType.PlayerHumor:
                return playerHumorFactors.Count;
                break;
            case MultipleUpgradesType.FurnaceSpeed:
                return furnaceSpeedFactors.Count;
                break;
            case MultipleUpgradesType.ClientsPatience:
                return clientsPatienceFactors.Count;
                break;
            case MultipleUpgradesType.ClientsMoney:
                return clientsMoneyFactors.Count;
                break;
        }
        return 0;
    }

    public int getUpgradeCost(MultipleUpgradesType upgradeType, int nextUpgrade)
    {
        switch (upgradeType)
        {
            case MultipleUpgradesType.BabyCalm:
                return babyCalmCosts[nextUpgrade];
                break;
            case MultipleUpgradesType.PlayerHumor:
                return playerHumorCosts[nextUpgrade];
                break;
            case MultipleUpgradesType.FurnaceSpeed:
                return furnaceSpeedCosts[nextUpgrade];
                break;
            case MultipleUpgradesType.ClientsPatience:
                return clientsPatienceCosts[nextUpgrade];
                break;
            case MultipleUpgradesType.ClientsMoney:
                return clientsMoneyCosts[nextUpgrade];
                break;
        }
        return 0;
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
