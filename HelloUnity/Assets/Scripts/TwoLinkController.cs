using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoLinkController : MonoBehaviour
{
    public Transform target;

    public Transform end;

    /**
     * The distance between the end point and the 1st joint
     */
    private float _lowerArmLength;
    /**
     * The distance between the the 1st joint and the 2nd joint
     */
    private float _upperArmLength;
    /**
     * The distance between the target and the 2nd joint
     */
    private float _targetToShoulder;
    /** 
     * The initial quaternion rotation of the elbow before the game starts
     */
    private Quaternion _elbowInitialRotation;
    /** 
     * The initial quaternion rotation of the shoulder before the game starts
     */
    private Quaternion _shoulderInitialRotation;
    /** 
     * The initial position of the shoulder before the game starts
     */
    private Vector3 _shoulderInitialPos;

    // Start is called before the first frame update
    void Start()
    {
        // these are the constants for a specific skeleton
        Vector3 endToElbow = end.position - end.parent.position;
        _lowerArmLength = endToElbow.magnitude;
        Vector3 elbowToShoulder = end.parent.position - 
            end.parent.parent.position;
        _upperArmLength = elbowToShoulder.magnitude;
        _elbowInitialRotation = end.parent.rotation;
        _shoulderInitialRotation = end.parent.parent.rotation;
        _shoulderInitialPos = end.parent.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        // calculate the distance from the shoulder to the end
        Vector3 targetToShoulderV = target.position - 
            end.parent.parent.position;
        _targetToShoulder = targetToShoulderV.magnitude;
        Debug.Log("shoulder-target: " + _targetToShoulder);
        Debug.Log("sum of bones: " + (_upperArmLength + _lowerArmLength));
        // solve for the elbow rotation that makes the distance between the 
        // shoulder and the end match _targetToEnd
        float elbowAng;
        if (_upperArmLength + _lowerArmLength <= _targetToShoulder) 
        {   // beyond reach, set elbow to 180 degrees
            elbowAng = Mathf.PI;
        }
        else 
        {   // within reach
            float cosElbow = (Mathf.Pow(_lowerArmLength, 2) + 
                Mathf.Pow(_upperArmLength, 2) - 
                Mathf.Pow(_targetToShoulder, 2)) /
                (2 * _upperArmLength * _lowerArmLength);
            elbowAng = Mathf.Acos(cosElbow);
            //Debug.Log(cosElbow);
        }
        
        
        // calculate the rotation for the shoulder
        Vector3 r = end.position - _shoulderInitialPos;
        Vector3 e = target.position - end.position;
        Vector3 cross = Vector3.Cross(r, e);
        float selfDot = Vector3.Dot(r, r);
        float dot = Vector3.Dot(r, e);
        float shoulderAng = Mathf.Atan2(cross.magnitude, selfDot + dot);
        Vector3 axis = cross / cross.magnitude;
        // now apply the shoulder rotation
        end.parent.parent.rotation = _shoulderInitialRotation * 
            Quaternion.AngleAxis(shoulderAng * Mathf.Rad2Deg, axis);
        // now apply the elbow rotation
        // the elbow's rotation is only around the z axis
        Debug.Log(end.parent.rotation);
        // tested! worked!
        end.parent.rotation = _elbowInitialRotation * Quaternion.Euler(0, 0, 
            (Mathf.PI - elbowAng) * Mathf.Rad2Deg);
    }
}
