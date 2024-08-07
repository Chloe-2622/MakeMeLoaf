using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Baby : MonoBehaviour
{
    /*
     * Int�grer musiques et sons b�b�.
     * B�b� qui chiale a intervalle random
     * Textures prefabs ingr�dients
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     */


    [Header("-------------- Mode présentation --------------")]
    [SerializeField] private bool presentationMode;
    [SerializeField] private int gameSpeed;

    [Header("Stats")]
    [SerializeField] private float movingSpeed;

    [Header("IA")]
    [SerializeField] private float timeBeforeStartingMoving;
    [SerializeField] private float timeBetweenEachAction;
    [SerializeField] private float total_frustration;
    [SerializeField] private float frustration_multiplier;
    [SerializeField] private bool isCrying;
    [SerializeField] private BabySkillCheck babySkillCheck;

    [Header("-------------- Frustration Factor --------------")]
    [SerializeField] private float frustration_factor;

    [Header("Activity level")]
    [SerializeField] private int currentActivityLevel;
    [SerializeField] private int activityLevel_1;
    [SerializeField] private int activityLevel_2;
    [SerializeField] private int activityLevel_3;
    [SerializeField] private float gameOver_frustration_factor;
    [SerializeField] private float timeBeforeNextAction;

    [Header("Cry stats")]
    [SerializeField] private float cryStat0;
    [SerializeField] private float cryStat1;
    [SerializeField] private float cryStat2;
    [SerializeField] private float cryStat3;

    [Header("Pièces")]
    [SerializeField] private GameObject RoomDelimitations;
    [SerializeField] private Vector3 cuisineMin;
    [SerializeField] private Vector3 cuisineMax;
    [SerializeField] private Vector3 magasinMin;
    [SerializeField] private Vector3 magasinMax;
    [SerializeField] private Vector3 sousSolMin;
    [SerializeField] private Vector3 sousSolMax;

    [Header("Probas de dèplacement en pi�ces")]
    [SerializeField] private float probaCuisine;
    [SerializeField] private float probaMagasin;
    [SerializeField] private float probaSousSol;

    [Header("UI")]
    [SerializeField] private Image babyImage;
    [SerializeField] private float babyBarMinX = -385;

    [Header("Animation")]
    [SerializeField] private Animator babyAnimator;
    [SerializeField] private bool menuBaby;

    [Header("Sounds")]
    [SerializeField] private AudioSource babyCry0;
    [SerializeField] private AudioSource babyCry1;
    [SerializeField] private AudioSource babyCry2;
    [SerializeField] private AudioSource gameMusic0;
    [SerializeField] private AudioSource gameMusic1;
    [SerializeField] private AudioSource gameMusic2;
    [SerializeField] private AudioSource rire1;
    [SerializeField] private AudioSource rire2;

    [Header("Scene objects")]
    [SerializeField] private LayerMask bebeMask;
    [SerializeField] private Outline outline;

    [Header("Larmes")]
    [SerializeField] private ParticleSystem particlePleure0;
    [SerializeField] private ParticleSystem particlePleure1;
    [SerializeField] private ParticleSystem particlePleure2;
    [SerializeField] private ParticleSystem particlePleure3;

    private NavMeshAgent agent;
    private GameManager gameManager;

    private void Awake()
    {
        
        total_frustration = 1f;

        gameManager = GameManager.Instance;
        gameManager.isBabyAngry = false;
        babyAnimator = transform.Find("Model").GetComponent<Animator>();
        outline = GetComponent<Outline>();
        outline.enabled = false;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = movingSpeed;

        if (menuBaby) { return; }

        // Rooms delimitations;

        cuisineMin = RoomDelimitations.transform.Find("CuisineMin").position;
        cuisineMax = RoomDelimitations.transform.Find("CuisineMax").position;
        magasinMin = RoomDelimitations.transform.Find("MagasinMin").position;
        magasinMax = RoomDelimitations.transform.Find("MagasinMax").position;
        sousSolMin = RoomDelimitations.transform.Find("SousSolMin").position;
        sousSolMax = RoomDelimitations.transform.Find("SousSolMax").position;

        

        timeBeforeNextAction = 5f;
        StartCoroutine(IAactionCountdown());
        currentActivityLevel = 0;

        probaCuisine = 1f;
        probaMagasin = 0f;
        probaSousSol = 0f;

    }

    void Update()
    {
        // Animator
        BabyAnimator();

        // Frustration
        frustration_factor = frustration_factor_formula();

        // Activity level
        ChangeActivityLevel();

        // Change time before next action
        ChangeTimeBetweenNextAction();

        // Change baby speed over time
        ChangeSpeedOverTime();

        // 

        babyImage.transform.localPosition = Vector3.Lerp(new Vector3(-babyBarMinX/2, 0, 0), new Vector3(babyBarMinX/2, 0, 0), frustration_factor / 0.8f);


        if (frustration_factor >= 0.8 && !gameManager.hasDayEnded)
        {
            gameManager.isBabyAngry = true;
            gameManager.EndDay();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            presentationMode = !presentationMode;

            if (presentationMode)
            {
                cryStat0 = 1f;
                cryStat1 = 1f;
                cryStat2 = 1f;
                cryStat3 = 1f;

                gameManager.minutesPerSecond = gameSpeed;
            } else
            {
                cryStat0 = .2f;
                cryStat1 = 1/3;
                cryStat2 = .5f;
                cryStat3 = .5f;

                gameManager.minutesPerSecond = 1;
            }

        }
    }

    private void BabyAnimator()
    {
        if (agent.velocity.magnitude > 0)
        {
            babyAnimator.SetBool("isWalking", true);
            babyAnimator.speed = 2 * agent.velocity.magnitude;
        }
        else
        {
            babyAnimator.SetBool("isWalking", false);
        }
    }
    private float frustration_factor_formula()
    {
        return 2 * (total_frustration / (1 + total_frustration) - 0.5f);
    }
    private void ChangeActivityLevel()
    {
        if (gameManager.timeOfDay == activityLevel_1) { currentActivityLevel = 1; probaCuisine = 0.5f; probaMagasin = 0.5f; }
        if (gameManager.timeOfDay == activityLevel_2) { currentActivityLevel = 2; probaCuisine = 0.2f; probaMagasin = 0.5f; probaSousSol = 0.3f; }
        if (gameManager.timeOfDay == activityLevel_3) { currentActivityLevel = 3; probaCuisine = 0.1f; probaMagasin = 0.4f; probaSousSol = 0.5f; }

        if (currentActivityLevel == 0)
        {
            IABaby_Activity_0();
        }
        if (currentActivityLevel == 1)
        {
            IABaby_Activity_1();
        }
        if (currentActivityLevel == 2)
        {
            IABaby_Activity_2();
        }
        if (currentActivityLevel == 3)
        {
            IABaby_Activity_3();
        }
    }
    private void ChangeTimeBetweenNextAction()
    {
        timeBetweenEachAction = 5 + (1 - frustration_factor) * 15f;
    }
    private void ChangeSpeedOverTime()
    {
        agent.speed = (1 + 2 * frustration_factor) * movingSpeed;
    }

    private IEnumerator CryBaby()
    {
        isCrying = true;

        gameMusic0.Stop();

        if (currentActivityLevel == 0)
        {
            babyCry0.Play();
            particlePleure0.gameObject.SetActive(true);
            particlePleure0.Play();
        }
        if (currentActivityLevel == 1)
        {
            babyCry1.Play();
            particlePleure1.gameObject.SetActive(true);
            particlePleure1.Play();
        }
        if (currentActivityLevel == 2)
        {
            babyCry2.Play();
            particlePleure2.gameObject.SetActive(true);
            particlePleure2.Play();
        }
        if (currentActivityLevel == 3)
        {
            babyCry2.Play();
            particlePleure3.gameObject.SetActive(true);
            particlePleure3.Play();
        }

        yield return new WaitForSeconds(2f);

        if (currentActivityLevel == 0)
        {
            gameMusic1.Play();
        }
        if (currentActivityLevel == 1)
        {
            gameMusic1.Play();
        }
        if (currentActivityLevel == 2)
        {
            gameMusic2.Play();
        }
        if (currentActivityLevel == 3)
        {
            gameMusic2.Play();
        }

        while (isCrying && BabySkillCheck.lastSkillCheckSuccess == false)
        {

            Debug.DrawLine(gameManager.mainCamera.transform.position, gameManager.mainCamera.transform.position + 1f * gameManager.mainCamera.transform.forward);

            if (Physics.Raycast(gameManager.mainCamera.transform.position,gameManager.mainCamera.transform.forward,2f, bebeMask))
            {
                outline.enabled = true;

                if (Input.GetKeyDown(KeyCode.E) && BabySkillCheck.lastSkillCheckSuccess == false)
                {
                    BabySkillCheck.StartBabySkillCheck();
                    Debug.Log("Starting baby skillcheck");
                }
            } else
            {
                outline.enabled = false;
            }
            yield return null;
        }
        particlePleure0.gameObject.SetActive(false);
        particlePleure0.Stop();
        particlePleure1.gameObject.SetActive(false);
        particlePleure1.Stop();
        particlePleure2.gameObject.SetActive(false);
        particlePleure2.Stop();
        particlePleure3.gameObject.SetActive(false);
        particlePleure3.Stop();

        MultiplyFrustration((0.75f - 0.25f * (gameManager.playerHumor - 1)));

        BabySkillCheck.lastSkillCheckSuccess = false;
        isCrying = false;
        outline.enabled = false;
        babyCry0.Stop();
        babyCry1.Stop();
        babyCry2.Stop();
        gameMusic1.Stop();
        gameMusic2.Stop();
        gameMusic0.Play();
        if (Random.Range(0,2) == 0)
        {
            rire1.Play();
        } else
        {
            rire2.Play();
        }

        yield return null;
    }


    private void IABaby_Activity_0()
    {

        if (timeBeforeNextAction <= 0f)
        {
            if (Random.Range(0f, 1f) < frustration_factor)
            {
                Vector3 randomPos = RandomPositionInArea(cuisineMin, cuisineMax);
                agent.SetDestination(randomPos);
                Debug.Log("Activit� 0 : Je me d�place dans la cuisine en " + randomPos);
            }

            if (!isCrying && Random.Range(0f, 1f) <= cryStat0)
            {
                StartCoroutine(CryBaby());
            }

            timeBeforeNextAction = timeBetweenEachAction;
            StartCoroutine(IAactionCountdown());
        }

    }
    private void IABaby_Activity_1()
    {

        if (timeBeforeNextAction <= 0f)
        {
            if (Random.Range(0f, 1f) < frustration_factor)
            {
                if (Random.Range(0f, 1f) <= probaCuisine)
                {
                    Vector3 randomPos = RandomPositionInArea(cuisineMin, cuisineMax);
                    agent.SetDestination(randomPos);
                    Debug.Log("Activit� 1 : Je me d�place dans la cuisine en " + randomPos);
                } else
                {
                    Vector3 randomPos = RandomPositionInArea(magasinMin, magasinMax);
                    agent.SetDestination(randomPos);
                    Debug.Log("Activit� 1 : Je me d�place dans le magasin en " + randomPos);
                }
            }

            if (!isCrying && Random.Range(0f, 1f) < cryStat1)
            {
                StartCoroutine(CryBaby());
            }

            timeBeforeNextAction = timeBetweenEachAction;
            StartCoroutine(IAactionCountdown());
        }

    }
    private void IABaby_Activity_2()
    {

        if (timeBeforeNextAction <= 0f)
        {
            if (Random.Range(0f, 1f) < frustration_factor)
            {
                float randomNum = Random.Range(0f, 1f);

                if (randomNum <= probaCuisine)
                {
                    Vector3 randomPos = RandomPositionInArea(cuisineMin, cuisineMax);
                    agent.SetDestination(randomPos);
                    Debug.Log("Activit� 2 : Je me d�place dans la cuisine en " + randomPos);
                }
                else if (randomNum > probaCuisine && randomNum < probaCuisine + probaMagasin)
                {
                    Vector3 randomPos = RandomPositionInArea(magasinMin, magasinMax);
                    agent.SetDestination(randomPos);
                    Debug.Log("Activit� 2 : Je me d�place dans le magasin en " + randomPos);
                } else
                {
                    Vector3 randomPos = RandomPositionInArea(sousSolMin, sousSolMax);
                    agent.SetDestination(randomPos);
                    Debug.Log("Activit� 2 : Je me d�place dans le sous-sol en " + randomPos);
                }
            }
            if (!isCrying && Random.Range(0f, 1f) < cryStat2)
            {
                StartCoroutine(CryBaby());
            }

            timeBeforeNextAction = timeBetweenEachAction;
            StartCoroutine(IAactionCountdown());
        }

    }
    private void IABaby_Activity_3()
    {

        if (timeBeforeNextAction <= 0f)
        {
            if (Random.Range(0f, 1f) < frustration_factor)
            {
                float randomNum = Random.Range(0f, 1f);

                if (randomNum <= probaCuisine)
                {
                    Vector3 randomPos = RandomPositionInArea(cuisineMin, cuisineMax);
                    agent.SetDestination(randomPos);
                    Debug.Log("Activit� 3 : Je me d�place dans la cuisine en " + randomPos);
                }
                else if (randomNum > probaCuisine && randomNum < probaCuisine + probaMagasin)
                {
                    Vector3 randomPos = RandomPositionInArea(magasinMin, magasinMax);
                    agent.SetDestination(randomPos);
                    Debug.Log("Activit� 3 : Je me d�place dans le magasin en " + randomPos);
                }
                else
                {
                    Vector3 randomPos = RandomPositionInArea(sousSolMin, sousSolMax);
                    agent.SetDestination(randomPos);
                    Debug.Log("Activit� 3 : Je me d�place dans le sous-sol en " + randomPos);
                }
            }

            if (!isCrying && Random.Range(0f, 1f) < cryStat3)
            {
                StartCoroutine(CryBaby());
            }

            timeBeforeNextAction = timeBetweenEachAction;
            StartCoroutine(IAactionCountdown());
        }

    }

    public void AddFrustration(float frustration)
    {
        total_frustration += frustration;
    }
    public void MultiplyFrustration(float frustrationFactor)
    {
        total_frustration *= frustrationFactor;
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
    public IEnumerator TurnTo(Vector3 targetPosition)
    {
        // Calculate the rotation needed to face the target position
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

        // Time elapsed
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            // Interpolate between start and target rotations
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / 1f);

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final rotation is the target rotation
        transform.rotation = targetRotation;
    }

}
