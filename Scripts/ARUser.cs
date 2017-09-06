using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARUser : MonoBehaviour
{
    public Transform cam;
    public RawImage cameraDisplayer;
    public Text gpsText;

    public float ox;
    public float oy;
    
    private float heading;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void StartGPS()
    {
        Input.location.Start(5, 5);
        Input.compass.enabled = true;
        Input.gyro.enabled = true;
    }

    public void StartCamera()
    {
        WebCamTexture webcamTexture = new WebCamTexture();
        cameraDisplayer.texture = webcamTexture;
        cameraDisplayer.enabled = true;
        webcamTexture.Play();
    }

    private void Update()
    {
        // Update UI

        gpsText.text = "Location Service:" + System.Environment.NewLine + Input.location.status;

        // Retrieve information from the device's sensors

        float latitude = ARCoordinateUtility.LAT_ORIGIN;
        float longitude = ARCoordinateUtility.LON_ORIGIN;
        float compass = 0;
        Quaternion gyro = Quaternion.identity;

        if (Input.location.status == LocationServiceStatus.Running)
        {
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            compass = Input.compass.trueHeading;
            gyro = GyroUtility.GyroRotation();
        }

        // Move the user based on their GPS position, placing them on top of any colliders

        Vector3 targPos = ARCoordinateUtility.LatLonToPosition(latitude + ox, longitude + oy);

        Ray ray = new Ray(targPos + Vector3.up * 100, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200))
            targPos = hit.point;

        transform.position = Vector3.Lerp(transform.position, targPos, 0.1f);

        // Rotate the camera by combining the gyro and compass readings

        heading = Mathf.LerpAngle(heading, compass, 0.2f);
        
        Vector3 facingVec = gyro * Vector3.forward;
        Vector3 yawVec = Vector3.ProjectOnPlane(facingVec, Vector3.up);
        float pitch = Vector3.Angle(facingVec, yawVec) * (facingVec.y < 0 ? 1 : -1);

        cam.rotation = Quaternion.Euler(pitch, heading, 0);
    }
}