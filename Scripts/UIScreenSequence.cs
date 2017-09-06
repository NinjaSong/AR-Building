using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreenSequence : MonoBehaviour
{
    public UIScreen[] screens;

    private bool open;
    private int curScreen;
    private bool changingScreens;

    public void StartSequence()
    {
        if (!open)
        {
            open = true;
            screens[curScreen = 0].SetOpen(true);
        }
    }

    public void NextScreen()
    {
        if (open && !changingScreens)
            StartCoroutine(NextScreenRoutine());
    }

    private IEnumerator NextScreenRoutine()
    {
        changingScreens = true;

        screens[curScreen].SetOpen(false);
        yield return new WaitForSeconds(0.3f);

        curScreen++;

        if (curScreen < screens.Length)
            screens[curScreen].SetOpen(true);
        else
        {
            open = false;
            curScreen = 0;
        }

        changingScreens = false;
    }

    public void StopSequence()
    {
        if (open && !changingScreens)
        {
            open = false;
            screens[curScreen].SetOpen(false);
            curScreen = 0;
        }
    }
}