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
    public enum SingleUpgradesType
    {
        Toys = 0,
        Camera_1 = 1,
        Camera_2 = 2,
        Camera_3 = 3,
        Bell = 4,
        Tablet = 5
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

    [Header("Toys")]
    [SerializeField] private int toysCost;
    [SerializeField] private bool toys;
    [SerializeField] private string toysTitle;
    [SerializeField] private string toysDescription;

    [Header("Camera 1")]
    [SerializeField] private int camera_1Cost;
    [SerializeField] private bool camera_1;
    [SerializeField] private string camera_1Title;
    [SerializeField] private string camera_1Description;

    [Header("Camera 2")]
    [SerializeField] private int camera_2Cost;
    [SerializeField] private bool camera_2;
    [SerializeField] private string camera_2Title;
    [SerializeField] private string camera_2Description;

    [Header("Camera 3")]
    [SerializeField] private int camera_3Cost;
    [SerializeField] private bool camera_3;
    [SerializeField] private string camera_3Title;
    [SerializeField] private string camera_3Description;

    [Header("Bell")]
    [SerializeField] private int bellCost;
    [SerializeField] private bool bell;
    [SerializeField] private string bellTitle;
    [SerializeField] private string bellDescription;

    [Header("Tablette")]
    [SerializeField] private int tabletCost;
    [SerializeField] private bool tablet;
    [SerializeField] private string tabletTitle;
    [SerializeField] private string tabletDescription;


    [HideInInspector] public UnityEvent costUpdateEvent;


    public void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
        DontDestroyOnLoad(gameObject);

        if (costUpdateEvent == null ) { costUpdateEvent = new UnityEvent(); }

        checkErrors();
    }

    public void setUpgradeValue(MultipleUpgradesType upgradeType, int value)
    {
        switch (upgradeType)
        {
            case MultipleUpgradesType.BabyCalm:
                babyCalm = value;
                break;
            case MultipleUpgradesType.PlayerHumor:
                playerHumor = value;
                break;
            case MultipleUpgradesType.FurnaceSpeed:
                furnaceSpeed = value;
                break;
            case MultipleUpgradesType.ClientsPatience:
                clientsPatience = value;
                break;
            case MultipleUpgradesType.ClientsMoney:
                clientsMoney = value;
                break;
        }
    }
    public void buy(SingleUpgradesType upgradeType, bool isBought)
    {
        switch (upgradeType)
        {
            case SingleUpgradesType.Toys:
                toys = isBought;
                break;                    
            case SingleUpgradesType.Camera_1:
                camera_1 = isBought;
                break;
            case SingleUpgradesType.Camera_2:
                camera_2 = isBought;
                break;
            case SingleUpgradesType.Camera_3:
                camera_3 = isBought;
                break;
            case SingleUpgradesType.Bell:
                bell = isBought;
                break;
            case SingleUpgradesType.Tablet:
                tablet = isBought;
                break;
        }
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

    public bool isUpgradeBought(SingleUpgradesType upgradeType)
    {
        switch (upgradeType)
        {
            case SingleUpgradesType.Toys:
                return toys;
            case SingleUpgradesType.Camera_1:
                return camera_1;
            case SingleUpgradesType.Camera_2:
                return camera_2;
            case SingleUpgradesType.Camera_3:
                return camera_3;
            case SingleUpgradesType.Bell:
                return bell;
            case SingleUpgradesType.Tablet:
                return tablet;
        }
        return false;
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

    public int getUpgradeCost(MultipleUpgradesType upgradeType, int upgradeLevel)
    {
        switch (upgradeType)
        {
            case MultipleUpgradesType.BabyCalm:
                return babyCalmCosts[upgradeLevel];
            case MultipleUpgradesType.PlayerHumor:
                return playerHumorCosts[upgradeLevel];
            case MultipleUpgradesType.FurnaceSpeed:
                return furnaceSpeedCosts[upgradeLevel];
            case MultipleUpgradesType.ClientsPatience:
                return clientsPatienceCosts[upgradeLevel];
            case MultipleUpgradesType.ClientsMoney:
                return clientsMoneyCosts[upgradeLevel];
        }
        return 0;
    }

    public int getUpgradeCost(SingleUpgradesType upgradeType)
    {
        switch (upgradeType)
        {
            case SingleUpgradesType.Toys:
                return toysCost;
            case SingleUpgradesType.Camera_1:
                return camera_1Cost;
            case SingleUpgradesType.Camera_2:
                return camera_2Cost;
            case SingleUpgradesType.Camera_3:
                return camera_3Cost;
            case SingleUpgradesType.Bell:
                return bellCost;
            case SingleUpgradesType.Tablet:
                return tabletCost;
        }
        return 0;
    }

    public string getUpgradeTitle(MultipleUpgradesType upgradeType)
    {
        switch (upgradeType)
        {
            case MultipleUpgradesType.BabyCalm:
                return babyCalmTitle;
            case MultipleUpgradesType.PlayerHumor:
                return playerHumorTitle;
            case MultipleUpgradesType.FurnaceSpeed:
                return furnaceSpeedTitle;
            case MultipleUpgradesType.ClientsPatience:
                return clientsPatienceTitle;
            case MultipleUpgradesType.ClientsMoney:
                return clientsMoneyTitle;
        }
        return null;
    }

    public string getUpgradeTitle(SingleUpgradesType upgradeType)
    {
        switch (upgradeType)
        {
            case SingleUpgradesType.Toys:
                return toysTitle;
            case SingleUpgradesType.Camera_1:
                return camera_1Title;
            case SingleUpgradesType.Camera_2:
                return camera_2Title;
            case SingleUpgradesType.Camera_3:
                return camera_3Title;
            case SingleUpgradesType.Bell:
                return bellTitle;
            case SingleUpgradesType.Tablet:
                return tabletTitle;
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

    public string getUpgradeDescription(SingleUpgradesType upgradeType)
    {
        switch (upgradeType)
        {
            case SingleUpgradesType.Toys:
                return toysDescription;
            case SingleUpgradesType.Camera_1:
                return camera_1Description;
            case SingleUpgradesType.Camera_2:
                return camera_2Description;
            case SingleUpgradesType.Camera_3:
                return camera_3Description;
            case SingleUpgradesType.Bell:
                return bellDescription;
            case SingleUpgradesType.Tablet:
                return tabletDescription;
        }
        return null;
    }


    // Money
    public int getMoney() { return money; }

    public void addMoney(int moneyDelta) { money += moneyDelta;}

    public void addToCostToPay(int costDelta) {  totalCost += costDelta; costUpdateEvent.Invoke(); }


    public int getTotalCost() { return totalCost; }

    public void deduceCost() { money -= totalCost; totalCost = 0;  costUpdateEvent.Invoke(); }


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

    // Get booleans
    public bool isToysAvailable() { return toys; }
    public bool isCamera_1Available() { return camera_1; }
    public bool isCamera_2Available() { return camera_2; }
    public bool isCamera_3Available() { return camera_3; }
    public bool isBellAvailable() { return bell; }
    public bool isTabletAvailable() { return tablet; }


}
