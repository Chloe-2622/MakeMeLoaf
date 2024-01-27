using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClientAgentScript : MonoBehaviour
{
    [HideInInspector] public bool isWaiting = true;

    [SerializeField] public Vector3 spawnPosition;
    [SerializeField] public Vector3 intermediatePos1;
    [SerializeField] public Vector3 intermediatePos2;
    [SerializeField] public Rect possibleWaitPosition;

    [SerializeField] public float speed = 1.5f;

    //DEBUG
    public void Start()
    {
        transform.position = spawnPosition;

        for(int i = 0; i < 5; i++)
        {
            transform.GetChild(i).GetComponent<SkinnedMeshRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0, 1.0f));
        }
        

        //Call clientprocess and after 30 seconds, set isWaiting to false
        StartCoroutine(ClientProcess());
    }
    

    public IEnumerator ClientProcess()
    {
        isWaiting = true;

        //The client comes into the restaurant from outside, it starts from the spawn position, and goes to a random position inside the possible wait area (X and Z aligned) and play walking animation
        //You go from spawn position to intermediate position 1 and then to intermediate position 2 and then to the random position, don't use navmesh
        Vector3[] path = new Vector3[4];
        path[0] = spawnPosition;
        path[1] = intermediatePos1;
        path[2] = intermediatePos2;
        path[3] = new Vector3(Random.Range(possibleWaitPosition.xMin, possibleWaitPosition.xMax), spawnPosition.y, Random.Range(possibleWaitPosition.yMin, possibleWaitPosition.yMax));
        GetComponent<Animator>().SetFloat("walkingFactor", 1.0f);
        //Smooth interpolation between each keyframe
        for (int i = 0; i < path.Length - 1; i++)
        {
            float t = 0;
            while (t < 1)
            {
                t +=  Time.deltaTime * speed / Vector3.Distance(path[i], path[i+1]);
                transform.position = Vector3.Lerp(path[i], path[i + 1], t);

                //Look at the next position
                transform.LookAt(path[i + 1]);

                yield return null;
            }

            if(i == path.Length - 2)
            {
                break;
            }

            //Smooth turn
            Quaternion startRotation = transform.rotation;
            float t2 = 0;
            while (t2 < 1)
            {
                t2 += Time.deltaTime * 1.0f;
                transform.rotation = Quaternion.Lerp(startRotation, Quaternion.LookRotation(path[i + 2] - transform.position), t2);

                yield return null;
            }
             
        }


        
        
        /*agent.SetDestination(new Vector3(Random.Range(possibleWaitPosition.xMin, possibleWaitPosition.xMax), spawnPosition.y, Random.Range(possibleWaitPosition.yMin, possibleWaitPosition.yMax)));
        GetComponent<Animator>().SetBool("Walking", true);
        yield return new WaitUntil(() => agent.remainingDistance < 0.1f);*/

        //The client stops walking and wait
        GetComponent<Animator>().SetFloat("walkingFactor", 0.0f);
        yield return new WaitUntil(() => !isWaiting);

        //The client goes to the exit and play walking animation
        /*agent.SetDestination(spawnPosition);
        GetComponent<Animator>().SetBool("Walking", true);
        yield return new WaitUntil(() => agent.remainingDistance < 0.1f);*/
        //You go from the random position to intermediate position 2 and then to intermediate position 1 and then to the spawn position, don't use navmesh
        path[0] = path[3];
        path[1] = intermediatePos2;
        path[2] = intermediatePos1;
        path[3] = spawnPosition;
        GetComponent<Animator>().SetFloat("walkingFactor", 1.0f);
        //Smooth interpolation between each keyframe
        for (int i = 0; i < path.Length - 1; i++)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * speed / Vector3.Distance(path[i], path[i + 1]); ;
                transform.position = Vector3.Lerp(path[i], path[i + 1], t);

                //Look at the next position
                transform.LookAt(path[i + 1]);

                yield return null;
            }


            if (i == path.Length - 2)
            {
                break;
            }


            //Smooth turn
            Quaternion startRotation = transform.rotation;
            float t2 = 0;
            while (t2 < 1)
            {
                t2 += Time.deltaTime * 1.0f;
                transform.rotation = Quaternion.Lerp(startRotation, Quaternion.LookRotation(path[i + 2] - transform.position), t2);

                yield return null;
            }
        }


        //The client stops walking and destroy itself
        GetComponent<Animator>().SetFloat("walkingFactor", 0.0f);
        Destroy(gameObject);
    }
}
