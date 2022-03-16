using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Collectable interestPrefab;

    public Collectable dangerPrefab;

    [SerializeField] int MAX_NUMBER_INTEREST = 10;
    [SerializeField] int MAX_NUMBER_DANGER = 5;
    [SerializeField] float respawnTime;
    [SerializeField] float distanceThreshold;

    public List<Collectable> interestObjects;
    public List<Collectable> dangerObjects;

    [SerializeField] private GameObject interestContainer;

    [SerializeField] private GameObject dangerContainer;

    [SerializeField] private SpawnArea spawnArea;
    
    // Start is called before the first frame update
    void Start()
    {
        OnValidate();
        StartCoroutine(DeployObjects());
    }

    private void OnValidate()
    {
        interestObjects = new List<Collectable>();
        foreach (Transform t in interestContainer.transform)
        {
            var item = t.GetComponent<Collectable>();
            if (item != null)
                interestObjects.Add(item);
        }
        
        dangerObjects = new List<Collectable>();
        foreach (Transform t in dangerContainer.transform)
        {
            var item = t.GetComponent<Collectable>();
            if (item != null)
                dangerObjects.Add(item);
        }
    }

    //Kamera grenzbereich

    void SpawnObjects()
    {
        if(interestObjects.Count < MAX_NUMBER_INTEREST)
        {
            SpawnObject(interestPrefab);
            GameObject o = Instantiate(interestObject) as GameObject;
            o.transform.position = new Vector3(Random.Range(-5,5), Random.Range(-5,5), 0);
        }
        
        if (dangerObjects.Count < MAX_NUMBER_DANGER)
        {
            SpawnObject(dangerPrefab);

            GameObject o = Instantiate(dangerObject) as GameObject;
            o.transform.position = new Vector3(Random.Range(-7, 7), Random.Range(-5, 5), 0);
        }
    }

    void SpawnObject(GameObject objectToSpawn)
    {
        Vector3 point = spawnArea.GetRandomPoint();
        
        // check if the point is too close to other objects
        bool spawnablePosition = false;
        int attempts = 0;
        while (!spawnablePosition)
        {
            attempts++;
            if (attempts > 5)
            {
                break;
            }
            
            float minDistance;
            if (interestObjects.Count > 0)
            {
                minDistance = Vector3.Distance(point, interestObjects[0].transform.position);
            }
            else if (dangerObjects.Count > 0)
            {
                minDistance = Vector3.Distance(point, dangerObjects[0].transform.position);
            }
            else break;

            float distance;
            foreach (Collectable item in interestObjects)
            {
                distance = Vector3.Distance(point, item.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                }
            }

            spawnablePosition = minDistance >= distanceThreshold;
        }

        if (spawnablePosition)
        {
            
            
        }
        
    }

    IEnumerator DeployObjects()
    {
        while(true)
        {
            yield return new WaitForSeconds(respawnTime);
            SpawnObjects();
        }
    }
    
}
