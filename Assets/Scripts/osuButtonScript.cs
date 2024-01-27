using System.Collections;
using UnityEngine;

public class osuButtonScript : MonoBehaviour
{
    [SerializeField] public float fadeDuration = 1f; // Duration of the fade in seconds
    [SerializeField] public float growDuration = 0.25f; // Duration of the growth animation in seconds
    [SerializeField] public Vector3 targetScale = new Vector3(1f, 1f, 1f); // Target scale for the button
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private bool fading = false;
    private bool growing = true;

    private CelianPetrissageScript instantiatingScript;

    // Method to receive the reference to the instantiating script
    public void SetInstantiatingScript(CelianPetrissageScript script)
    {
        instantiatingScript = script;
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        // Set initial scale to zero
        rectTransform.localScale = Vector3.zero;

        // Start growing animation
        StartCoroutine(GrowAnimation());
    }

    IEnumerator GrowAnimation()
    {
        float timer = 0f;

        while (timer < growDuration)
        {
            timer += Time.deltaTime;
            float scaleProgress = timer / growDuration;
            rectTransform.localScale = Vector3.Lerp(Vector3.zero, targetScale, scaleProgress);
            yield return null;
        }

        // Ensure the scale is set to the target scale exactly
        rectTransform.localScale = targetScale;
    }

    void Update()
    {
        if (fading)
        {
            float newAlpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, Time.deltaTime / fadeDuration);
            canvasGroup.alpha = newAlpha;

            if (canvasGroup.alpha <= 0)
            {
                gameObject.SetActive(false);
                Destroy(gameObject, 0f);
                instantiatingScript.decrHowManyOsu();
            }
        }
    }

    public void OnButtonPressed()
    {
        fading = true;
    }
}
