using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CommentDisplayConfirmation : Button
{
    public CommentDisplayManager manager;
    public Color backColor;
    public Color backHighlightColor;
    public Color pinColor;
    public Color pinHighlightColor;
    public Image back;
    public Image pin;

    private RectTransform rect;
    private LineRenderer lines;
    private float lineWidth = 0.01f;

    private float colorHighlight = 0;
    private float colorSpeed = 0.05f;

    private bool show;
    private float showAmount;
    private float showDefault = 0.0005f;
    private float showDrag = 0.2f;
    private float showCutoff = 0.99f;

    protected override void Awake()
    {
        base.Awake();

        rect = GetComponent<RectTransform>();
        lines = GetComponentInChildren<LineRenderer>();
    }

    private void Update()
    {
        if (!Application.isPlaying)
            return;

        float showTarg = show ? 1 : 0;
        showAmount = Mathf.Lerp(showAmount, showTarg, showDrag);
        colorHighlight = Mathf.MoveTowards(colorHighlight, interactable && IsPressed() ? 1 : 0, colorSpeed);
        float t = Mathf.SmoothStep(0, 1, showAmount);
        float c = Mathf.SmoothStep(0, 1, colorHighlight);
        
        interactable = showAmount > showCutoff;
        lines.widthMultiplier = t * lineWidth;
        rect.localScale = t * showDefault * (1 - c * 0.05f) * Vector3.one;
        rect.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);

        back.color = Color.Lerp(backColor, backHighlightColor, c);
        pin.color = Color.Lerp(pinColor, pinHighlightColor, c);
    }
    
    public void ShowAtPosition(Vector3 position)
    {
        show = true;
        rect.localPosition = position;
        rect.SetAsLastSibling();
    }

    public void Hide()
    {
        show = false;
    }

    public bool IsShowing()
    {
        return show;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (interactable)
            manager.CommentDisplayConfirmationClicked();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (interactable)
            colorHighlight = 1;
    }
}