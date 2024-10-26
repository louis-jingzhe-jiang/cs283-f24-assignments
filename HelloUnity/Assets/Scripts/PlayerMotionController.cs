using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotionController : MonoBehaviour
{
    public float speed;

    public float rotationSpeed;

    private Animator _animator;

    private float _currentSpeed;

    private Vector3 _previousPos;

    private CharacterController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _currentSpeed = 0;
        _previousPos = transform.position;
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // rotate horizontal mouse movement around world coordinate axis
        // transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed, 0, 
        //    Space.World);

        if (Input.GetKey("w")) // move forward
        {
            //transform.position += transform.forward * speed * Time.deltaTime;
            _controller.Move(transform.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey("s")) // move backward
        {
            //transform.position -= transform.forward * speed * Time.deltaTime;
            _controller.Move(-1 * transform.forward * speed * Time.deltaTime);
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

        // calculate the current x-z plane movement speed
        
        _currentSpeed = Mathf.Sqrt(Mathf.Pow(
            _previousPos.x - transform.position.x, 2)
            + Mathf.Pow(_previousPos.z - transform.position.z, 2))
            / Time.deltaTime;
        
        //_currentSpeed = Vector3.Magnitude(_previousPos - transform.position) / Time.deltaTime;
        _animator.SetFloat("horizontalSpeed", _currentSpeed);
        Debug.Log(_currentSpeed);

        _previousPos = transform.position;
    }
}
