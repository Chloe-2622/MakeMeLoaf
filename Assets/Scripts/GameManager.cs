using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Variable représentant le temps de la journée, un float entre 0 (8h00 du mat début de journée) et 720 (720 minutes, 12h de plus, donc 18h fin de journée)
    public int timeOfDay;

    public GameObject baby;

    public bool hasDayStarted;
    public bool hasDayEnded;
    public bool isTimePassing;
    public float playerRange;

    public int minutesPerSecond;

    public float frustration_time_factor;

    public float debugTimeScale;

    public GameObject TimeUI;



    // Instance statique du GameManager
    public static GameManager Instance { get; private set; }

    private void Awake()
    {




        //baby = GameObject.Find("Baby");

        TimeUI.GetComponent<TextMeshProUGUI>().text = "08 : 00";

        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Gardez le GameManager lors des changements de scène
        }
        else
        {
            Destroy(gameObject); // Détruisez les doublons
        }


        //StartCoroutine(PassTime());
        hasDayStarted = true;

        Time.timeScale = debugTimeScale;
    }

    private void GetUpgrades()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hasDayStarted && !isTimePassing)
        {
            // StartCoroutine(PassTime());
            isTimePassing = true;
        }
    }

    private IEnumerator PassTime()
    {
        int i = 0;

        while (!hasDayEnded)
        {
            timeOfDay += minutesPerSecond;
            if (i % 2 == 0)
            {
                TimeUI.GetComponent<TextMeshProUGUI>().text = (8 + timeOfDay / 60).ToString("D2") + ":" + (timeOfDay % 60).ToString("D2");
            } else
            {
                TimeUI.GetComponent<TextMeshProUGUI>().text = (8 + timeOfDay / 60).ToString("D2") + " " + (timeOfDay % 60).ToString("D2");
            }
            i++;
            baby.GetComponent<Baby>().AddFrustration(frustration_time_factor * minutesPerSecond);
            yield return new WaitForSeconds(1);
        }
        
    }
}
