using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObjects : MonoBehaviour
{
    private Collider _collider;
    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Vector3.Magnitude(_rigidBody.velocity));
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered OnTrigger");
        if (other.tag == "Player")
        {
            Debug.Log("Hit Player");
            // apply a force on the pushable object
            Vector3 force = transform.position - other.transform.position;
            _rigidBody.AddForce(force);
        }
        else if (other.tag == "Monster")
        {
            // if the velocity of the sphere is fast, monster will die
            if (Vector3.Magnitude(_rigidBody.velocity) > 2f)
            {
                Animator animator = other.GetComponent<Animator>();
                animator.SetBool("die", true);
            }
        }
    }
}
