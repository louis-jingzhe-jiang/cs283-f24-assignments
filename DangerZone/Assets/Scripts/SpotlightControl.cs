using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightControl : MonoBehaviour
{
    private Light _light;
    private float _luminosity;

    public float minSpotAngle;
    public float maxSpotAngle;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
        _luminosity = _light.intensity * _light.spotAngle;
    }

    // Update is called once per frame
    void Update()
    {
        // press f to turn on or turn off the light
        if (Input.GetKeyUp("f"))
        {
            _light.enabled = ! _light.enabled;
        }
        // monitor the scrollwheel to change the focus of the spotlight
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        // Debug.Log(scrollInput);
        if (scrollInput > 0 && _light.spotAngle < maxSpotAngle)
        {
            _light.spotAngle += 2 * scrollInput;
            // recaculate intensity
            _light.intensity = _luminosity / _light.spotAngle;
        }
        else if (scrollInput < 0 && _light.spotAngle > minSpotAngle)
        {
            _light.spotAngle += 2 * scrollInput;
            _light.intensity = _luminosity / _light.spotAngle;
        }
    }
}
