using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngineInternal;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 20.0f;

    [SerializeField] private float rotationSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var relativePos = new Vector3(horizontal, 0, vertical);
        var rotation = Quaternion.LookRotation(relativePos);
        transform.rotation =
            Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
