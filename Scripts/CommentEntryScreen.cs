using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentEntryScreen : MonoBehaviour
{
    [Header("Internal References")]
    [SerializeField]
    private CommentEntryManager manager;
    [SerializeField]
    private CanvasScaler scaler;
    [SerializeField]
    private Image background;
    [SerializeField]
    private CanvasGroup group;
    [SerializeField]
    private CanvasGroup textGroup;
    [SerializeField]
    private Text petalTitle;
    [SerializeField]
    private Text petalDescription;
    [SerializeField]
    private Text petalQuestions;
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text timestampText;
    [SerializeField]
    private Button submitButton;

    private RectTransform rect;
    private float fadeLength = 0.2f;

    private bool open;
    private bool busy;
    private Petal prevPetal;
    
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float keyboardHeight = TouchScreenKeyboard.area.height;
        float pushAmount = Util.Smootherstep(Mathf.Clamp01(keyboardHeight / Screen.height * 2));
        float alphaAmount = 1 - Mathf.Pow(group.alpha, 0.075f);
        float displayHeight = keyboardHeight / (Screen.height / scaler.referenceResolution.y);
        rect.offsetMin = new Vector2(0, displayHeight - 100 * alphaAmount);
        rect.offsetMax = new Vector2(0, -60 + 190 * pushAmount - 100 * alphaAmount);
        textGroup.alpha = 1 - pushAmount;

        // Set the timestamp.

        timestampText.text = Util.CurrentReadableTimestamp();

        // Enforce that the user can only submit a non-empty comment.

        inputField.text = inputField.text.Replace("\\", "").TrimStart();
        submitButton.interactable = inputField.text != "";
    }
    
    public void EntryComplete(string text)
    {
        inputField.text = inputField.text.Trim();
    }

    // Callback from the submit button when it is pressed.

    public void Submit()
    {
        if (inputField.text != "")
        {
            CommentCreator.TEXT = inputField.text;
            manager.SubmitClicked();
        }
    }

    public void Open(Petal petal)
    {
        if (!open && !busy)
            StartCoroutine(OpenRoutine(petal));
    }

    private IEnumerator OpenRoutine(Petal petal)
    {
        busy = true;
        
        if (prevPetal != petal)
        {
            prevPetal = petal;
            inputField.text = "";
        }

        petalTitle.text = petal.ToString().ToLower();
        petalDescription.text = Petals.PetalDescription(petal);
        petalQuestions.text = Petals.PETAL_QUESTIONS[(int) petal].Replace("|", System.Environment.NewLine + System.Environment.NewLine);
        background.color = Color.Lerp(Petals.PetalColor(petal), Color.black, 0.5f);

        group.blocksRaycasts = true;
        background.raycastTarget = true;
        
        for (float f = 0; f < fadeLength; f += Time.deltaTime)
        {
            group.alpha = Mathf.SmoothStep(0, 1, f / fadeLength);
            SetBackgroundAlpha(group.alpha);
            yield return null;
        }

        group.alpha = 1;
        SetBackgroundAlpha(1);
        group.interactable = true;

        open = true;
        busy = false;
    }

    public void Close()
    {
        if (open && !busy)
            StartCoroutine(CloseRoutine());
    }

    public void ClearText()
    {
        inputField.text = "";
    }

    public Color GetColor()
    {
        return new Color(background.color.r, background.color.g, background.color.b, 1);
    }

    private IEnumerator CloseRoutine()
    {
        busy = true;

        group.interactable = false;
        
        for (float f = 0; f < fadeLength; f += Time.deltaTime)
        {
            group.alpha = Mathf.SmoothStep(0, 1, 1 - f / fadeLength);
            SetBackgroundAlpha(group.alpha);
            yield return null;
        }

        group.alpha = 0;
        SetBackgroundAlpha(0);
        group.blocksRaycasts = false;
        background.raycastTarget = false;

        open = false;
        busy = false;
    }

    private void SetBackgroundAlpha(float alpha)
    {
        background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
    }
}