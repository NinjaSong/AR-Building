using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ARCoordinator : MonoBehaviour
{
    [Space(10)]
    [Header("Internal References")]
    [SerializeField]
    private UIFade fade;
    [SerializeField]
    private UIFillButton[] buttons;

    private bool busy = true;

    private void Awake()
    {
        StartCoroutine(StartARRoutine());
    }

    private IEnumerator StartARRoutine()
    {
        fade.In(0.4f);
        yield return new WaitForSeconds(0.5f);

        foreach (UIFillButton b in buttons)
            b.Show(true);
        busy = false;
    }

    public void ARToMap()
    {
        if (!busy)
            StartCoroutine(ARToMapRoutine());
    }

    private IEnumerator ARToMapRoutine()
    {
        busy = true;

        foreach (UIFillButton b in buttons)
            b.Show(false);
        fade.Out(0.5f);
        yield return new WaitForSeconds(0.6f);

        SceneManager.LoadScene("App");
    }
}