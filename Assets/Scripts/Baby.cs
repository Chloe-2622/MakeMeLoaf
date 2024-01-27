using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Baby : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] private float movingSpeed;

    [Header("IA")]
    [SerializeField] private float timeBeforeStartingMoving;
    [SerializeField] private float total_frustration;
    [SerializeField] private float frustration_factor;
    [SerializeField] private float frustration_multiplier;
    [SerializeField] private int activityLevel;
    [SerializeField] private int activityLevel_1;
    [SerializeField] private int activityLevel_2;
    [SerializeField] private int activityLevel_3;
    [SerializeField] private float gameOver_frustration_factor;
    [SerializeField] private float timeBeforeNextAction;

    [Header("Pièces")]
    [SerializeField] private GameObject RoomDelimitations;
    [SerializeField] private Vector3 cuisineMin;
    [SerializeField] private Vector3 cuisineMax;
    [SerializeField] private Vector3 magasinMin;
    [SerializeField] private Vector3 magasinMax;
    [SerializeField] private Vector3 sousSolMin;
    [SerializeField] private Vector3 sousSolMax;

    [Header("UI")]
    [SerializeField] private Image babyImage;
    [SerializeField] private float babyBarMinX = -385;


    private NavMeshAgent agent;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;

        // Rooms delimitations;

        cuisineMin = RoomDelimitations.transform.Find("CuisineMin").position;
        cuisineMax = RoomDelimitations.transform.Find("CuisineMax").position;
        magasinMin = RoomDelimitations.transform.Find("MagasinMin").position;
        magasinMax = RoomDelimitations.transform.Find("MagasinMax").position;
        //sousSolMin = RoomDelimitations.transform.Find("SousSolMin").position;
        //sousSolMax = RoomDelimitations.transform.Find("SousSolMax").position;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = movingSpeed;

        timeBeforeNextAction = 5f;
        StartCoroutine(IAactionCountdown());
        activityLevel = 0;
    }

    void Update()
    {
        frustration_factor = total_frustration / (1 + total_frustration);
        // Debug.Log(frustration_factor / gameOver_frustration_factor);
        babyImage.transform.position = Vector3.Lerp(babyImage.transform.position, new Vector3(babyBarMinX, babyImage.transform.position.y, babyImage.transform.position.z), frustration_factor / gameOver_frustration_factor);

        if (gameManager.timeOfDay == activityLevel_1) { activityLevel = 1; }
        if (gameManager.timeOfDay == activityLevel_2) { activityLevel = 2; }
        if (gameManager.timeOfDay == activityLevel_3) { activityLevel = 3; }

        if (activityLevel == 0)
        {
            IABaby_Activity_0();
        }
        if (activityLevel == 1)
        {
            IABaby_Activity_1();
        }
    }

    private void IABaby_Activity_0()
    {

        if (timeBeforeNextAction == 0f)
        {
            if (Random.Range(0f, 1f) < frustration_factor)
            {
                agent.SetDestination(RandomPositionInArea(cuisineMin, cuisineMax));
            }

            timeBeforeNextAction = 10f;
            StartCoroutine(IAactionCountdown());
        }

    }

    private void IABaby_Activity_1()
    {

        if (timeBeforeNextAction == 0f)
        {
            if (Random.Range(0f, 1f) < frustration_factor)
            {
                if (Random.Range(0, 2) == 0)
                {
                    agent.SetDestination(RandomPositionInArea(cuisineMin, cuisineMax));
                } else
                {
                    agent.SetDestination(RandomPositionInArea(magasinMin, magasinMax));
                }
            }

            timeBeforeNextAction = 10f;
            StartCoroutine(IAactionCountdown());
        }

    }



    public void AddFrustration(float frustration)
    {
        total_frustration += frustration;
    }

    private IEnumerator IAactionCountdown()
    {
        while (timeBeforeNextAction > 0)
        {
            timeBeforeNextAction -= Time.deltaTime;
            yield return null;
        }
        timeBeforeNextAction = 0f;
    }

    private Vector3 RandomPositionInArea(Vector3 areaMin, Vector3 areaMax)
    {
        return new Vector3(Random.Range(areaMin.x, areaMax.x), areaMin.y, Random.Range(areaMin.z, areaMax.z));
    }
}
