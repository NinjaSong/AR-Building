#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(UISettingsGear))]
public class UISettingsGearEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif