using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelianMelangeScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public Canvas canvas; // Reference to the Canvas

    private Vector2 canvasSize;

    private RectTransform canvasRectTransform;

    [SerializeField] public GameObject arrowPrefab;

    private GameObject instantiatedArrow;

    private float lastDegree = 0;

    private int amountOfTurnsToWin = 10;

    private int nextFraction = 180;
    private int fractionSize = 30;

    private bool won = false;

    private GameManager gM;
    // Start is called before the first frame update
    void Start()
    {
        gM = GameManager.Instance;
        canvasRectTransform = canvas.GetComponent<RectTransform>();

        canvasSize = canvasRectTransform.sizeDelta;
        instantiatedArrow = SpawnArrow();
        instantiatedArrow.SetActive(false);
    }

    
    void Update()
    {
        

    }

    public GameObject SpawnArrow(){
        Vector3 randomPosition = new Vector3(0f, 0f, 0f);
        
        //Debug.Log(randomPosition);

        // Instantiate the object inside the canvas
        GameObject instantiatedObject = Instantiate(arrowPrefab, randomPosition, Quaternion.identity, canvasRectTransform);
        instantiatedObject.transform.localPosition = randomPosition;
        
        return(instantiatedObject);
    }

    public IEnumerator StartGame()
    {
        instantiatedArrow.SetActive(true);
        amountOfTurnsToWin = 10;

        nextFraction = 180;
        fractionSize = 30;

        while (won == false)
        {
            // Get the position you want to convert to degrees
            Vector2 position = Input.mousePosition; // Example position, replace with your own

            position.x = position.x / Screen.width * canvasSize.x - canvasSize.x / 2;
            position.y = position.y / Screen.height * canvasSize.y - canvasSize.y / 2;

            // If you also want to know the direction (clockwise or counterclockwise), you can use the cross product
            // Vector3.Cross calculates the cross product between two vectors
            // If the resulting vector's z component is positive, the angle is counterclockwise; otherwise, it's clockwise

            // Convert angle to degrees
            float degrees = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;

            // Print the result to the console
            // Debug.Log("Angle in degrees: " + degrees + " position : " + position);

            if (Vector2.Distance(position, new Vector2(0f, 0f)) < canvasSize.x / 12)
            {
                degrees = lastDegree;
            }

            instantiatedArrow.transform.rotation = Quaternion.Euler(0f, 0f, degrees);

            lastDegree = degrees;

            if (degrees < nextFraction && degrees > nextFraction - fractionSize)
            {
                nextFraction -= fractionSize;
                if (nextFraction == 0)
                {
                    amountOfTurnsToWin--;
                    if (amountOfTurnsToWin == 0)
                    {
                        won = true;
                    }
                }
                if (nextFraction == -180)
                {
                    nextFraction = 180;
                }
            }
            // Debug.Log("nextFraction : " + nextFraction + " amountOfTurnsToWin : " + amountOfTurnsToWin);
            yield return null;
        }
        instantiatedArrow.SetActive(false);
        gM.Focus = true;
        yield break;
    }
}
