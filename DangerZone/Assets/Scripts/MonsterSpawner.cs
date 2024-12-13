using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monster;
    public GameObject hitBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.CompareTag("Spawn Trigger"))
        {
            // spawn monster
            monster.SetActive(true);
            GameObject demon = GameObject.Find("/Demon_default/Demon");
            Animator animator = demon.GetComponent<Animator>();
            animator.SetBool("spawn", true);
            StartCoroutine(WaitFor2Sec(animator));
            // disable the collider game object
            hitBox.SetActive(false);
        }
    }
    private IEnumerator WaitFor2Sec(Animator animator) 
    {
        yield return new WaitForSeconds(2.0f);
        animator.SetBool("spawn", false);
    }
}
