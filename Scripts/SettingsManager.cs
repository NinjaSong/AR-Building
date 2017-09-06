using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private AppCoordinator app;
    [SerializeField]
    private ServerBackend serverBackend;
    [SerializeField]
    private UIScreenSequence screenSequence;
    [SerializeField]
    private SettingsAuthorization authScreen;
    [SerializeField]
    private SettingsScreen settingsScreen;

    private bool open;
    private bool busy;

    // Called by the AppCoordinator when the user clicks on the app settings.

    public void OpenSettings()
    {
        if (!open && !busy)
        {
            open = true;
            screenSequence.StartSequence();
            authScreen.ClearField();
            settingsScreen.PopulateSettings();
        }
    }

    // Called by the SettingsAuthorization when the user submits a password.

    public void CheckPassword(string password)
    {
        serverBackend.AuthorizeAdmin(password, CheckPasswordCallback);
    }

    private void CheckPasswordCallback(bool success)
    {
        if (open && !busy)
        {
            if (success)
                screenSequence.NextScreen();
            else
                authScreen.WrongPassword();
        }
    }

    // Called by the Cancel or Apply buttons in the settings screens.

    public void CloseSettings()
    {
        if (open && !busy)
            StartCoroutine(CloseSettingsRoutine());
    }

    private IEnumerator CloseSettingsRoutine()
    {
        busy = true;

        screenSequence.StopSequence();
        yield return new WaitForSeconds(0.75f);

        app.SettingsToMap();

        busy = false;
        open = false;
    }
}