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
        // rotate horizontal mouse movement around world coordinate axis
        // transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0, 
        //    Space.World);

        if (Input.GetKey("w")) // move forward
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey("s")) // move backward
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey("a")) // move left
        {   // Quaternions multiply with each other to perform rotation
            // For example a object with rotation quaternion A, and performs
            // a rotation represented by Quaternion B. The result rotation
            // representation in Quaternion is A * B.
            float theta = Time.deltaTime * rotationSpeed * (-1);
            Quaternion rotation = new Quaternion(0, Mathf.Sin(theta / 2), 
                0, Mathf.Cos(theta / 2));
            transform.rotation *= rotation;
        }
        if (Input.GetKey("d")) // move right
        {   
            float theta = Time.deltaTime * rotationSpeed;
            Quaternion rotation = new Quaternion(0, Mathf.Sin(theta / 2), 
                0, Mathf.Cos(theta / 2));
            transform.rotation *= rotation;
        }
    }
}
