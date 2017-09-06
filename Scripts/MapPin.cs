using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapPin : Button
{
    private MapManager manager;
    private Panorama panorama;
    public Panorama Panorama { get { return panorama; } }

    private Text text;

    private float scale = 0;
    private float scaleVisible = 2;
    private float scaleDrag = 0.2f;
    private bool visible = false;
    private float cutoff = 0.99f;

    public void Initialize(MapManager manager, Panorama panorama)
    {
        this.manager = manager;
        this.panorama = panorama;

        text = GetComponentInChildren<Text>();
        if (text)
            text.text = panorama.name;
    }

    public void SetVisible(bool visible)
    {
        this.visible = visible;
    }

    private void Update()
    {
        if (!Application.isPlaying)
            return;

        Vector3 delta = Camera.main.transform.position - transform.position;
        transform.LookAt(transform.position - delta);

        float scaleTarg = visible ? 1 : 0;
        scale = Mathf.Lerp(scale, scaleTarg, scaleDrag);
        transform.localScale = Mathf.SmoothStep(0, 1, scale) * scaleVisible * Vector3.one;
        interactable = scale > cutoff;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (interactable)
            manager.PinClicked(this);
    }
}