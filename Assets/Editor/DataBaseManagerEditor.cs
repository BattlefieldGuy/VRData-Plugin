using System.Collections.Generic;
using Devs.Jesper;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ApiGetExample))]
public class DatabaseManagerEditor : Editor
{
    EventData eventData = new EventData();
    bool awaiting = false;

    public override async void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draws the normal inspector

        ApiGetExample dbManager = (ApiGetExample)target;
        
        // add a unclickable checkmark to show if connected or not


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

        if (GUILayout.Button("Connect to Database"))
        {
            GUI.enabled = false;
            await dbManager.TestConnection();
            GUI.enabled = true;
        }

        GUI.enabled = true;
        // if connected, show a new ui to add dummy data
        if (dbManager.SuccessfullyConnected)
        {
            EditorGUILayout.LabelField("Add dummy data", EditorStyles.boldLabel);
            // shows user the a list of the EventData class to fill in
            eventData.controller = (Controller)EditorGUILayout.EnumPopup("Controller", eventData.controller);
            eventData.eventType = EditorGUILayout.TextField("Event Type", eventData.eventType);
            // eventData.timestamp = EditorGUILayout.TextField("Timestamp", eventData.timestamp);
            // add button to send the data to the database
            if (!awaiting)
                if (GUILayout.Button("Send Event Data"))
                {
                    awaiting = true;
                    eventData.timestamp = System.DateTime.UtcNow.ToString("o");
                    await dbManager.PostEvents(90909090, new List<EventData> { eventData });
                    awaiting = false;
                }
        }
    }
    
}
