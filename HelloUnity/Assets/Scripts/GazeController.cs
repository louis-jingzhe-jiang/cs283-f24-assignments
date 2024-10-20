using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class GazeController : MonoBehaviour
{
    // It looks like the head of the character I chose has +z orientation 
    // pointing towards left instead of ahead. Its +x orientation points
    // downward and its +y orientation points ahead. Thus, I want to make 
    // +y direction pointing at my target while keeping +x direction pointing
    // downward.
    public Transform head;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        //_rotationSuppliment = Quaternion.Euler()
    }

    // Update is called once per frame
    void Update()
    {   
        // calculate the direction vector
        Vector3 direction = target.position - head.position;
        // calculate the current heading vector r (current looking object)
        Vector3 r = head.up; // the up direction actually points forward
        // calculate the displacement vector e representing the displacement
        // from the current looking object to the target
        Vector3 e = direction - r;
        // visualize the line
        Debug.DrawLine(head.position, target.position, Color.red);
        // calculating dot and cross products
        Vector3 cross = Vector3.Cross(r, e);
        if (cross.magnitude == 0) return;
        float dot = Vector3.Dot(r, e);
        float selfDot = Vector3.Dot(r, r);
        // calculating angle
        float angle = Mathf.Atan2(cross.magnitude, dot + selfDot);
        // calculating rotation axis
        Vector3 axis = cross / cross.magnitude;
        // Now, we should assign the rotation to the head
        head.parent.rotation *=  Quaternion.AngleAxis(
            angle * Mathf.Rad2Deg, axis);
    }
}
