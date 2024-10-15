using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathLinear : MonoBehaviour
{
    public Transform[] points;
    /**
     * timePoints represents the relative time of each point to the first
     * point.
     * timePoints[0] should always be 0
     */
    public float[] timePoints; 
    private float _timer = 0f;
    private float _duration = 0f;
    private int _seg = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoLerp());
        _duration = timePoints[timePoints.Length - 1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(DoLerp());
        }
    }

    IEnumerator DoLerp()
    {
        for (float _timer = 0; _timer < _duration; _timer += Time.deltaTime)
        {
            // determine which segment the object is in at this moment
            for (int i = 0; i < timePoints.Length - 1; i++)
            {
                if (_timer > timePoints[i] && 
                    _timer <= timePoints[i + 1]) // located the segment
                {
                    _seg = i;
                    // stop the loop since there is no need to keep scanning
                    break;
                }
            }
            Debug.Log("timer = " + _timer);
            // normalize time
            float u = (_timer - timePoints[_seg]) / 
                (timePoints[_seg + 1] - timePoints[_seg]);
            transform.position = Vector3.Lerp(points[_seg].position, 
                points[_seg + 1].position, u);
            // keeping the orientation of the character pointing towards the
            // next target position
            transform.rotation = Quaternion.LookRotation(
                points[_seg + 1].position - points[_seg].position);
            //transform.rotation = Quaternion.Slerp(points[_seg].rotation, 
            //    points[_seg + 1].rotation, u);
            yield return null;
        }
    }
}
