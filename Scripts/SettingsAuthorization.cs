using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsAuthorization : MonoBehaviour
{
    [SerializeField]
    private SettingsManager manager;
    [SerializeField]
    private RectTransform rootRect;
    [SerializeField]
    private InputField input;

    float jiggle;
    float jiggleVel;
    float jiggleAccel = 0.3f;
    float jiggleDrag = 0.75f;

    private void Update()
    {
        jiggleVel -= jiggle * jiggleAccel;
        jiggleVel *= jiggleDrag;
        jiggle += jiggleVel;

        rootRect.anchoredPosition = new Vector2(jiggle, 0);
    }

    // Called by the input field when a password is input.

    public void OnEndEdit(string password)
    {
        if (!string.IsNullOrEmpty(password))
            manager.CheckPassword(password);
    }

    // Called by the SettingsManager when a password check fails.

    public void WrongPassword()
    {
        jiggleVel = 5;
        ClearField();
    }

    public void ClearField()
    {
        input.text = "";
    }
}