using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialoguePlayableAsset))]
public class DialoguePlayableAssetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DialoguePlayableAsset asset = (DialoguePlayableAsset)target;

        if (GUILayout.Button("Add New Line"))
        {
            asset.dialogueLines.Add("");
        }
    }
}
