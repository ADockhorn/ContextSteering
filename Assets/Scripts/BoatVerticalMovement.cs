using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatVerticalMovement : MonoBehaviour
{
    [SerializeField] private AnimationCurve upDownMovement;

    [SerializeField] private float speedOfMovement;
    [SerializeField] private float offsetMultiplier;
    
    private float currentTime;

    private float initialHeight;
    
    
    // Start is called before the first frame update
    void Start()
    {
        initialHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        currentTime %= speedOfMovement;
        var heightOffset = upDownMovement.Evaluate(currentTime / speedOfMovement);
        transform.position = new Vector3(transform.position.x,
            initialHeight + heightOffset*offsetMultiplier,
            transform.position.z);
    }
}
