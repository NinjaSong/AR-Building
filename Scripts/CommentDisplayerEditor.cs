#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(CommentDisplayer))]
public class CommentDisplayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif