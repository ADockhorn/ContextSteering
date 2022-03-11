using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsBehavior : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    float directionChange;

    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += speed * direction * Time.deltaTime;
    }

    //Camera Boundaries

    void ChangeDirection()
    {
        direction = new Vector3(Random.Range(-1f,1f), Random.Range(-1f, 1f), 0);
    }

    IEnumerator Delay()
    {
        while (true)
        {
            yield return new WaitForSeconds(directionChange);
            ChangeDirection();
        }
    }
}
