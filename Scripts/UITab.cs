using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITab : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup allContent;
    [SerializeField]
    private CanvasGroup openContent;
    [SerializeField]
    private UIArrowButton arrowButton;

    [SerializeField]
    private float normalHeight;
    [SerializeField]
    private float openHeight;
    [SerializeField]
    private float invisibleHeight;

    private RectTransform rect;

    private bool open;
    private float openAmount;
    private float openDrag = 0.15f;
    private bool visible;
    private float visibleAmount;
    private float visibleDrag = 0.1f;
    private float cutoff = 0.99f;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float openTarg = open ? 1 : 0;
        openAmount = Mathf.Lerp(openAmount, openTarg, openDrag);
        float t = Mathf.SmoothStep(0, 1, openAmount);

        float visibleTarg = visible ? 1 : 0;
        visibleAmount = Mathf.Lerp(visibleAmount, visibleTarg, visibleDrag);

        float height = normalHeight;
        height = Mathf.Lerp(height, openHeight, t);
        height = Mathf.Lerp(height, invisibleHeight, Mathf.SmoothStep(0, 1, 1 - visibleAmount));
        rect.anchoredPosition = new Vector2(0, height);

        if (arrowButton)
            arrowButton.SetArrowRotation(180 * (1 - t));
        if (openContent)
            openContent.alpha = openAmount * openAmount;
        allContent.interactable = visibleAmount > cutoff;
    }

    public void SetVisible(bool visible)
    {
        this.visible = visible;
        if (!visible)
            open = false;
    }

    public void ToggleOpen()
    {
        open = !open;
    }
}