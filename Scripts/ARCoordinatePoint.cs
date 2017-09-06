using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ARCoordinatePoint : MonoBehaviour
{
    public float latitude;
    public float longitude;
    public float yOffset;
    public string locationName;

    public Text locationText;

    private void Update()
    {
        Vector3 delta = locationText.transform.position - Camera.main.transform.position;
        locationText.transform.LookAt(locationText.transform.position + delta, Vector3.up);
    }

    private void OnValidate()
    {
        transform.position = ARCoordinateUtility.LatLonToPosition(latitude, longitude);
        transform.position = transform.position + Vector3.up * yOffset;
        name = locationName;

        if (locationText)
            locationText.text = locationName;
    }
}