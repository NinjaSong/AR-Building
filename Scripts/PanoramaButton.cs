using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanoramaButton : Button
{
    private PanoramaManager manager;
    private Panorama panorama;
    public Panorama Panorama { get { return panorama; } }

    private Image icon;

    public void Initialize(PanoramaManager manager, Panorama panorama)
    {
        this.manager = manager;
        this.panorama = panorama;

        icon = GetComponent<Image>();
        icon.sprite = panorama.icon;
    }
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (interactable)
            manager.ButtonClicked(this);
    }
}