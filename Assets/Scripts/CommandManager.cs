using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandManager : MonoBehaviour
{
    public static CommandManager instance = null;


    public enum Product {
        NONE = -1,
        PAIN_CEREAL = 0,
        BAGUETTE = 1,
        PAIN_DE_MIE = 2,
        ECLAIR = 3,
        CROISSANT = 4,
        PAIN_CHOCOLAT = 5
    }


    [Serializable]
    public struct Command
    {
        public Product product;
        public float time;
        public ClientAgentScript client;
    }

    [SerializeField] private int maxCommands = 5;
    [SerializeField] private int dayDuration = 600;
    [SerializeField] private int dayStartHour = 6;
    [SerializeField] private int dayEndHour = 18;

    private Command[] commands;
    private int currentCommands = 0;

    [SerializeField] private string[] productNames = { "Pain complet", "Baguette", "Pain de mie", "Éclair au chocolat", "Croissant", "Pain au chocolat" };

    [SerializeField] private TMPro.TextMeshProUGUI[] commandTexts;
    [SerializeField] private TMPro.TextMeshProUGUI[] commandTimes;
    [SerializeField] private TMPro.TextMeshProUGUI dayTimeText;
    [SerializeField] private TMPro.TextMeshProUGUI moneyText;

    [SerializeField] private ClientAgentScript clientAgentPrefab;

    private float currentDayTime = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        commands = new Command[maxCommands];
        for (int i = 0; i < commands.Length; i++)
        {
            commands[i].time = 0;
            commands[i].product = Product.NONE;
        }

        beginTime = Time.time;
        UpdateDisplay();


        StartCoroutine(ClientGenerationCoroutine());
    }
    float beginTime;
    private IEnumerator ClientGenerationCoroutine()
    {
        beginTime = Time.time;
        //Instantiate clients randomly, not more than 5 clients at the same time, their duration time is random and can be upgraded with the upgrade system
        while (Time.time < beginTime + dayDuration)
        {
            currentDayTime = Time.time - beginTime;
            if (currentCommands < maxCommands)
            {
                ClientAgentScript client = Instantiate(clientAgentPrefab);
                client.transform.position = client.spawnPosition;
                //Randomize client 
                client.isWaiting = true;
                client.GetComponent<Animator>().SetFloat("walkingFactor", 1.0f);
                client.StartCoroutine(client.ClientProcess());

                yield return new WaitForSeconds(5.0f);

                //Randomize client patience
                float duration = UnityEngine.Random.Range(60.0f, 120.0f) * UpgradesManager.Instance.getClientsPatienceFactor();
                Command command = new Command();
                command.time = duration;
                command.product = (Product)UnityEngine.Random.Range(0, 6);
                command.client = client;

                TryAddNewCommand(command);
                
                yield return new WaitForSeconds(UnityEngine.Random.Range(20.0f, 40.0f));
            }
            else
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(10.0f, 20.0f));
            }
        }

        //End of the day when every Command is empty
        while (currentCommands > 0)
        {
            yield return null;
        }

        //End of the day wait 5 seconds and call EndDay
        yield return new WaitForSeconds(5.0f);
        EndDay();


    }

    private void EndDay()
    {
        SceneManager.LoadScene("Upgrades");
    }

    //RETURN True if the command is added, false if the command is not added
    public bool TryAddNewCommand(Command command)
    {
        if(currentCommands >= maxCommands)
        {
            return false;
        }

        //Insert command if there is an empty slot in a increasing time order
        for (int i = 0; i < commands.Length; i++)
        {
            if (commands[i].time == 0)
            {
                commands[i] = command;
                return true;
            }
            else if (commands[i].time < command.time)
            {
                //SHIFT THE rest of the commands
                Command temp = command;
                for (int j = i; j < commands.Length; j++)
                {
                    Command temp2 = commands[j];
                    commands[j] = temp;
                    temp = temp2;
                }

                break;
            }
        }

        currentCommands++;

        return true;
    }

    private void Update()
    {
        currentDayTime = Time.time - beginTime;
        //Decrease time of all commands
        for (int i = 0; i < commands.Length; i++)
        {
            commands[i].time -= Time.deltaTime;
            if (commands[i].time < 0)
            {
                //If the time is less than 0, remove the command
                ClearCommand(i);
            }
        }

        //Update Display
        UpdateDisplay();

    }

    private void ClearCommand(int i)
    {
        if (commands[i].client == null) return;

        //Remove command of index i
        commands[i].client.isWaiting = false;
        commands[i].time = 0;
        commands[i].product = Product.NONE;
        currentCommands--;

        for (int j = i; j < commands.Length-1; j++)
        {
            commands[j] = commands[j + 1];
        }
    }

    private void UpdateDisplay()
    {
        int timeOfDay = GameManager.Instance.timeOfDay;

        int hours = dayStartHour + timeOfDay / 60;
        int min = timeOfDay % 60;
        dayTimeText.text = hours.ToString("00") + ":" + min.ToString("00");

        moneyText.text = UpgradesManager.Instance?.getMoney().ToString() + "€";

        for (int i = 0; i < commands.Length; i++)
        {
            if (commands[i].time > 0 && commands[i].product != Product.NONE)
            {
                commandTexts[i].text = productNames[(int)commands[i].product];
                int minutes = (int)commands[i].time / 60;
                int seconds = (int)commands[i].time % 60;
                commandTimes[i].text = minutes.ToString("00") + ":" + seconds.ToString("00");
            }
            else
            {
                commandTexts[i].text = "";
                commandTimes[i].text = "";
            }
        }
    }

    public void TryToSell(Selectable selectable)
    {
        float earnedMoney = 0;
        Product expectedProduct = Product.NONE;
        switch (selectable.label)
        {
            case "Éclair au chocolat":
                earnedMoney = 200f;
                expectedProduct = Product.ECLAIR;
                break;
            case "Croissant":
                earnedMoney = 100f;
                expectedProduct = Product.CROISSANT;
                break;
            case "Pain au chocolat":
                earnedMoney = 150f;
                expectedProduct = Product.PAIN_CHOCOLAT;    
                break;
            case "Pain complet":
                earnedMoney = 90f;
                expectedProduct = Product.PAIN_CEREAL;
                break;
            case "Pain de mie":
                earnedMoney = 70f;
                expectedProduct = Product.PAIN_DE_MIE;
                break;
            case "Baguette":
                earnedMoney = 50f;
                expectedProduct = Product.BAGUETTE;
                break;

        }

        earnedMoney *= UpgradesManager.Instance.getClientsMoneyFactor();

        if(earnedMoney > 0)
        {
            for(int i = 0; i < commands.Length; i++)
            {
                if (commands[i].product == expectedProduct)
                {
                    ClearCommand(i);
                    UpgradesManager.Instance.addMoney((int)earnedMoney);
                    Select.instance.ResetSelection();
                    Destroy(selectable.gameObject);
                    break;
                }
            }

        }
    }
}
