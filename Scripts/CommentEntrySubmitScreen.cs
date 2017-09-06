using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentEntrySubmitScreen : MonoBehaviour
{
    [Header("Display Text")]
    public string successHeaderText;
    public string successBodyText;
    public string successButtonText;
    public string errorHeaderText;
    public string errorBodyText;
    public string errorButtonText;

    [Header("Internal References")]
    [SerializeField]
    private CommentEntryManager manager;
    [SerializeField]
    private CanvasGroup group;
    [SerializeField]
    private CanvasGroup queryGroup;
    [SerializeField]
    private CanvasGroup resultGroup;
    [SerializeField]
    private Image background;
    [SerializeField]
    private Text headerText;
    [SerializeField]
    private Text bodyText;
    [SerializeField]
    private UIFillButton button;
    [SerializeField]
    private Text buttonText;
    [SerializeField]
    private RectTransform[] queryDots;

    private bool open;
    private float openAmount;
    private float openSpeed = 0.05f;
    private float openTime;
    private float minimumTime = 1;

    private bool fade;
    private float fadeAmount;
    private float fadeSpeed = 0.075f;
    private float cutoff = 0.99f;

    private bool success;

    private void Update()
    {
        float openTarg = open ? 1 : 0;
        openAmount = Mathf.MoveTowards(openAmount, openTarg, openSpeed);

        float fadeTarg = fade ? 1 : 0;
        fadeAmount = Mathf.MoveTowards(fadeAmount, fadeTarg, fadeSpeed);

        // Screen animation

        group.alpha = Mathf.SmoothStep(0, 1, openAmount);
        group.interactable = openAmount > cutoff;
        group.blocksRaycasts = openAmount > 0;

        queryGroup.alpha = Mathf.SmoothStep(0, 1, 1 - fadeAmount);
        resultGroup.alpha = Mathf.SmoothStep(0, 1, fadeAmount);
        resultGroup.interactable = fadeAmount > cutoff;
        resultGroup.blocksRaycasts = fadeAmount > 0;

        // Query animation

        int n = queryDots.Length;
        for (int i = 0; i < n; i++)
        {
            float x = queryDots[i].anchoredPosition.x;
            float y = 5 * Mathf.Sin(i + Time.time * 8);
            queryDots[i].anchoredPosition = new Vector2(x, y);

            float s = 0.75f + 0.1f * Mathf.Sin(i + Time.time * 5);
            s *= (float) i / (n - 1);
            s *= (float) ((n - 1) - i) / (n - 1) * 4;
            s += 0.1f;

            queryDots[i].localScale = s * Vector3.one;
        }
    }

    // This is called by the ServerBackend, letting this screen know
    // whether or not the upload succeeded or failed.

    public void ResultCallback(bool success)
    {
        this.success = success;

        headerText.text = success ? successHeaderText : errorHeaderText;
        bodyText.text = success ? successBodyText : errorBodyText;
        bodyText.text = bodyText.text.Replace("[n]", System.Environment.NewLine);
        buttonText.text = success ? successButtonText : errorButtonText;

        float delay = Mathf.Max(0, minimumTime - (Time.time - openTime));
        StartCoroutine(FadeRoutine(delay, true));
    }
    
    // This is called by the "OK" button on the results screen when it is pressed.

    public void OKPressed()
    {
        button.Show(false);
        manager.SubmitAcknowledged(success);

        StartCoroutine(FadeRoutine(0.8f, false));
    }

    private IEnumerator FadeRoutine(float delay, bool fade)
    {
        yield return new WaitForSeconds(delay);
        this.fade = fade;
    }

    public void SetOpen(bool open)
    {
        this.open = open;

        if (open)
        {
            fade = false;
            fadeAmount = 0;
            openTime = Time.time;
            button.Show(true);
        }
    }

    public void SetColor(Color c)
    {
        background.color = c;
        button.SetColor(c);
    }
}