using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CelianPetrissageScript : MonoBehaviour
{

    [SerializeField] public GameObject prefabToSpawn;
    static private GameObject prefab;
    [SerializeField] public Canvas canvas;

    [SerializeField] public GameObject osu;

    static private int howManyOsu = 0;

    static private int howManyWaves = 5;
    static private int howManyPerWave = 5;

    static private List<Vector3> spawnList;

    static private Vector2 osuSize;

    static private Vector2 canvasSize;

    static private RectTransform canvasRectTransform;
    private RectTransform osuRectTransform;


    private GameManager gM;
    // Start is called before the first frame update
    void Start()
    {
        gM = GameManager.Instance;
        canvasRectTransform = canvas.GetComponent<RectTransform>();

        osuRectTransform = osu.GetComponent<RectTransform>();
        /*
        // Calculate a random position within the canvas bounds
        float randomX = Random.Range(-960f, 960f);
        float randomY = Random.Range(-540f, 540f);
        */
        prefab = prefabToSpawn;
        //Debug.Log(prefab);
        canvasSize = canvasRectTransform.sizeDelta;

        osuSize = osuRectTransform.sizeDelta;
    }

    void Update()
    {
        /*
        if (howManyOsu == 0){
            spawnList = new List<Vector3>();
            if (howManyWaves != 0){
                howManyWaves--;
                for (int i = 0; i < howManyPerWave; i++){
                    SpawnPrefab();
                }
            }
        }*/
    }

    [ContextMenu("start game")]
    public IEnumerator StartGame()
    {
        howManyWaves = 5;
        howManyPerWave = 5;
        spawnList = new List<Vector3>();
        for (int i = 0; i < howManyPerWave; i++)
        {
            SpawnPrefab();
        }

        while (howManyWaves > 0)
        {
            if (howManyOsu == 0)
            {
                spawnList = new List<Vector3>();
                howManyWaves--;
                for (int i = 0; i < howManyPerWave; i++)
                {
                    SpawnPrefab();
                }
                
            }
            yield return null;
        }
        while (howManyOsu > 0) yield return null;
        gM.Focus = true;
    }

    static void SpawnPrefab(){
        howManyOsu++;
        


    // Calculate the half size of the canvas
        float halfCanvasWidth = canvasSize.x / 2f;
        float halfCanvasHeight = canvasSize.y / 2f;

        Vector3 randomPosition = new Vector3(0f, 0f, 0f);
        do {
            // Calculate random position within canvas bounds
            float randomX = Random.Range(-halfCanvasWidth + osuSize.x/2 , halfCanvasWidth - osuSize.x/2);
            
            float randomY = Random.Range(-halfCanvasHeight + osuSize.y/2, halfCanvasHeight - osuSize.y/2);
            randomPosition = new Vector3(randomX, randomY, 0f);
            //Debug.Log(randomPosition);
        } while (isTooClose(randomPosition));

        //Debug.Log(randomPosition);

        // Instantiate the object inside the canvas
        GameObject instantiatedObject = Instantiate(prefab, randomPosition, Quaternion.identity, canvasRectTransform);
        instantiatedObject.transform.localPosition = randomPosition;
        spawnList.Add(randomPosition);
    }

    

    static public void decrHowManyOsu(){
        howManyOsu--;
    }

    static public bool isTooClose(Vector3 randomPosition){
        for (int i = 0; i < spawnList.Count; i++){
            Debug.Log(i);
            if (Vector3.Distance(spawnList[i], randomPosition) < osuSize.x){
                return(true);
            }
        }
        return(false);
    }


    
}
