using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField]
    private Transform cameraPoint;
    public Vector3 position { get { return cameraPoint.position; } }
    public Quaternion rotation { get { return cameraPoint.rotation; } }
    public float fieldOfView { get { return fov; } }

    private float rotX = 20;
    private float rotXMin = 10;
    private float rotXMax = 55;
    private float rotY = 45;

    private float fov = 30;
    private float fovTarg = 30;
    private float fovMin = 15;
    private float fovMax = 35;

    private Vector2 movement;
    private float movementSpeed = 450f;
    private float movementDrag = 0.9f;

    private void Update()
    {
        rotX -= movement.y * movementSpeed;
        rotX = SoftClamp(rotX, rotXMin, rotXMax, 5, 0.5f);
        rotY += movement.x * movementSpeed;
        movement *= movementDrag;

        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        fov = Mathf.Lerp(fov, fovTarg, 0.25f);
    }

    public void ResetRotation()
    {
        rotX = 20;
        rotY = 45;
    }

    // Functions called by the MapSurface (which the user drags and zooms on)
    // that let the user change the movement and FOV of the camera.

    public void Movement(Vector2 movement)
    {
        this.movement = Vector2.Lerp(this.movement, movement, 0.25f);
    }

    public void Zoom(float zoom)
    {
        fovTarg /= zoom;
        fovTarg = SoftClamp(fovTarg, fovMin, fovMax, 5, 0.5f);
    }

    // Helper function which clamps values with a region of elasticity.

    private float SoftClamp(float x, float min, float max, float border, float dampening)
    {
        if (x < min + border)
            x = Mathf.Lerp(x, min + border, dampening);
        if (x > max - border)
            x = Mathf.Lerp(x, max - border, dampening);
        return Mathf.Clamp(x, min, max);
    }
}