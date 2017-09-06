using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ServerBackend : MonoBehaviour
{
    private readonly static Regex COMMENT_REGEX             = new Regex("{[^}]*}");
    private readonly static int COMMENT_REFRESH_INTERVAL    = 15;

    public delegate void CommentDownloadCallback(Comment[] comments);
    public static event CommentDownloadCallback CommentCallback;

    public delegate void SessionInfoDownloadCallback(SessionInfo info);
    public static event SessionInfoDownloadCallback SessionInfoCallback;

    private static List<Comment> sessionComments = new List<Comment>();     // only used by normal users
    private float commentTimer = 0;                                         // only used by administrators

    private bool downloadingComments;
    private bool uploadingComments;
    private bool authorizingAdmin;
    private bool downloadingSessionInfo;

    // If the current user is an administrator, automatically refresh the comments
    // every COMMENT_REFRESH_INTERVAL seconds. This would let an admin view all the
    // comments in realtime as they arrived from multiple people using the app.

    private void Update()
    {
        if (AppConfiguration.VIEW_ALL_COMMENTS && !downloadingComments && !uploadingComments)
        {
            if (commentTimer > 0)
                commentTimer -= Time.deltaTime;
            else
                DownloadComments();
        }
    }

    // This method is used to refresh which comments are visible in the app,
    // which depends on the user's permissions. Administrators can see all the comments,
    // downloading them from the actual server. Normal users can only see the comments
    // they have submitted during their session so as not to skew the response data.

    public void DownloadComments()
    {
        commentTimer = COMMENT_REFRESH_INTERVAL;
        
        if (AppConfiguration.VIEW_ALL_COMMENTS)
            StartCoroutine(DownloadCommentsFromServerRoutine());
        else
            CommentCallback(sessionComments.ToArray());
    }

    // This is called by the CommentEntryManager when the user has finished preparing
    // a new comment and hits the Submit button. The callback is to inform the
    // CommentEntrySubmitScreen whether or not the upload was successful.

    public void UploadComment(Comment comment, Action<bool> callback)
    {
        if (!uploadingComments)
            StartCoroutine(UploadCommentToServerRoutine(comment, callback));
    }

    public void AuthorizeAdmin(string password, Action<bool> callback)
    {
        if (!authorizingAdmin)
            StartCoroutine(AuthorizeAdminRoutine(password, callback));
    }

    public void DownloadSessionInfo()
    {
        if (!downloadingSessionInfo)
            StartCoroutine(DownloadSessionInfoFromServerRoutine());
    }

    // Called by the SessionManager when a new user session is started.
    // This ensures that a new person using the app won't see the old comments,
    // and the new person is given a unique (enough) ID for the database.

    public void StartSession()
    {
        sessionComments.Clear();
        CommentCreator.USER = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        DownloadComments();
    }

    // ========================================================================================================================

    // This coroutine download comments from the actual server.
    // This is only for administrators, not regular users.

    private IEnumerator DownloadCommentsFromServerRoutine()
    {
        downloadingComments = true;
            
        WWWForm form = new WWWForm();
        form.AddField("key", AppConfiguration.COMMENT_DOWNLOAD_KEY);
        WWW www = new WWW(AppConfiguration.URL, form);

        yield return www;
        downloadingComments = false;

        if (AppConfiguration.VIEW_ALL_COMMENTS && string.IsNullOrEmpty(www.error))
        {
            MatchCollection mc = COMMENT_REGEX.Matches(www.text);

            Comment[] comments = new Comment[mc.Count];
            for (int i = 0; i < mc.Count; i++)
                comments[i] = JsonUtility.FromJson<Comment>(mc[i].Value);

            if (CommentCallback != null)
                CommentCallback(comments);
        }
    }

    // This coroutine attempts to upload a new comment to the server. If successful,
    // comments are then redownloaded so the user can see their submission.

    private IEnumerator UploadCommentToServerRoutine(Comment comment, Action<bool> callback)
    {
        uploadingComments = true;

        WWWForm form = new WWWForm();
        form.AddField("key", AppConfiguration.COMMENT_UPLOAD_KEY);
        form.AddField("timestamp", comment.Timestamp);
        form.AddField("user", comment.User);
        form.AddField("demographics", comment.Demographics);
        form.AddField("location", comment.Location);
        form.AddField("x", comment.X.ToString());
        form.AddField("y", comment.Y.ToString());
        form.AddField("z", comment.Z.ToString());
        form.AddField("petal", comment.Petal);
        form.AddField("text", comment.Text);
        WWW www = new WWW(AppConfiguration.URL, form);

        yield return www;
        uploadingComments = false;

        // Was the upload successful?
        // Pass that to the callback

        bool success = string.IsNullOrEmpty(www.error);
        callback(success);
        
        if (success)
        {
            // In addition to uploading the comment to the server, we locally store the successfully
            // uploaded comment so a user can see just the comments they've written during their session.
            // This requires knowing the unique ID of the new comment (which the server returns.)
            
            int responseID = -1;
            int.TryParse(www.text, out responseID);

            comment.ID = responseID;
            sessionComments.Add(comment);

            // If the upload was successful, download comments again to show the newly uploaded comment

            DownloadComments();
        }
    }

    private IEnumerator AuthorizeAdminRoutine(string password, Action<bool> callback)
    {
        authorizingAdmin = true;

        WWWForm form = new WWWForm();
        form.AddField("key", AppConfiguration.ADMIN_KEY);
        form.AddField("password", password);
        WWW www = new WWW(AppConfiguration.URL, form);

        yield return www;
        authorizingAdmin = false;

        // If successfully authorized, the server will echo "Authorized" (www.text)

        bool success = string.IsNullOrEmpty(www.error) && !string.IsNullOrEmpty(www.text);
        callback(success);
    }

    private IEnumerator DownloadSessionInfoFromServerRoutine()
    {
        downloadingSessionInfo = true;

        WWWForm form = new WWWForm();
        form.AddField("key", AppConfiguration.SESSION_KEY);
        WWW www = new WWW(AppConfiguration.URL, form);

        yield return www;
        downloadingSessionInfo = false;

        if (string.IsNullOrEmpty(www.error) && SessionInfoCallback != null)
            SessionInfoCallback(JsonUtility.FromJson<SessionInfo>(www.text));
    }
}