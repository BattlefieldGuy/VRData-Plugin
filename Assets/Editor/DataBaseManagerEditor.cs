using Devs.Jesper;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ApiGetExample))]
public class DatabaseManagerEditor : Editor
{
    public override async void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the normal inspector

        ApiGetExample dbManager = (ApiGetExample)target;
        // add a unclickable checkmark to show if connected or not
        if (dbManager.successfullyConnected)
        {
           
        }

        if (dbManager.successfullyConnected)
        {
            EditorGUILayout.LabelField("Status: Connected", EditorStyles.boldLabel);
            GUI.enabled = false;
        }
        else
        {
            EditorGUILayout.LabelField("Status: Not Connected  ", EditorStyles.boldLabel);
            GUI.enabled = true;
        }

        if (GUILayout.Button("Connect to Database"))
        {
            GUI.enabled = false;
            await dbManager.TestConnection();
            GUI.enabled = true;
        }
        GUI.enabled = true;
    }

}