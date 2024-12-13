using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public GameObject book;
    public GameObject door;
    public GameObject text;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = book.GetComponent<Animator>();
        _animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("key"))
        {
            // play animation
            StartCoroutine(_PlayAnimation());
            // open the gate
            door.transform.rotation = Quaternion.AngleAxis(180, 
                door.transform.up);
            // Display words on the UI
            StartCoroutine(_ShowPanel());
        }
    }

    private IEnumerator _ShowPanel()
    {
        text.SetActive(true);
        yield return new WaitForSeconds(2f);
        text.SetActive(false);
    }

    private IEnumerator _PlayAnimation()
    {
        _animator.enabled = true;
        yield return new WaitForSeconds(0.9f);
        book.SetActive(false);
    }
}
