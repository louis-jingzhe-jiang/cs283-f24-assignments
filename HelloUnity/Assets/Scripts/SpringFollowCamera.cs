using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringFollowCamera : MonoBehaviour
{
    public Transform target; // following target

    public float hDist; // horizontal follow distance
    
    public float vDist; // vertical follow distance

    public float springConst; // spring constant

    public float dampConst; // damping constant

    //public float angSpringConst; // angular spring constant

    //public float angDampConst;// angular damping constant

    private Vector3 _velocity; // current camera linear velocity

    //private Vector3 _angVelocity; // angular rotation per time frame

    // Start is called before the first frame update
    void Start()
    {   // same as the Rigid Follow Camera startup, move the camera to the 
        // desired location behind the model
        transform.position = target.position - 
            target.forward * hDist + target.up * vDist;
        // the new Vector3(0, 30 / hDist, 0) is used as a suppliment to make 
        // the camera focus on the head of the character while following the
        // center pivot of the whole character
        transform.Rotate(Quaternion.LookRotation(target.position - 
            transform.position).eulerAngles + new Vector3(0, 30 / hDist, 0));
        // initialize velocity to 0
        _velocity = new Vector3(0, 0, 0);
        //_angVelocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // desired values
        Vector3 desiredPos = target.position - 
            target.forward * hDist + target.up * vDist;
        Vector3 desiredRot = (Quaternion.LookRotation(
            target.position - desiredPos) * 
            new Quaternion(Mathf.Sin(- Mathf.PI / 180 * 20 / hDist), 
                0, 0, Mathf.Cos(- Mathf.PI / 180 * 20 / hDist)))
            .eulerAngles;
        // calculate linear acceleration
        Vector3 displacement = transform.position - desiredPos;
        Vector3 springAcc = (-springConst * displacement) - 
            dampConst * _velocity;
        // calculate angular acceleration
        //Vector3 angDisplacement = transform.rotation.eulerAngles - desiredRot;
        //Debug.Log(angDisplacement);
        //Vector3 angAcc = (-angSpringConst * angDisplacement) - angDampConst * _angVelocity;
        // update linear velocity and position
        _velocity = springAcc * Time.deltaTime;
        transform.position += _velocity * Time.deltaTime;
        // update angular velocity and position
        //_angVelocity = angAcc * Time.deltaTime;
        //transform.Rotate(_angVelocity * Time.deltaTime);

        //transform.rotation = Quaternion.LookRotation(
        //    target.position - desiredPos) * 
        //    new Quaternion(Mathf.Sin(- Mathf.PI / 180 * 20 / hDist), 
        //        0, 0, Mathf.Cos(- Mathf.PI / 180 * 20 / hDist));

        transform.rotation = Quaternion.LookRotation(
            target.position - transform.position) *
            new Quaternion(Mathf.Sin(-Mathf.PI / 180 * 20 / hDist),
                0, 0, Mathf.Cos(-Mathf.PI / 180 * 20 / hDist));
    }
}//transform.position
