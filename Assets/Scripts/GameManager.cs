using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Variable repr�sentant le temps de la journ�e, un float entre 0 (8h00 du mat d�but de journ�e) et 720 (720 minutes, 12h de plus, donc 18h fin de journ�e)
    public int timeOfDay;

    public GameObject baby;

    public bool hasDayStarted;
    public bool hasDayEnded;
    public bool isTimePassing;

    public int minutesPerSecond;

    public float frustration_time_factor;

    // Instance statique du GameManager
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        //baby = GameObject.Find("Baby");

        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Gardez le GameManager lors des changements de sc�ne
        }
        else
        {
            Destroy(gameObject); // D�truisez les doublons
        }

        hasDayStarted = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (hasDayStarted && !isTimePassing)
        {
            StartCoroutine(PassTime());
            isTimePassing = true;
        }
    }

    private IEnumerator PassTime()
    {
        while (!hasDayEnded)
        {
            timeOfDay += minutesPerSecond;
            baby.GetComponent<Baby>().AddFrustration(frustration_time_factor * minutesPerSecond);
            yield return new WaitForSeconds(1);
        }
        
    }
}
