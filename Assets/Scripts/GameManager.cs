using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // Variable repr�sentant le temps de la journ�e, un float entre 0 (8h00 du mat d�but de journ�e) et 720 (720 minutes, 12h de plus, donc 18h fin de journ�e)
    public int timeOfDay;

    public bool hasDayStarted;
    public bool hasDayEnded;
    public bool isTimePassing;
    public float playerRange;



    // Instance statique du GameManager
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (hasDayStarted && !isTimePassing)
        {
            StartCoroutine(PassTime());
        }
    }

    private IEnumerator PassTime()
    {
        while (!hasDayEnded)
        {
            timeOfDay++;
            yield return new WaitForSeconds(1);
        }
        
    }
}
