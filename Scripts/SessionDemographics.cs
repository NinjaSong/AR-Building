using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionDemographics : MonoBehaviour
{
    [SerializeField]
    private SessionManager sessionManager;
    [SerializeField]
    private GameObject questionPrefab;
    [SerializeField]
    private RectTransform questionHolder;
    [SerializeField]
    private UIArrowButton submitButton;
    [SerializeField]
    private RectTransform rootRect;

    private List<SessionDemographicQuestion> questions = new List<SessionDemographicQuestion>();

    public void CreateSurvey(DemographicQuestion[] questions)
    {
        foreach (SessionDemographicQuestion s in this.questions)
            Destroy(s.gameObject);
        this.questions.Clear();

        foreach (DemographicQuestion q in questions)
        {
            GameObject g = Instantiate(questionPrefab);
            SessionDemographicQuestion sdq = g.GetComponent<SessionDemographicQuestion>();

            sdq.Initialize(questionHolder, q);
            this.questions.Add(sdq);
        }
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(rootRect);
    }

    public void ResetForm()
    {
        foreach (SessionDemographicQuestion q in questions)
            q.ResetQuestion();
    }

    private void Update()
    {
        submitButton.interactable = CanSubmit();
    }

    // This is called when the Submit button is pressed.
    // This stores the user's demographic data in the CommentCreator
    // and informs the SessionManager that demographics have been submitted.

    public void SubmitPressed()
    {
        if (CanSubmit())
        {
            CommentCreator.DEMOGRAPHICS = GetDemographicResponses();
            sessionManager.DemographicsSubmitted();
        }
    }

    private bool CanSubmit()
    {
        bool canSubmit = true;
        
        foreach (SessionDemographicQuestion q in questions)
            canSubmit &= q.IsSatisfied();

        return canSubmit;
    }

    private string GetDemographicResponses()
    {
        string result = "";

        for (int i = 0; i < questions.Count; i++)
        {
            result += questions[i];
            if (i < questions.Count - 1)
                result += " | ";
        }

        return result;
    }
}