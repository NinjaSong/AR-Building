using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISettingsGear : Button
{
    public Color normalColor;
    public Color selectColor;

    public CanvasGroup group;
    public RectTransform gear;
    public Image gearIcon;
    public RectTransform dropdown;
    public CanvasGroup dropdownGroup;

    private float fadeAmount;
    private float fadeSpeed = 0.1f;

    private bool open;
    private float openAmount;
    private float openSpeed = 0.2f;
    
    private float showAmount;
    private float showSpeed = 0.2f;

    private float cutoff = 0.99f;

    private void Update()
    {
        if (!Application.isPlaying)
            return;

        if (!interactable)
            open = false;

        float showTarg = interactable ? 1 : 0;
        showAmount = Mathf.Lerp(showAmount, showTarg, showSpeed);
        float s = Mathf.SmoothStep(0, 1, showAmount);

        group.alpha = s;
        group.blocksRaycasts = showAmount > (1 - cutoff);
        group.interactable = showAmount > cutoff;

        float fadeTarg = (IsPressed() || open) && interactable ? 1 : 0;
        fadeAmount = Mathf.MoveTowards(fadeAmount, fadeTarg, fadeSpeed);
        gearIcon.color = Color.Lerp(normalColor, selectColor, Mathf.SmoothStep(0, 1, fadeAmount));

        float openTarg = open && interactable ? 1 : 0;
        openAmount = Mathf.Lerp(openAmount, openTarg, openSpeed);
        float t = Mathf.SmoothStep(0, 1, openAmount);

        gear.localScale = s * Vector3.one;
        gear.rotation = Quaternion.Euler(0, 0, t * 60);

        dropdown.anchoredPosition = new Vector3(15, -24 + (1 - t) * 8, 0);
        dropdownGroup.alpha = t * t;
        dropdownGroup.blocksRaycasts = openAmount > (1 - cutoff);
        dropdownGroup.interactable = openAmount > cutoff;
    }

    public void Show(bool show)
    {
        interactable = show;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (interactable)
            fadeAmount = 1;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (interactable)
            open = !open;
    }
}