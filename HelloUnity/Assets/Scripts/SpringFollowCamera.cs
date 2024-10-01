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

    // Start is called before the first frame update
    void Start()
    {   // same as the Rigid Follow Camera startup, move the camera to the 
        // desired location behind the model
        transform.position = target.position - 
            target.forward * hDist + target.up * vDist;
        transform.rotation = Quaternion.LookRotation(target.position - 
            transform.position + new Vector3(0, hDist/3, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
