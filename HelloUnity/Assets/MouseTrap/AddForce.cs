using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyForce(gameObject.GetComponent<Rigidbody>());
        }
    }

    void ApplyForce(Rigidbody body)
    {
        Vector3 direction = transform.forward;
        body.AddForceAtPosition(direction * 3, transform.position);
    }
}
