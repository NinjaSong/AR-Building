using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommentDisplaySurface : Button
{
    private CommentDisplayManager manager;

    protected override void Awake()
    {
        manager = FindObjectOfType<CommentDisplayManager>();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        
        Vector3 p = Camera.main.ScreenPointToRay(eventData.position).direction;
        manager.CommentDisplaySurfaceClicked(p);
    }
}