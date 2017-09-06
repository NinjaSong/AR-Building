using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommentEntryManager : MonoBehaviour
{
    [Header("Internal References")]
    [SerializeField]
    private AppCoordinator app;
    [SerializeField]
    private ServerBackend serverBackend;
    [SerializeField]
    private UITab menuTab;
    [SerializeField]
    private UITitle flowerTitle;
    [SerializeField]
    private CommentEntryFlower flower;
    [SerializeField]
    private CommentEntryScreen entryScreen;
    [SerializeField]
    private CommentEntrySubmitScreen submitScreen;
    [SerializeField]
    private UIFade fade;
    
    private CommentEntryState state = CommentEntryState.Hidden;

    // ========================================================================================================================

    // Allows the AppCoordinator to boot up the comment entry screen.
    // This is after the user has confirmed they want to make a new comment.

    public void Initialize()
    {
        if (state == CommentEntryState.Hidden)
            StartCoroutine(HiddenToFlowerScreenRoutine());
    }

    private IEnumerator HiddenToFlowerScreenRoutine()
    {
        state = CommentEntryState.Busy;

        fade.Out(0.5f);
        yield return new WaitForSeconds(0.5f);

        menuTab.SetVisible(true);
        flower.SetOpen(true);
        yield return new WaitForSeconds(0.2f);
        
        flowerTitle.Show(true);

        state = CommentEntryState.FlowerScreen;
    }

    // ========================================================================================================================

    // This is called by the CommentEntryFlower when one of its petals is clicked,
    // signifying that the user wants to enter a comment for that petal.

    public void PetalClicked(Petal petal)
    {
        if (state == CommentEntryState.FlowerScreen)
            StartCoroutine(FlowerScreenToEntryScreenRoutine(petal));
    }

    private IEnumerator FlowerScreenToEntryScreenRoutine(Petal petal)
    {
        state = CommentEntryState.Busy;

        entryScreen.Open(petal);
        CommentCreator.PETAL = petal.ToString();
        yield return new WaitForSeconds(0.3f);

        state = CommentEntryState.EntryScreen;
    }

    // ========================================================================================================================

    // This is called by the CommentEntryScreen when the Submit button is pressed,
    // signifying that the user has completed their comment and it should be uploaded.

    public void SubmitClicked()
    {
        if (state == CommentEntryState.EntryScreen)
        {
            serverBackend.UploadComment(CommentCreator.Bake(), submitScreen.ResultCallback);

            menuTab.SetVisible(false);
            submitScreen.SetColor(entryScreen.GetColor());
            submitScreen.SetOpen(true);

            state = CommentEntryState.SubmitScreen;
        }
    }

    // ========================================================================================================================

    // This is called by the CommentEntrySubmitScreen when the user presses "OK" after seeing the submission result.
    // If the submission succeeded, the user is taken back to the panorama to see their new comment.
    // If the submission failed, the user is taken back to the EntryScreen to try again later.

    public void SubmitAcknowledged(bool success)
    {
        if (state == CommentEntryState.SubmitScreen)
        {
            if (success)
                StartCoroutine(SubmitScreenToHiddenRoutine());
            else
                StartCoroutine(SubmitScreenToEntryScreenRoutine());
        }
    }

    private IEnumerator SubmitScreenToHiddenRoutine()
    {
        state = CommentEntryState.Busy;

        flowerTitle.Show(false);
        flower.SetOpen(false);
        entryScreen.Close();
        entryScreen.ClearText();
        fade.In(0.4f);
        yield return new WaitForSeconds(0.4f);

        submitScreen.SetOpen(false);
        yield return new WaitForSeconds(0.5f);

        app.CommentToPanorama();

        state = CommentEntryState.Hidden;
    }

    private IEnumerator SubmitScreenToEntryScreenRoutine()
    {
        state = CommentEntryState.Busy;

        menuTab.SetVisible(true);
        submitScreen.SetOpen(false);
        yield return new WaitForSeconds(0.3f);

        state = CommentEntryState.EntryScreen;
    }

    // ========================================================================================================================

    // This is called by the Back button available in the header. This does slightly different "cancel"
    // operations depending on which stage of entering a comment the user was at.

    public void Back()
    {
        if (state == CommentEntryState.EntryScreen)
            StartCoroutine(EntryScreenToFlowerScreenRoutine());
        else if (state == CommentEntryState.FlowerScreen)
            StartCoroutine(FlowerScreenToHiddenRoutine());
    }

    private IEnumerator EntryScreenToFlowerScreenRoutine()
    {
        state = CommentEntryState.Busy;

        entryScreen.Close();
        yield return new WaitForSeconds(0.3f);

        state = CommentEntryState.FlowerScreen;
    }

    private IEnumerator FlowerScreenToHiddenRoutine()
    {
        state = CommentEntryState.Busy;

        menuTab.SetVisible(false);
        flowerTitle.Show(false);
        flower.SetOpen(false);
        fade.In(0.5f);
        yield return new WaitForSeconds(0.6f);

        app.CommentToPanorama();

        state = CommentEntryState.Hidden;
    }

    // ========================================================================================================================

    public enum CommentEntryState
    {
        Hidden, FlowerScreen, EntryScreen, SubmitScreen, Busy
    }
}