using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanoramaManager : MonoBehaviour
{
    [Header("Asset References")]
    [SerializeField]
    private GameObject panoramaButtonPrefab;
    [SerializeField]
    private Material panoramaMaterial;

    [Space(10)]
    [Header("Internal References")]
    [SerializeField]
    private AppCoordinator app;
    [SerializeField]
    private UITab menuTab;
    [SerializeField]
    private UITab panoramaTab;
    [SerializeField]
    private RectTransform panoramaButtonHolder;
    [SerializeField]
    private Text panoramaName;
    [SerializeField]
    private UIFade panoramaNameFade;

    private PanoramaButton[] buttons;

    // For each panorama in the given list, create an associated PanoramaButton in the UI.
    // This is called by the AppCoordinator when the app launches.

    public void CreateButtons(Panorama[] panoramas)
    {
        buttons = new PanoramaButton[panoramas.Length];

        for (int i = 0; i < panoramas.Length; i++)
        {
            GameObject g = Instantiate(panoramaButtonPrefab);
            g.transform.SetParent(panoramaButtonHolder);
            g.transform.localScale = Vector3.one;

            buttons[i] = g.GetComponent<PanoramaButton>();
            buttons[i].Initialize(this, panoramas[i]);
        }
    }

    // A function called by PanoramaButtons when they are clicked. The manager, upon receiving this,
    // lets the AppCoordinator know that the user wants to view the panorama associated with that button.

    public void ButtonClicked(PanoramaButton button)
    {
        app.PanoramaToPanorama(button.Panorama);
        panoramaTab.ToggleOpen();
    }

    // A function called by the "Back to Map" button when it is clicked. This tells the AppCoordinator
    // to return to the map.

    public void BackToMap()
    {
        app.PanoramaToMap();
    }

    // A function which allows the AppCoordinator to display a given panorama (as the skybox.)

    public void DisplayPanorama(Panorama p)
    {
        if (p == null)
            RenderSettings.skybox = null;

        else
        {
            CommentCreator.LOCATION = p.name;
            RenderSettings.skybox = panoramaMaterial;
            panoramaMaterial.SetTexture("_FrontTex", p.front);
            panoramaMaterial.SetTexture("_BackTex", p.back);
            panoramaMaterial.SetTexture("_LeftTex", p.left);
            panoramaMaterial.SetTexture("_RightTex", p.right);
            panoramaMaterial.SetTexture("_UpTex", p.up);
            panoramaMaterial.SetTexture("_DownTex", p.down);

            foreach (PanoramaButton b in buttons)
                b.interactable = b.Panorama.name != p.name;
        }
    }

    // A function which allows the AppCoordinator to display or hide a panorama's name.

    public void DisplayPanoramaName(Panorama p, float time)
    {
        if (p != null)
        {
            panoramaName.text = p.name;
            panoramaNameFade.In(time);
        }

        else
            panoramaNameFade.Out(time);
    }

    // A function which allows the AppCoordinator to get the current panorama's name.

    public string GetPanoramaName()
    {
        return panoramaName.text;
    }

    // A function which allows the AppCoordinator to show and hide the UI tabs.

    public void SetUIVisibility(bool visible)
    {
        menuTab.SetVisible(visible);
        panoramaTab.SetVisible(visible);
    }
}