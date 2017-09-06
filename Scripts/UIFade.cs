using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Color fadedColor;
    [SerializeField]
    private Color clearColor;

    public bool faded;
    private bool busy;

    public void SetColor(Color c, float fadedAlpha = 1, float clearAlpha = 0)
    {
        fadedColor = new Color(c.r, c.g, c.b, fadedAlpha);
        clearColor = new Color(c.r, c.g, c.b, clearAlpha);
    }

    private void Awake()
    {
        image.enabled = faded;
    }

    public void In(float time)
    {
        if (!busy && faded)
            StartCoroutine(InRoutine(time));
    }

    private IEnumerator InRoutine(float time)
    {
        busy = true;

        time = Mathf.Max(0.1f, time);
        for (float f = 0; f < time; f += Time.deltaTime)
        {
            float t = Mathf.SmoothStep(0, 1, f / time);
            image.color = Color.Lerp(fadedColor, clearColor, t);
            yield return null;
        }

        image.color = clearColor;
        image.raycastTarget = false;
        image.enabled = false;
        faded = false;

        busy = false;
    }

    public void Out(float time)
    {
        if (!busy && !faded)
            StartCoroutine(OutRoutine(time));
    }

    private IEnumerator OutRoutine(float time)
    {
        busy = true;

        image.enabled = true;
        image.raycastTarget = true;

        time = Mathf.Max(0.1f, time);
        for (float f = 0; f < time; f += Time.deltaTime)
        {
            float t = Mathf.SmoothStep(0, 1, f / time);
            image.color = Color.Lerp(clearColor, fadedColor, t);
            yield return null;
        }

        image.color = fadedColor;
        faded = true;

        busy = false;
    }
}