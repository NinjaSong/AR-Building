using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentEntryFlower : MonoBehaviour
{
    [Header("Internal References")]
    [SerializeField]
    private CommentEntryManager manager;
    [SerializeField]
    private Camera screenCamera;
    [SerializeField]
    private GameObject commentEntryPetalPrefab;

    private CanvasGroup group;
    private List<CommentEntryPetal> commentEntryPetals;
    private Petal recentPetal;

    private bool open;
    private float openAmount;
    private float openSpeed = 0.03f;
    private float openCutoff = 0.99f;
    private float intro;

    // When the CommentEntryFlower starts, it needs to populate itself
    // with all the petals from the Petals class, allowing the user to
    // select whichever petal categorizes their comment.

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        commentEntryPetals = new List<CommentEntryPetal>();

        foreach (Petal petal in Enum.GetValues(typeof(Petal)))
        {
            GameObject g = Instantiate(commentEntryPetalPrefab);
            CommentEntryPetal cep = g.GetComponent<CommentEntryPetal>();
            cep.Initialize(this, screenCamera.transform, petal);
            commentEntryPetals.Add(cep);
        }
    }

    // Animate the flower.

    private void Update()
    {
        float targ = open ? 1 : 0;
        float speed = open ? openSpeed : openSpeed * 2;
        openAmount = Mathf.MoveTowards(openAmount, targ, speed);
        float t = Util.Smootherstep(openAmount);
        intro *= 0.975f;

        group.alpha = Mathf.Sqrt(t);

        float yaw = Mathf.Sin(Time.time * 0.25f) * 10;
        float roll = Mathf.Sin(Time.time * 0.125f) * 10 - intro * 60;
        transform.localRotation = Quaternion.Euler(45, yaw, roll);
        transform.localScale = t * Vector3.one;
    }

    // Allows the petals to get how "open" they should be (local pitch.)

    public float GetPitch()
    {
        return -15 + Mathf.Sin(Time.time) * 2.5f + intro * intro * -45;
    }

    // Allows the petals to get what size they should be.

    public float GetScale(Petal petal)
    {
        return 1;
    }

    // Allows the petals to know if they should be clickable.

    public bool IsInteractable()
    {
        return openAmount > openCutoff;
    }

    // Allows the petals to report that they have received a touch,
    // which allows the flower to keep track of the most recently touched petal.

    public void PetalDown(Petal petal)
    {
        recentPetal = petal;
    }

    // Allows the petals to check if they are the most recently touched petal.

    public bool IsRecentPetal(Petal petal)
    {
        return petal == recentPetal;
    }

    // Allows the petals to report that they were clicked,
    // which the flower reports to the CommentEntryManager.

    public void PetalClicked(Petal petal)
    {
        manager.PetalClicked(petal);
    }

    // Allows the CommentEntryManager to open and close the flower.

    public void SetOpen(bool open)
    {
        this.open = open;
        if (open)
            intro = 1;
    }
}