using UnityEditor;
using UnityVOIP;
using System;

[CustomEditor(typeof(VoipListener))]
public class VoipListenerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var myTarget = (VoipListener)target;

        if ( !myTarget.IsListening )
        {
            EditorGUILayout.HelpBox("Not listening", MessageType.Warning);
        }

        EditorGUILayout.HelpBox("Chunks:" + String.Format("{0:#,##0}", myTarget.chunkCount), MessageType.Info);
        EditorGUILayout.HelpBox("Bytes:" + String.Format("{0:#,##0}", myTarget.bytes), MessageType.Info);
        EditorGUILayout.HelpBox("Bytes/Second:" + String.Format("{0:#,##0}", myTarget.bytesPerSecond), MessageType.Info);
        DrawDefaultInspector();
    }

    public override bool RequiresConstantRepaint()
    {
        return true;
    }
}