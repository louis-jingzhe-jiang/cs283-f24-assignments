using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flythrough : MonoBehaviour
{

    //public Vector3 velocity;
    public float velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // rotate horizontal mouse movement around world coordinate axis
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0, Space.World);
        // rotate vertical mouse movement around camera's local coordinate axis
        transform.Rotate(- Input.GetAxis("Mouse Y"), 0, 0);
        if (Input.GetKey("w"))
        {
            transform.position += velocity * Time.deltaTime * 
                transform.forward;
        }
        if (Input.GetKey("s"))
        {
            transform.position -= velocity * Time.deltaTime * 
                transform.forward;
        }
        if (Input.GetKey("a"))
        {   // transform.up is the upward point axis in local coordinate
            transform.position += velocity * Time.deltaTime * 
                (Quaternion.AngleAxis(-90, transform.up) * transform.forward);
        }
        if (Input.GetKey("d"))
        {   // rotate transform.up by 90 or -90 degrees around forward  
            // pointing axis results left or right
            transform.position += velocity * Time.deltaTime * 
                (Quaternion.AngleAxis(90, transform.up) * transform.forward);
        }

    }
}
