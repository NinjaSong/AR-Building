using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{
    public CanvasGroup group;

    private RectTransform rect;

    private bool open;
    private float openAmount;
    private float openSpeed = 0.04f;
    private float openCutoff = 0.99f;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float openTarg = open ? 1 : 0;
        openAmount = Mathf.MoveTowards(openAmount, openTarg, openSpeed);

        float t = Util.Smootherstep(openAmount);
        group.alpha = t;
        group.blocksRaycasts = openAmount > 0;
        group.interactable = openAmount > openCutoff;
        rect.localScale = Mathf.Max(0.1f, Mathf.Pow(t, 0.02f)) * Vector3.one;
    }

    public void SetOpen(bool open)
    {
        this.open = open;
    }
}