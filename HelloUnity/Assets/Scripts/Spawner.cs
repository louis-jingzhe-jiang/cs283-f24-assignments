using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObj;
    public int maxSpawn;
    public float radius;
    private int _spawnCount;
    private GameObject[] _generatedObj;

    // Start is called before the first frame update
    void Start()
    {
        _spawnCount = 0;
        _generatedObj = new GameObject[maxSpawn];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < maxSpawn; i++)
        {
            if (_generatedObj[i] == null)
            {   // haven't been generated yet, generate
                _generatedObj[i] = GameObject.Instantiate(spawnObj, 
                    _GeneratePosition(), transform.rotation);
                _generatedObj[i].SetActive(true);
            }
            else if (!_generatedObj[i].activeInHierarchy)
            {   // not an active object, update its position and make it active
                // transist the animation back to original state
                Animator animator = _generatedObj[i].GetComponent<Animator>();
                animator.SetBool("Exit", false);
                _generatedObj[i].transform.position = _GeneratePosition();
                _generatedObj[i].transform.rotation = spawnObj.transform.rotation;
                _generatedObj[i].transform.localScale = spawnObj.transform.localScale;
                _generatedObj[i].SetActive(true);
            }
        }
    }

    /**
     * Generate a position for the new item in the same horizontal plane as
     * the Spawner
     */
    private Vector3 _GeneratePosition()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(0f, radius);
        float x = Mathf.Cos(angle) * distance + transform.position.x;
        float z = Mathf.Sin(angle) * distance + transform.position.z;
        return new Vector3(x, transform.position.y, z);
    }
}
