using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CommentDisplayManager : MonoBehaviour
{
    [Header("Prefab Reference")]
    [SerializeField]
    private GameObject commentDisplayerPrefab;

    [Space(10)]
    [Header("Internal References")]
    [SerializeField]
    private AppCoordinator app;
    [SerializeField]
    private ServerBackend serverBackend;
    [SerializeField]
    private CommentDisplayConfirmation commentDisplayConfirmation;
    [SerializeField]
    private CommentDisplaySurface commentDisplaySurface;
    [SerializeField]
    private Transform worldUI;

    private List<CommentDisplayer> commentDisplayers = new List<CommentDisplayer>();
    private CommentDisplayer activeCommentDisplayer;
    public CommentDisplayer ActiveCommentDisplayer { get { return activeCommentDisplayer; } }
    private string currentLocation;

    private void OnEnable()
    {
        ServerBackend.CommentCallback += UpdateDisplayersFromComments;
    }

    private void OnDisable()
    {
        ServerBackend.CommentCallback -= UpdateDisplayersFromComments;
    }
    
    private void UpdateDisplayersFromComments(Comment[] allComments)
    {
        // Remove comment displayers that are no longer tied to comments

        List<CommentDisplayer> copy = new List<CommentDisplayer>(commentDisplayers);

        foreach (CommentDisplayer cd in copy)
        {
            if (!allComments.Contains(cd.Comment))
            {
                Destroy(cd.gameObject);
                commentDisplayers.Remove(cd);
            }
        }

        // Add comment displayers for new comments

        List<Comment> existingComments = commentDisplayers.Select(c => c.Comment).ToList();

        foreach (Comment c in allComments.Except(existingComments))
        {
            GameObject g = Instantiate(commentDisplayerPrefab);
            CommentDisplayer cd = g.GetComponent<CommentDisplayer>();

            cd.Initialize(this, worldUI, c);
            cd.interactable = (currentLocation != null && cd.Comment.Location == currentLocation);
            commentDisplayers.Add(cd);
        }
    }

    // Make the displayers and surface follow the camera.

    private void LateUpdate()
    {
        worldUI.position = Camera.main.transform.position;
    }

    // A function called by CommentDisplayers when they are clicked. 
    // This changes the currently active comment.
    
    public void CommentDisplayerClicked(CommentDisplayer cd)
    {
        activeCommentDisplayer = cd;
        commentDisplayConfirmation.Hide();
    }

    // A function called by the CommentDisplaySurface when it is clicked.
    // This represents a click on the "background", which either closes the currently open comment
    // or signifies that the user wants to place a new comment at the given position, showing the
    // CommentDisplayConfirmation where they clicked.

    public void CommentDisplaySurfaceClicked(Vector3 position)
    {
        if (activeCommentDisplayer != null)
            activeCommentDisplayer = null;
        else if (commentDisplayConfirmation.IsShowing())
            commentDisplayConfirmation.Hide();
        else
        {
            commentDisplayConfirmation.ShowAtPosition(position);
            CommentCreator.POSITION = position;
        }
    }

    // A function called by the CommentDisplayConfirmation when it is clicked.
    // This tells the AppCoordinator that the user has confirmed they want to enter a new comment.
    
    public void CommentDisplayConfirmationClicked()
    {
        app.PanoramaToComment();
    }

    // Allows the AppCoordinator or ARCoordinator to "setup" the comments state to the given location,
    // allowing the user to view and initialize comments. Only comments that were made at the given location are shown.

    public void OpenCommentingForLocation(string location)
    {
        currentLocation = location;

        foreach (CommentDisplayer cd in commentDisplayers)
            cd.interactable = (location != null && cd.Comment.Location == location);
        commentDisplaySurface.gameObject.SetActive(true);
    }

    // Allows the AppCoordinator or ARCoordinator to "teardown" the comments state, preventing the user
    // from viewing any comments or being able to comment. This is used during most state transitions.

    public void CloseCommenting()
    {
        currentLocation = null;
        activeCommentDisplayer = null;
        commentDisplayConfirmation.Hide();

        foreach (CommentDisplayer cd in commentDisplayers)
            cd.interactable = false;
        commentDisplaySurface.gameObject.SetActive(false);
    }
}