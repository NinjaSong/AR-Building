using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIArrowButton : Button
{
    private readonly static Color normalColor = new Color(0, 122 / 255f, 1f);
    private readonly static Color selectColor = Color.Lerp(normalColor, Color.white, 0.75f);

    private Image arrow;
    private Text text;

    private float fade;
    private float fadeSpeed = 0.05f;

    protected override void Awake()
    {
        base.Awake();
        arrow = GetComponentInChildren<Image>();
        text = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (!Application.isPlaying)
            return;

        float fadeTarg = IsPressed() || !interactable ? 1 : 0;
        fade = Mathf.MoveTowards(fade, fadeTarg, fadeSpeed);

        Color c = Color.Lerp(normalColor, selectColor, Mathf.SmoothStep(0, 1, fade));
        if (arrow != null)
            arrow.color = c;
        text.color = c;
    }

    public void SetArrowRotation(float r)
    {
        if (arrow != null)
            arrow.rectTransform.localRotation = Quaternion.Euler(0, 0, r);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (interactable)
            fade = 1;
    }
}