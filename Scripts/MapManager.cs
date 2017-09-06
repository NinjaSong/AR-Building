using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [Header("Prefab Reference")]
    [SerializeField]
    private GameObject mapPinPrefab;

    [Space(10)]
    [Header("Internal References")]
    [SerializeField]
    private AppCoordinator app;
    [SerializeField]
    private GameObject map;
    [SerializeField]
    private Transform mapPinHolder;
    [SerializeField]
    private MapSurface mapSurface;
    [SerializeField]
    private UITitle mapTitle;
    [SerializeField]
    private UISettingsGear settingsGear;
    [SerializeField]
    private UIFillButton ARButton;

    private List<MapPin> pins;

    // For each panorama in the given list, create an associated MapPin on the map.
    // This is called by the AppCoordinator when the app launches.

    public void CreatePins(Panorama[] panoramas)
    {
        pins = new List<MapPin>(panoramas.Length);

        for (int i = 0; i < panoramas.Length; i++)
        {
            GameObject g = Instantiate(mapPinPrefab);
            g.transform.SetParent(mapPinHolder);
            g.transform.localPosition = panoramas[i].position;

            MapPin p = g.GetComponent<MapPin>();
            p.Initialize(this, panoramas[i]);
            pins.Add(p);
        }
    }

    // Sort the pins visually

    public void Update()
    {
        Vector3 c = Camera.main.transform.position;

        pins.Sort(delegate (MapPin a, MapPin b)
        {
            return (b.transform.position - c).sqrMagnitude.CompareTo((a.transform.position - c).sqrMagnitude);
        });

        foreach (MapPin p in pins)
            p.transform.SetAsLastSibling();
    }

    // A function called by MapPins when they are clicked. The manager, upon receiving this,
    // lets the AppCoordinator know that the user wants to view the panorama associated with that pin.

    public void PinClicked(MapPin pin)
    {
        app.MapToPanorama(pin.Panorama);
    }

    // A function called by the AR button when it is clicked. The manager lets the AppCoordinator
    // know that the user wants to go to the AR portion of the app.

    public void ARButtonClicked()
    {
        app.MapToAR();
    }

    // A function which allows the AppCoordinator to control whether or not the map is interactive.
    // This controls pin visibility and also whether or not the map surface can be dragged and zoomed.

    public void SetMapInteractivity(bool interactive)
    {
        foreach (MapPin p in pins)
            p.SetVisible(interactive);
        mapSurface.gameObject.SetActive(interactive);
        mapTitle.Show(interactive);
        ARButton.Show(interactive);
        settingsGear.Show(interactive);
    }

    // A function which allows the AppCoordinator to control whether or not the map is visible.

    public void SetMapVisibility(bool visible)
    {
        map.SetActive(visible);
    }
}