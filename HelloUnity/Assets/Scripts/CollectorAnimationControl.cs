using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorAnimationControl : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("exit", true);
    }
}
