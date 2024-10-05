using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidFollowCamera : MonoBehaviour
{
    public Transform target; // following target

    public float hDist; // horizontal follow distance
    
    public float vDist; // vertical follow distance


    // Start is called before the first frame update
    void Start()
    {   // When dealing with position, we move the camera towards the back
        // of the model by hDist, and then move the camera towards the up
        // direction of the model by vDist
        transform.position = target.position - 
            target.forward * hDist + target.up * vDist;
        // When dealign with rotation, we want the camera to point at the 
        // displacement vector pointing from the camera's updated position
        // to the target's position
        // The vector (0, hDist/3, 0) is a suppliment vector used to make the
        // camera focus on the position slightly higher than the model's 
        // center, since the head is usually above the model's center
        transform.rotation = Quaternion.LookRotation(target.position - 
            transform.position + new Vector3(0, hDist/3, 0));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position - 
            target.forward * hDist + target.up * vDist;
        transform.rotation = Quaternion.LookRotation(target.position - 
            transform.position + new Vector3(0, hDist/3, 0));
    }
}
