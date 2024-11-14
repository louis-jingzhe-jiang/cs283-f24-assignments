using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class CollectionGame : MonoBehaviour
{
    private int[] _counts;

    public Text[] uiText;

    public string[] tags;

    // Start is called before the first frame update
    void Start()
    {
        _counts = new int[tags.Length];
    }

    // Update is called once per frame
    void Update()
    {
        // update the text on the UI
        for (int i = 0; i < tags.Length; i++)
        {
            uiText[i].text = "x" + _counts[i].ToString();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        int index = indexOf(other.tag);
        if (index != -1) // the item is in the tags
        {   
            // play animation / modify transform
            Animator animator = other.gameObject.GetComponent<Animator>();
            if (animator.GetBool("exit")) 
            {
                return;
            }
            animator.SetBool("exit", true);
            StartCoroutine(SleepAndDeactivate(animator.GetCurrentAnimatorStateInfo(0).length, other));
            // when the character hits an target object, add _count by 1
            _counts[index]++;
            Debug.Log("HIT");
        }
    }

    private int indexOf(string str)
    {
        for (int i = 0; i < tags.Length; i++)
        {
            if (str.Equals(tags[i]))
            {
                return i;
            }
        }
        return -1;
    }

    private IEnumerator SleepAndDeactivate(float s, Collider other) 
    {
        yield return new WaitForSeconds(s);
        // make it inactive
        other.gameObject.SetActive(false);
    }
}
