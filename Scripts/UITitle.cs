using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITitle : MonoBehaviour
{
    private RectTransform rect;
    private CanvasGroup group;

    public TitleStyle style;
    public bool show = true;
    public float showDrag = 0.2f;
    public float asymmetricFadeFactor = 1;

    private float showAmount;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (style == TitleStyle.Fade)
            group = gameObject.AddComponent<CanvasGroup>();
        showAmount = show ? 1 : 0;
    }

    private void Update()
    {
        float showTarg = show ? 1 : 0;
        float drag = showDrag * (show ? asymmetricFadeFactor : 1);
        showAmount = Mathf.Lerp(showAmount, showTarg, drag);

        if (style == TitleStyle.Scale)
            rect.localScale = Mathf.SmoothStep(0, 1, showAmount) * Vector3.one;
        else
            group.alpha = showAmount;
    }

    public void Show(bool show)
    {
        this.show = show;
    }

    public enum TitleStyle
    {
        Scale, Fade
    }
}