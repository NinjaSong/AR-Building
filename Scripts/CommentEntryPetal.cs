using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommentEntryPetal : Button
{
    private RectTransform rect;
    private Image icon;
    private Text text;

    private CommentEntryFlower flower;
    private Transform lookAt;
    private Petal petal;

    private Color colorDefault;
    private float colorHighlight = 0;
    private float colorSpeed = 0.05f;

    protected override void Awake()
    {
        base.Awake();

        rect = GetComponent<RectTransform>();
        icon = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }

    public void Initialize(CommentEntryFlower flower, Transform lookAt, Petal petal)
    {
        this.flower = flower;
        this.lookAt = lookAt;
        this.petal = petal;

        rect.SetParent(flower.transform);
        rect.localPosition = Vector3.zero;
        rect.localScale = Vector3.one;
        
        colorDefault = Petals.PetalColor(petal);
        text.text = petal.ToString().ToLower();
    }

    private void Update()
    {
        if (!Application.isPlaying)
            return;

        // Set color

        float targ = flower.IsInteractable() && IsPressed() && flower.IsRecentPetal(petal) ? 1 : 0;
        colorHighlight = Mathf.MoveTowards(colorHighlight, targ, colorSpeed);
        float c = Mathf.SmoothStep(0, 1, colorHighlight);

        icon.color = Color.Lerp(colorDefault, Color.white, c / 2);
        text.color = Color.Lerp(colorDefault, Color.white, 0.5f + c / 2);

        // Animate transforms

        rect.localRotation = Quaternion.Euler(0, 0, (int) petal * (360f / Petals.COUNT)) * Quaternion.Euler(flower.GetPitch(), 0, 0);
        rect.localScale = (1 + c * 0.05f) * flower.GetScale(petal) * Vector3.one;

        text.rectTransform.LookAt(lookAt, lookAt.up);
        text.rectTransform.localRotation *= Quaternion.Euler(0, 180, 0);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (flower.IsInteractable() && flower.IsRecentPetal(petal))
            flower.PetalClicked(petal);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (flower.IsInteractable())
        {
            flower.PetalDown(petal);
            colorHighlight = 1;
        }
    }
}