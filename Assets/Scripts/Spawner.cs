using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject interestObject;

    public GameObject dangerObject;

    [SerializeField]
    int MAX_NUMBER_INTEREST;

    [SerializeField]
    int MAX_NUMBER_DANGER;

    [SerializeField]
    float respawnTime;

    int currentInterest;
    int currentDanger;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i += 1)
        {
            GameObject o = Instantiate(interestObject) as GameObject;
            o.transform.position = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        }

        respawnTime = 1f;
        StartCoroutine(DeployObjects());
    }

    // Update is called once per frame
    void Update()
    {
        currentInterest = GameObject.FindGameObjectsWithTag("Interest").Length;
        currentDanger = GameObject.FindGameObjectsWithTag("Danger").Length;
    }

    //Kamera grenzbereich

    void SpawnObjects()
    {
        if(currentInterest < MAX_NUMBER_INTEREST)
        {
            GameObject o = Instantiate(interestObject) as GameObject;
            o.transform.position = new Vector3(Random.Range(-5,5), Random.Range(-5,5), 0);
        }
        if (currentDanger < MAX_NUMBER_DANGER)
        {
            GameObject o = Instantiate(dangerObject) as GameObject;
            o.transform.position = new Vector3(Random.Range(-7, 7), Random.Range(-5, 5), 0);
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
