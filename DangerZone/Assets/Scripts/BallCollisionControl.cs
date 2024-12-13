using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallCollisionControl : MonoBehaviour
{
    public Text health;
    public int damage;
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject demon = GameObject.Find("/Demon_default/Demon");
        _animator = demon.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (! other.gameObject.CompareTag("player"))
        {
            _animator.SetBool("throwFail", true);
            StartCoroutine(WaitFor1Sec(_animator));
        }
        else 
        {   // deduct health from player
            int.TryParse(health.text, out int currHealth);
            currHealth -= damage;
            health.text = currHealth.ToString();
            // temperally disable collider
            gameObject.GetComponent<Collider>().enabled = false;
            StartCoroutine(WaitFor4Sec());
        }
    }

    private IEnumerator WaitFor1Sec(Animator animator) 
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("throwFail", false);
    }

    private IEnumerator WaitFor4Sec() 
    {
        yield return new WaitForSeconds(4.0f);
        gameObject.GetComponent<Collider>().enabled = true;
    }
}
