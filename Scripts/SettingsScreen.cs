using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    public ServerBackend serverBackend;
    public Toggle viewAllComments;
    public InputField serverURL;

    public void PopulateSettings()
    {
        viewAllComments.isOn = AppConfiguration.VIEW_ALL_COMMENTS;
        serverURL.text = AppConfiguration.URL;
    }

    public void OnToggle(bool value)
    {
        AppConfiguration.VIEW_ALL_COMMENTS = value;
        serverBackend.DownloadComments();
    }

    public void OnInputField(string input)
    {
        input = input.Replace("http://", "").Replace("https://", "");

        AppConfiguration.SERVER_URL = input;
        serverURL.text = AppConfiguration.URL;
    }
}