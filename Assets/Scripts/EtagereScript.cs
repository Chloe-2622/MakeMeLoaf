using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtagereScript : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    [SerializeField] private float itemSpawnHeight = 0.0f;
    [SerializeField] private int itemSpawnCount = 10;

    [SerializeField] private float maxHeight = 2.0f;
    [SerializeField] private float minHeight = 0.0f;
    [SerializeField] private float maxSide = 1.0f;

    [SerializeField] private bool otherOrientation = false;

    private bool reseted = false;

    private GameObject[] items;

    private void Awake()
    {
        items = new GameObject[itemSpawnCount];
    }

    private IEnumerator ResetEtagere()
    {
        for(int i = 0; i < itemSpawnCount; i++)
        {
            if (items[i] != null && items[i].transform.position.y < 0)
            {
                Selectable ing = items[i].GetComponent<Selectable>();
                if (ing != null && ing != Select.selected)
                {
                    Destroy(items[i]);
                }
            }
            yield return null;
        }

        for (int i = 0; i < itemSpawnCount; i++)
        {
            Vector3 spawnPosition = new Vector3(otherOrientation ? 0 : Random.Range(-maxSide, maxSide), Random.Range(minHeight, maxHeight), otherOrientation ? Random.Range(-maxSide, maxSide) : 0);
            GameObject item = Instantiate(itemPrefab, transform.position + spawnPosition, Quaternion.identity);
            item.transform.parent = transform;
            items[i] = item;
            yield return null;
        }

        yield break;
    }

    private void Update()
    {
        if (Camera.main.gameObject.transform.position.y > itemSpawnHeight)
        {
            if (!reseted)
            {
                StartCoroutine(ResetEtagere());
                reseted = true;
            }
        }
        else
        {
            reseted = false;
        }
    }
}
