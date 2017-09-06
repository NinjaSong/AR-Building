using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapSurface : Selectable
{
    [Header("Internal References")]
    [SerializeField]
    private MapCamera cam;
    
    private bool touched;

    private void Update()
    {
        if (!Application.isPlaying)
            return;
        
        if (touched)
        {
            if (Input.touchCount == 1)
            {
                Touch t = Input.GetTouch(0);

                Vector2 move = t.deltaPosition / Screen.width;
                //float direction = t.position.y / Screen.height;
                //move.x *= (0.5f - direction) * 2;

                cam.Movement(move);
            }

            if (Input.touchCount == 2)
            {
                Touch t1 = Input.GetTouch(0);
                Touch t2 = Input.GetTouch(1);
                Vector2 lastPos1 = t1.position - t1.deltaPosition;
                Vector2 lastPos2 = t2.position - t2.deltaPosition;
                float lastDist = (lastPos1 - lastPos2).magnitude / Screen.width;
                float curDist = (t1.position - t2.position).magnitude / Screen.width;

                cam.Zoom(curDist / lastDist);
            }
        }
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        touched = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        touched = false;
    }
}