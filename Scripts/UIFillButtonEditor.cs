#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(UIFillButton))]
public class UIFillButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif