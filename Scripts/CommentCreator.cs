using UnityEngine;

// The process the user goes through to create a new comment involves many steps and is not entirely linear:
// they change the comment's location by interacting with the PanoramaManager, they choose the (x, y, z)
// by clicking on the CommentDisplaySurface, the petal by clicking a CommentEntryPetal, et cetera...

// Instead of having all these disparate classes be bound tightly together by a "comment-in-progress"
// passing between them, they simply report the most up-to-date information they have to this class, which
// can be easily "baked" into the new comment at an arbitrary future time when all the fields are complete.

public static class CommentCreator
{
    public static int USER;
    public static string DEMOGRAPHICS = "";
    public static string LOCATION;
    public static Vector3 POSITION;
    public static string PETAL;
    public static string TEXT;

    // Takes the information that was set above and returns an actual comment that can be uploaded.

    public static Comment Bake()
    {
        return new Comment(Util.CurrentSQLTimestamp(), USER, DEMOGRAPHICS, LOCATION, POSITION.x, POSITION.y, POSITION.z, PETAL, TEXT);
    }
}