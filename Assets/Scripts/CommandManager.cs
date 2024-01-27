using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    private Command[] commands;
    private int currentCommands = 0;

    [SerializeField] private string[] productNames = {"Pain aux céréales", "Baguette", "Pain de mie", "Eclair", "Croissant", "Pain au chocolat"};

    [SerializeField] private TMPro.TextMeshProUGUI[] commandTexts;
    [SerializeField] private TMPro.TextMeshProUGUI[] commandTimes;

    [SerializeField] private ClientAgentScript clientAgentPrefab;

    public float clientPatience = 1.0f;

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

        UpdateDisplay();


        StartCoroutine(ClientGenerationCoroutine());
    }

    private IEnumerator ClientGenerationCoroutine()
    {
        //Instantiate clients randomly, not more than 5 clients at the same time, their duration time is random and can be upgraded with the upgrade system
        while (true)
        {
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
                float duration = UnityEngine.Random.Range(60.0f, 120.0f) * clientPatience;
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
}
