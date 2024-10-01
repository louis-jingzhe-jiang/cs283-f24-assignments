using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float speed;

    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w")) // move forward
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey("s")) // move backward
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey("a")) // turn left
        {   // Quaternions multiply with each other to perform rotation
            float theta = Time.deltaTime * rotationSpeed * (-1);
            Quaternion rotation = new Quaternion(0, Mathf.Sin(theta / 2), 
                0, Mathf.Cos(theta / 2));
            transform.rotation *= rotation;
        }
        if (Input.GetKey("d")) // turn right
        {   // For example a object with rotation quaternion A, and performs
            // a rotation represented by Quaternion B. The result rotation
            // representation in Quaternion is A * B.
            float theta = Time.deltaTime * rotationSpeed;
            Quaternion rotation = new Quaternion(0, Mathf.Sin(theta / 2), 
                0, Mathf.Cos(theta / 2));
            transform.rotation *= rotation;
        }
    }
}
