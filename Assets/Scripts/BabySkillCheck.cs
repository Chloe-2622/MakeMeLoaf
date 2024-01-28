using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabySkillCheck : MonoBehaviour
{
    [SerializeField] RectTransform playerCursor;
    [SerializeField] RectTransform babyCursor;

    private static BabySkillCheck instance;
    public static bool lastSkillCheckSuccess;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }

        gameObject.SetActive(false);
    }

    private void Start()
    {
        //DEBUG
        //StartBabySkillCheck();
    }

    public static void StartBabySkillCheck()
    {
        instance.gameObject.SetActive(true);

        instance.StartCoroutine(instance.BabySkillCheckCoroutine());
    }

    private IEnumerator BabySkillCheckCoroutine()
    {
        float playerCursorY = playerCursor.anchoredPosition.y;
        float babyCursorY = Random.Range(babyCursor.anchoredPosition.y - 180, babyCursor.anchoredPosition.y + 180);
        babyCursor.anchoredPosition = new Vector2(babyCursor.anchoredPosition.x, babyCursorY);
        float sign = -1;

        while (true)
        {
            playerCursorY += 500.0f * Time.deltaTime * sign;

            // Change direction if we reach the limits
            if (playerCursorY > 200 || playerCursorY < -200)
            {
                playerCursorY = Mathf.Clamp(playerCursorY, -200, 200);
                sign *= -1;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                lastSkillCheckSuccess = (Mathf.Abs(playerCursorY - babyCursorY) < 20);
                break;
            }

            playerCursor.anchoredPosition = new Vector2(playerCursor.anchoredPosition.x,playerCursorY);

            yield return null;
        }

        instance.gameObject.SetActive(false);

        yield break;
    }

}
