using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIFillButton : Button
{
    private Color normalColor = new Color(50 / 255f, 51 / 255f, 105 / 255f, 230 / 255f);
    private Color selectColor = Color.white;

    private RectTransform rect;
    private Image fill;
    private Text text;

    private float fade;

    public bool show;

    private float showAmount;
    private float showDrag = 0.2f;
    private float showCutoff = 0.99f;

    protected override void Awake()
    {
        base.Awake();

        rect = GetComponent<RectTransform>();
        fill = transform.Find("Fill").GetComponent<Image>();
        text = transform.Find("Text").GetComponent<Text>();
    }

    private void Update()
    {
        if (!Application.isPlaying)
            return;

        float fadeTarg = IsPressed() ? 1 : 0;
        fade = Mathf.MoveTowards(fade, fadeTarg, 0.05f);

        float showTarg = show ? 1 : 0;
        showAmount = Mathf.Lerp(showAmount, showTarg, showDrag);
        
        fill.color = Color.Lerp(normalColor, selectColor, Mathf.SmoothStep(0, 1, fade));
        text.color = Color.Lerp(selectColor, normalColor, Mathf.SmoothStep(0, 1, fade));

        rect.localScale = Mathf.SmoothStep(0, 1, Mathf.Max(0.0001f, showAmount)) * Vector3.one;
        interactable = showAmount > showCutoff;
    }

    public void Show(bool show)
    {
        this.show = show;
    }

    public void SetColor(Color color)
    {
        normalColor = color;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable)
            return;

        base.OnPointerDown(eventData);
        fade = 1;
    }
}