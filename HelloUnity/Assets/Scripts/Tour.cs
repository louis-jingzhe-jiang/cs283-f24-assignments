using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform[] scenicSpots;
    public float speed;
    private int _index = 0;
    private float _time;
    private float _timeElapsed;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private bool _isLerping;

    // Start is called before the first frame update
    void Start()
    {
        _isLerping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (scenicSpots.Length == 0)
        {   // no scenic spot
            return;
        }
        if (Input.GetKey("n") && !_isLerping)
        {   // start the determining process
            _index++;
            if (_index >= scenicSpots.Length)
            {   // return to the first spot when all of them have been gone over
                _index -= scenicSpots.Length;
            }
            // get current camera transform: Camera.main.transform
            // compute the distance between current camera transform and
            // target scenic spot transform
            _startPosition = Camera.main.transform.position;
            _startRotation = Camera.main.transform.rotation;
            
            float distance = Vector3.Distance(
                _startPosition, 
                scenicSpots[_index].position
            );
            // compute the time needed for this transition
            _time = distance / speed;
            // set the elapsed time for lerp and slerp to be 0
            _timeElapsed = 0;
            _isLerping = true;
            //Debug.Log(_time);
        }
        if (_isLerping)
        {   // do lerp
            Camera.main.transform.position = Vector3.Lerp(
                _startPosition,
                scenicSpots[_index].position,
                _timeElapsed / _time
            );
            // do slerp
            Camera.main.transform.rotation = Quaternion.Slerp(
                _startRotation,
                scenicSpots[_index].rotation,
                _timeElapsed / _time
            );
            // update _timeElapsed
            _timeElapsed += Time.deltaTime;
        }
        if (_timeElapsed > _time - Time.deltaTime)
        {   // stop moving when reach destination
            _isLerping = false;
        }
    }
}
