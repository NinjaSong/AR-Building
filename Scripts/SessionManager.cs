using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionManager : MonoBehaviour
{
    [SerializeField]
    private AppCoordinator app;
    [SerializeField]
    private ServerBackend serverBackend;
    [SerializeField]
    private UIScreenSequence screenSequence;
    [SerializeField]
    private Text welcomeText;
    [SerializeField]
    private RectTransform consentRect;
    [SerializeField]
    private Text consentText;
    [SerializeField]
    private SessionDemographics demographics;

    private bool started;
    private bool busy;

    private void OnEnable()
    {
        ServerBackend.SessionInfoCallback += ReceiveSessionInfo;
        serverBackend.DownloadSessionInfo();
    }

    private void OnDisable()
    {
        ServerBackend.SessionInfoCallback -= ReceiveSessionInfo;
    }

    private void ReceiveSessionInfo(SessionInfo info)
    {
        welcomeText.text = info.welcomeText;
        consentText.text = info.consentText;
        demographics.CreateSurvey(info.demographicQuestions);
        Petals.PETAL_QUESTIONS = info.petalQuestions;
    }

    // Called by the AppCoordinator when the app begins or when a new session is started.

    public void StartSession()
    {
        if (!started && !busy)
        {
            started = true;
            AppConfiguration.VIEW_ALL_COMMENTS = false;
            serverBackend.StartSession();
            screenSequence.StartSequence();

            // Reset the UI

            consentRect.anchoredPosition = Vector3.zero;
            demographics.ResetForm();
        }
    }

    // Called by the SessionDemographics when the demographics are submitted.
    // This signifies the end of the session creation sequence.

    public void DemographicsSubmitted()
    {
        if (started && !busy)
            StartCoroutine(EndSessionRoutine());
    }

    private IEnumerator EndSessionRoutine()
    {
        busy = true;

        screenSequence.StopSequence();
        yield return new WaitForSeconds(0.75f);

        app.SessionToMap();

        busy = false;
        started = false;
    }
}