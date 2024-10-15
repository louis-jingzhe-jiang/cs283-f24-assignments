using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathCubic : MonoBehaviour
{
    public Transform[] points;
    /**
     * timePoints represents the relative time of each point to the first
     * point.
     * timePoints[0] should always be 0
     */
    public float[] timePoints;
    private Vector3 _b0;
    private Vector3 _b1;
    private Vector3 _b2;
    private Vector3 _b3;
    private float _duration;
    private int _seg;
    private float _timer;


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
        // When I delete the keyword "float" in front of the variable _timer,
        // it looks like _timer is ticking much faster than the actual time.
        // I have no idea why is that.
        for (float _timer = 0; _timer < _duration; _timer += Time.deltaTime)
        {
            // determine which segment the object is in at this moment
            for (int i = 0; i < timePoints.Length - 1; i++)
            {
                if (_timer > timePoints[i] &&
                    _timer <= timePoints[i + 1]) // located the segment
                {
                    _seg = i;
                    // find parameters b0 & b3, since they are straightforward
                    _b0 = points[_seg].position;
                    _b3 = points[_seg + 1].position;
                    // now we should determine whether the current segment
                    // is the first, last, or middle segment
                    if (_seg == 0) // first segment
                    {
                        _b2 = _b3 - (1.0f / 6.0f) * (points[_seg + 2].position
                            - points[_seg].position);
                        // Confusing problem here: It looks like in homework
                        // webpage, the last term is (_b1 - _b0). However,
                        // we are actually calculating _b1, which shouldn't 
                        // appear on the right hand side of the equation.
                        // I changed _b1 to _b2 here.
                        _b1 = _b0 + (1.0f / 6.0f) * (_b2 - _b0);
                    }
                    else if (_seg == points.Length - 2) // last segment
                    {
                        _b1 = _b0 + (1.0f / 6.0f) * (points[_seg + 1].position
                            - points[_seg - 1].position);
                        _b2 = _b3 + (1.0f / 6.0f) * (_b3 - _b0);
                    }
                    else // middle segments
                    {
                        _b1 = _b0 + (1.0f / 6.0f) * (points[_seg + 1].position 
                            - points[_seg - 1].position);
                        _b2 = _b3 - (1.0f / 6.0f) * (points[_seg + 2].position
                            - points[_seg].position);
                    }
                    // stop the loop since there is no need to keep scanning
                    break;
                }
            }
            // normalize time
            float u = (_timer - timePoints[_seg]) /
                (timePoints[_seg + 1] - timePoints[_seg]);
            transform.position = _Calculate(u);
            // now, calculate the orientation using ideas from calculus
            float uNext = u + Time.deltaTime /
                (timePoints[_seg + 1] - timePoints[_seg]);
            Vector3 expectedPosNextFrame;
            if (uNext >= 1.0f) // at the end of the current segment
            {   // calculate orientation using one time step before
                uNext = u - Time.deltaTime /
                    (timePoints[_seg + 1] - timePoints[_seg]);
                expectedPosNextFrame = _Calculate(uNext);
                transform.rotation = Quaternion.LookRotation(
                    transform.position - expectedPosNextFrame);
            }
            else // at the beginning or middle of the current segment
            {   // calculate orientation using one time step after
                expectedPosNextFrame = _Calculate(uNext);
                transform.rotation = Quaternion.LookRotation(
                    expectedPosNextFrame - transform.position);
            }
            //transform.rotation = Quaternion.Slerp(points[_seg].rotation, 
            //    points[_seg + 1].rotation, u);
            yield return null;
        }
    }

    /**
     * @param t Normalized time (must be between 0 and 1)
     */
    private Vector3 _Calculate(float u)
    {
        return Mathf.Pow(1 - u, 3) * _b0 + 3 * u *
            Mathf.Pow(1 - u, 2) * _b1 + 3 * Mathf.Pow(u, 2) * (1 - u) *
            _b2 + Mathf.Pow(u, 3) * _b3; // according to the equation
    }
}
