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
    /**
     * The look direction
     */
    //private Vector3 _direction;
    /**
     * vector from head to target CROSS forward direction of head
     */
    //private Vector3 _crossProd;
    /**
     * vector from head to target DOT forward direction of head
     */
    //private float _dotProd;
    /**
     * vector from head to target DOT itself
     */
    //private float _selfDotProd;
    /**
     * rotation angle
     */
    //private float _angle;
    /**
     * rotation axis
     */
    //private Vector3 _axis;
    //private Quaternion _rotationSuppliment;

    // Start is called before the first frame update
    void Start()
    {
        //_rotationSuppliment = Quaternion.Euler()
    }

    // Update is called once per frame
    void Update()
    {   
        // calculate the direction vector
        Vector3 _direction = target.position - head.position;
        // visualize the line
        Debug.DrawLine(head.position, target.position, Color.red);
        // calculating dot and cross products
        // the head's up direction is actually pointing forward, thus we use
        // head.up all the time in the calculation of dot and cross products
        Vector3 _crossProd = Vector3.Cross(_direction, head.forward);
        float _dotProd = Vector3.Dot(_direction, head.forward);
        float _selfDotProd = Vector3.Dot(_direction, _direction);
        // calculating angle
        float _angle = Mathf.Atan2(_crossProd.magnitude, _dotProd + _selfDotProd);
        // calculating rotation axis
        Vector3 _axis = _crossProd / _crossProd.magnitude;
        // Now, we should assign the rotation to the head
        head.rotation = Quaternion.AngleAxis(_angle, _axis) * Quaternion.Inverse(Quaternion.Euler(90f, 180f, -90));
    }
}
