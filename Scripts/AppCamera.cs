using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppCamera : MonoBehaviour
{
    [Header("Internal References")]
    [SerializeField]
    private Transform map;
    [SerializeField]
    private MapCamera mapCamera;

    private Camera cam;

    private Vector3 panoramaPos;
    private float panoramaAmount;
    private bool busy;

    private static float PANORAMA_FOV = 90;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Vector3 pos = mapCamera.position;
        Quaternion rot = mapCamera.rotation;
        float fov = mapCamera.fieldOfView;

        if (panoramaAmount > 0)
        {
            pos = Vector3.Lerp(pos, panoramaPos, panoramaAmount);
            rot = Quaternion.Slerp(rot, GyroUtility.GyroRotation(), Mathf.Pow(panoramaAmount, 6));
            fov = Mathf.Lerp(fov, PANORAMA_FOV, Mathf.Pow(panoramaAmount, 6));
        }

        transform.position = pos;
        transform.rotation = rot;
        cam.fieldOfView = fov;
    }

    public void Transition(Panorama p = null, float time = 0)
    {
        if (!busy)
            StartCoroutine(TransitionRoutine(p, time));
    }

    private IEnumerator TransitionRoutine(Panorama p, float time)
    {
        busy = true;

        if (p != null)
            panoramaPos = map.TransformPoint(p.position);

        if (time > 0)
        {
            for (float f = 0; f < time; f += Time.deltaTime)
            {
                float t = Mathf.SmoothStep(0, 1, f / time);
                panoramaAmount = p != null ? t : 1 - t;

                yield return null;
            }
        }
        
        panoramaAmount = p != null ? 1 : 0;
        busy = false;
    }
}