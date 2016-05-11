using UnityEditor;
using UnityVOIP;
using System;

[CustomEditor(typeof(VoipSpeaker))]
public class VoipSpeekerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var myTarget = (VoipSpeaker)target;

        EditorGUILayout.HelpBox("Chunks:" + String.Format("{0:#,##0}", myTarget.chunkCount), MessageType.Info);
        EditorGUILayout.HelpBox("Bytes:" + String.Format("{0:#,##0}", myTarget.bytes), MessageType.Info);
        EditorGUILayout.HelpBox("Bytes/Second:" + String.Format("{0:#,##0}", myTarget.bytesPerSecond), MessageType.Info);

        DrawDefaultInspector();
    }
}