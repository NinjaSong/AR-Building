using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommentDisplayer : Button
{
    private CommentDisplayManager manager;
    private Comment comment;
    public Comment Comment { get { return comment; } }
    
    public RectTransform rootRect;
    public RectTransform commentRect;
    public RectTransform iconRect;
    public Image pin;
    public Image back;
    public CanvasGroup headerGroup;
    public CanvasGroup contentGroup;
    public Text headerText;
    public Text contentText;

    private Color colorDefault;
    private float colorHighlight = 0;
    private float colorSpeed = 0.05f;

    private float rootScale = 0f;
    private float rootScaleDefault = 0.0005f;
    private float commentScale = 0f;
    private float commentScaleDefault = 5f;
    private float iconScale = 0;
    private float scaleDrag = 0.2f;

    public void Initialize(CommentDisplayManager manager, Transform parent, Comment comment)
    {
        this.manager = manager;
        this.comment = comment;
        rootRect.SetParent(parent);
        rootRect.localPosition = new Vector3(comment.X, comment.Y, comment.Z).normalized;

        colorDefault = Petals.PetalColor(Petals.StringToPetal(comment.Petal));

        headerText.text = HeaderFormatter(comment);
        contentText.text = ContentFormatter(comment);

        LayoutRebuilder.ForceRebuildLayoutImmediate(rootRect);
    }

    private void Update()
    {
        if (!Application.isPlaying)
            return;

        // Set root and rotation scale (the pin being visible or not based on its interactivity)

        rootScale = Mathf.Lerp(rootScale, interactable ? 1 : 0, scaleDrag);
        rootRect.localScale = Mathf.SmoothStep(0, 1, rootScale) * rootScaleDefault * Vector3.one;
        rootRect.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);

        // Set parameters relating to whether or not the comment is active (showing text rather than just the pin)

        commentScale = Mathf.Lerp(commentScale, manager.ActiveCommentDisplayer == this ? 1 : 0, scaleDrag);
        float s = Mathf.SmoothStep(0, 1, commentScale);

        commentRect.localScale = s * commentScaleDefault * Vector3.one;
        headerGroup.alpha = s;
        contentGroup.alpha = s;

        // Set icon scale

        float t = 1 + Mathf.Sin(Time.time * 5) * 0.025f;
        bool isHighlighted = interactable && IsPressed();
        if (isHighlighted)
            t = 0.85f;
        iconScale = Mathf.Lerp(iconScale, t, 0.4f);
        iconRect.localScale = iconScale * Vector3.one;

        // Set color

        colorHighlight = Mathf.MoveTowards(colorHighlight, isHighlighted ? 1 : 0, colorSpeed);

        Color c = Color.Lerp(colorDefault, Color.white, Mathf.SmoothStep(0, 1, colorHighlight) / 2);
        pin.color = c;
        back.color = c;
        headerText.color = c;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (interactable)
        {
            if (manager.ActiveCommentDisplayer != this)
                manager.CommentDisplayerClicked(this);
            else
                manager.CommentDisplayerClicked(null);
            rootRect.SetAsLastSibling();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (interactable)
            colorHighlight = 1;
    }

    // ============================================================================================ Formatting helpers

    private static string HeaderFormatter(Comment comment)
    {
        return comment.Petal.ToLower();
    }

    private static string ContentFormatter(Comment comment)
    {
        string result = comment.Text;
        result = result.Replace("&amp;", "&");
        result = result.Replace("&quot;", "\"");
        result = result.Replace("&#039;", "'").Replace("&apos;", "'");
        result = result.Replace("&lt;", "<");
        result = result.Replace("&gt", ">");
        result += System.Environment.NewLine + System.Environment.NewLine;
        result += "<color=#aaaaaa>" + Util.SQLTimestampToReadableTimestamp(comment.Timestamp) + "</color>";
        return result;
    }
}