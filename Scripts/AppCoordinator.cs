using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppCoordinator : MonoBehaviour
{
    [Header("Panorama References")]
    [SerializeField]
    private Panorama[] panoramas;

    [Space(10)]
    [Header("Internal References")]
    [SerializeField]
    private AppCamera appCamera;
    [SerializeField]
    private SessionManager sessionManager;
    [SerializeField]
    private SettingsManager settingsManager;
    [SerializeField]
    private MapManager mapManager;
    [SerializeField]
    private PanoramaManager panoramaManager;
    [SerializeField]
    private CommentDisplayManager commentDisplayManager;
    [SerializeField]
    private CommentEntryManager commentEntryManager;
    [SerializeField]
    private ServerBackend serverBackend;
    [SerializeField]
    private UIFade fade;

    // This is the state variable for the entire app, which is how the coordinator
    // keeps track of where it is and what options it should be presenting the user
    // via the lower-level UI managers.

    private AppState currentState = AppState.StartingSession;

    // When the app starts, the coordinator tells the managers to create UI objects
    // for the panoramas it has reference to above. Pins must be placed on the map
    // and buttons must be placed inside the panorama tab. This consolidates which
    // panoramas are viewable into a single field in the coordinator.

    private void Awake()
    {
        mapManager.CreatePins(panoramas);
        panoramaManager.CreateButtons(panoramas);

        Application.targetFrameRate = 60;
        Input.gyro.enabled = true;

        if (!AppConfiguration.FIRST_INIT)
            sessionManager.StartSession();
        else
        {
            SessionToMap();
            serverBackend.DownloadComments();
        }

        AppConfiguration.FIRST_INIT = true;
    }

    private void Update()
    {
        if (currentState == AppState.ViewingMap)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                MapToSession();
            if (Input.GetKeyDown(KeyCode.X))
                MapToSettings();
        }
    }

    // ======================================================================================================================== TRANSITION FUNCTIONS

    // This function and coroutine handle the transition from the new-session screen sequence
    // (welcome, consent, demographics) to viewing the map of the GTLB.

    public void SessionToMap()
    {
        if (currentState == AppState.StartingSession)
            StartCoroutine(SessionToMapRoutine());
    }

    private IEnumerator SessionToMapRoutine()
    {
        currentState = AppState.Busy;

        FindObjectOfType<MapCamera>().ResetRotation();
        mapManager.SetMapVisibility(true);
        fade.In(0.5f);
        yield return new WaitForSeconds(0.6f);

        mapManager.SetMapInteractivity(true);

        currentState = AppState.ViewingMap;
    }

    // This function and coroutine handle the transition from viewing the map to starting a new session.

    public void MapToSession()
    {
        if (currentState == AppState.ViewingMap)
            StartCoroutine(MapToSessionRoutine());
    }

    private IEnumerator MapToSessionRoutine()
    {
        currentState = AppState.Busy;

        mapManager.SetMapInteractivity(false);
        fade.Out(0.5f);
        yield return new WaitForSeconds(0.6f);

        mapManager.SetMapVisibility(false);
        sessionManager.StartSession();

        currentState = AppState.StartingSession;
    }

    // This function and coroutine handle the transition from changing the settings to viewing the map.

    public void SettingsToMap()
    {
        if (currentState == AppState.ChangingSettings)
            StartCoroutine(SettingsToMapRoutine());
    }

    private IEnumerator SettingsToMapRoutine()
    {
        currentState = AppState.Busy;

        mapManager.SetMapVisibility(true);
        fade.In(0.5f);
        yield return new WaitForSeconds(0.6f);

        mapManager.SetMapInteractivity(true);

        currentState = AppState.ViewingMap;
    }

    // This function and coroutine handle the transition from viewing the map to changing the settings.

    public void MapToSettings()
    {
        if (currentState == AppState.ViewingMap)
            StartCoroutine(MapToSettingsRoutine());
    }

    private IEnumerator MapToSettingsRoutine()
    {
        currentState = AppState.Busy;

        mapManager.SetMapInteractivity(false);
        fade.Out(0.5f);
        yield return new WaitForSeconds(0.6f);

        mapManager.SetMapVisibility(false);
        settingsManager.OpenSettings();

        currentState = AppState.ChangingSettings;
    }

    // This function and coroutine handle the transition from viewing the map to the AR portion of the app.
    // This is called by the MapManager when the AR button is clicked.

    public void MapToAR()
    {
        if (currentState == AppState.ViewingMap)
            StartCoroutine(MapToARRoutine());
    }

    private IEnumerator MapToARRoutine()
    {
        currentState = AppState.Busy;

        mapManager.SetMapInteractivity(false);
        fade.Out(0.5f);
        yield return new WaitForSeconds(0.6f);

        SceneManager.LoadScene("AR");
    }
    
    // This function and coroutine handle the transition from viewing the map to viewing a panorama.
    // This is called by the MapManager when any of the map pins are clicked.

    public void MapToPanorama(Panorama p)
    {
        if (currentState == AppState.ViewingMap && p != null)
            StartCoroutine(MapToPanoramaRoutine(p));
    }

    private IEnumerator MapToPanoramaRoutine(Panorama p)
    {
        currentState = AppState.Busy;
        
        mapManager.SetMapInteractivity(false);
        appCamera.Transition(p, 1.5f);
        yield return new WaitForSeconds(0.5f);

        fade.Out(0.5f);
        yield return new WaitForSeconds(0.6f);

        mapManager.SetMapVisibility(false);
        panoramaManager.DisplayPanorama(p);
        panoramaManager.DisplayPanoramaName(p, 0.4f);
        panoramaManager.SetUIVisibility(true);
        fade.In(0.4f);
        yield return new WaitForSeconds(0.5f);

        commentDisplayManager.OpenCommentingForLocation(p.name);

        currentState = AppState.ViewingPanorama;
    }

    // This function and coroutine handle the transition from viewing one panorama to another.
    // This is called by the PanoramaManager when any of the panorama buttons are clicked.

    public void PanoramaToPanorama(Panorama p)
    {
        if (currentState == AppState.ViewingPanorama && p != null)
            StartCoroutine(PanoramaToPanoramaRoutine(p));
    }

    private IEnumerator PanoramaToPanoramaRoutine(Panorama p)
    {
        currentState = AppState.Busy;

        panoramaManager.DisplayPanoramaName(null, 0.3f);
        commentDisplayManager.CloseCommenting();
        fade.Out(0.3f);
        yield return new WaitForSeconds(0.4f);

        appCamera.Transition(p);
        panoramaManager.DisplayPanorama(p);
        panoramaManager.DisplayPanoramaName(p, 0.3f);
        fade.In(0.3f);
        yield return new WaitForSeconds(0.4f);

        commentDisplayManager.OpenCommentingForLocation(p.name);

        currentState = AppState.ViewingPanorama;
    }

    // This function and coroutine handle the transition from viewing a panorama to viewing the map.
    // This is called by the PanoramaManager when the "Back to Map" button is clicked.

    public void PanoramaToMap()
    {
        if (currentState == AppState.ViewingPanorama)
            StartCoroutine(PanoramaToMapRoutine());
    }

    private IEnumerator PanoramaToMapRoutine()
    {
        currentState = AppState.Busy;

        appCamera.Transition(null, 1.5f);
        panoramaManager.SetUIVisibility(false);
        panoramaManager.DisplayPanoramaName(null, 0.4f);
        commentDisplayManager.CloseCommenting();
        fade.Out(0.4f);
        yield return new WaitForSeconds(0.5f);
        
        panoramaManager.DisplayPanorama(null);
        mapManager.SetMapVisibility(true);
        fade.In(0.4f);
        yield return new WaitForSeconds(0.75f);

        mapManager.SetMapInteractivity(true);
        yield return new WaitForSeconds(0.3f);

        currentState = AppState.ViewingMap;
    }

    // This function and coroutine handle the initialization of comment entry.
    // This is called by the CommentDisplayManager when the creation of a new comment is confirmed.

    public void PanoramaToComment()
    {
        if (currentState == AppState.ViewingPanorama)
            StartCoroutine(PanoramaToCommentRoutine());
    }

    private IEnumerator PanoramaToCommentRoutine()
    {
        currentState = AppState.Busy;

        panoramaManager.SetUIVisibility(false);
        commentDisplayManager.CloseCommenting();
        yield return new WaitForSeconds(0.25f);

        commentEntryManager.Initialize();
        
        currentState = AppState.EnteringComment;
    }

    // This function and coroutine handle the finalization of comment entry.
    // This is called by the CommentEntryManager when the user is done commenting (submitted or cancelled.)

    public void CommentToPanorama()
    {
        if (currentState == AppState.EnteringComment)
            StartCoroutine(CommentToPanoramaRoutine());
    }

    private IEnumerator CommentToPanoramaRoutine()
    {
        currentState = AppState.Busy;

        panoramaManager.SetUIVisibility(true);
        yield return new WaitForSeconds(0.4f);

        commentDisplayManager.OpenCommentingForLocation(panoramaManager.GetPanoramaName());

        currentState = AppState.ViewingPanorama;
    }
}

public enum AppState
{
    StartingSession, ChangingSettings, ViewingMap, ViewingPanorama, EnteringComment, Busy
}