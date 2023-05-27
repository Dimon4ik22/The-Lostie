using UnityEditor;
using UnityEngine;

public class ResetTransformHotkey
{
    [MenuItem("Tools/Reset Transform %t")]
    private static void ResetSelectedTransform()
    {
        if (Selection.activeTransform != null)
        {
            Transform selectedTransform = Selection.activeTransform;
            Undo.RecordObject(selectedTransform, "Reset Transform");
            selectedTransform.localPosition = Vector3.zero;
            selectedTransform.localRotation = Quaternion.identity;
            selectedTransform.localScale = Vector3.one;
        }
    }

    [MenuItem("Tools/Reset Transform %t", true)]
    private static bool CanResetSelectedTransform()
    {
        return Selection.activeTransform != null;
    }
}
