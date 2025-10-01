#if UNITY_EDITOR
using Devs.Jesper;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DatabaseConnection))]
public class DatabaseManagerEditor : Editor
{
    // EventData eventData = new EventData();
    private bool _awaiting = false;
    private bool isLoggedIn = false;
    // public string baseUrl = "http://localhost:3000";

    public override async void OnInspectorGUI()
    {
        // DrawDefaultInspector(); // Draws the normal inspector
        DatabaseConnection dbManager = (DatabaseConnection)target;
        // check if in play mode
        if (Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Database connection settings cannot be changed in Play mode.", MessageType.Warning);
            return;
        }
        dbManager.baseUrl = EditorGUILayout.TextField("Base URL", dbManager.baseUrl);
        // dbManager.apiKey = EditorGUILayout.TextField("API Key", dbManager.apiKey);
        
        

        


        if (dbManager.SuccessfullyConnected)
        {
            EditorGUILayout.LabelField("Status: Connected", EditorStyles.boldLabel);
            GUI.enabled = false;
        }
        else
        {
            EditorGUILayout.LabelField("Status: Not Connected  ", EditorStyles.boldLabel);
            GUI.enabled = true;
        }

        if (GUILayout.Button("Connect to Database") )
        {
            GUI.enabled = false;
            await dbManager.TestConnection();
            GUI.enabled = true;
        }

        GUI.enabled = true;
        // if connected, show a new ui to add dummy data
        if (dbManager.SuccessfullyConnected)
        {
            // Add option to login, logout and create a new session.
            // if(!isLoggedIn)
                
        }
    }
    
    
}
#endif