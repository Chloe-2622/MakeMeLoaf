using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Baby : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] private float movingSpeed;

    [Header("IA")]
    [SerializeField] private float frustration;
    [SerializeField] private float frustrationFactor = 1f;
    [SerializeField] private int activityLevel;
    [SerializeField] private int activityLevel_1;
    [SerializeField] private int activityLevel_2;
    [SerializeField] private int activityLevel_3;

    [SerializeField] private float timeBeforeNextAction;

    [Header("Pièces")]
    [SerializeField] private Vector3 cuisineMin;
    [SerializeField] private Vector3 cuisineMax;
    [SerializeField] private Vector3 magasinMin;
    [SerializeField] private Vector3 magasinMax;
    [SerializeField] private Vector3 sousSolMin;
    [SerializeField] private Vector3 sousSolMax;

    private NavMeshAgent agent;

    private GameManager gameManager = GameManager.Instance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movingSpeed;

        timeBeforeNextAction = 10f;
        activityLevel = 0;
    }

    void Update()
    {
        frustration = frustrationFactor * gameManager.timeOfDay;

        if (gameManager.timeOfDay == activityLevel_1) { activityLevel = 1; }
        if (gameManager.timeOfDay == activityLevel_2) { activityLevel = 2; }
        if (gameManager.timeOfDay == activityLevel_3) { activityLevel = 3; }

        if (activityLevel == 0)
        {
            IABaby_Activity_0();
        }
    }

    private void IABaby_Activity_0()
    {

        if (timeBeforeNextAction == 0f)
        {

        }

    }

    public IEnumerator IAactionCountdown()
    {
        while (timeBeforeNextAction > 0)
        {
            timeBeforeNextAction -= Time.deltaTime;
            yield return null;
        }
        timeBeforeNextAction = 0f;
    }
}
