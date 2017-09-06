#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(CommentDisplayConfirmation))]
public class CommentDisplayConfirmationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif