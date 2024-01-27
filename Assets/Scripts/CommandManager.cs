using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    [SerializeField] private int maxCommands = 5;

    private Command[] commands;
    private int currentCommands = 0;

    [SerializeField] private string[] productNames = {"Pain aux céréales", "Baguette", "Pain de mie", "Eclair", "Croissant", "Pain au chocolat"};

    [SerializeField] private TMPro.TextMeshProUGUI[] commandTexts;
    [SerializeField] private TMPro.TextMeshProUGUI[] commandTimes;

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

        //DEBUG

        Command command = new Command();
        command.product = Product.PAIN_CEREAL;
        command.time = 100;
        TryAddNewCommand(command);

        Command command2 = new Command();
        command2.product = Product.BAGUETTE;
        command2.time = 50;
        TryAddNewCommand(command2);

        Command command3 = new Command();
        command3.product = Product.PAIN_DE_MIE;
        command3.time = 70;
        TryAddNewCommand(command3);

        Command command4 = new Command();
        command4.product = Product.ECLAIR;
        command4.time = 30;
        TryAddNewCommand(command4);
        Command command5 = new Command();
        command5.product = Product.CROISSANT;
        command5.time = 10;
        TryAddNewCommand(command5);

        Command command6 = new Command();
        command6.product = Product.PAIN_CHOCOLAT;
        command6.time = 20;
        TryAddNewCommand(command6);

        UpdateDisplay();

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
            if (commands[i].time <= 0)
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
        //Remove command of index i
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
