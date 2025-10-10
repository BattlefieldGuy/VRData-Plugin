#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using Devs.Jesper;

[CustomEditor(typeof(DatabaseEditorConnection))]
public class DatabaseEditorConnectionEditor : Editor
{
    private string username = "Username";
    private int year = 2000, month = 1, day = 1;
    private bool errored = false;
    private bool _loginScreen;
    private bool _registerScreen;
    private DatabaseEditorConnection _dbe;

    public override void OnInspectorGUI()
    {
        if (!_dbe)
            _dbe = (DatabaseEditorConnection)target;

        // Draw baseUrl normally
        _dbe.baseUrl = EditorGUILayout.TextField("Base URL", _dbe.baseUrl);

        // Connection status
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Status", _dbe.connected ? "Connected" : "Not Connected");
        if (!_dbe.connected)
            if (GUILayout.Button("Test Connection"))
            {
                _ = _dbe.TestConnection(); // fire async without blocking
            }

        if (_dbe.connected)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("User", _dbe.userId >= 0 ? _dbe.userId.ToString() : "Not Logged In");

            if (!_dbe.LoggedIn)
            {
                if (!_loginScreen && !_registerScreen)
                {
                    GUILayout.BeginHorizontal();
                    _loginScreen = GUILayout.Toggle(_loginScreen, "Login", "Button");
                    _registerScreen = GUILayout.Toggle(_registerScreen, "Register", "Button");
                    GUILayout.EndHorizontal();
                }
                else
                {
                    if (GUILayout.Button("Back"))
                    {
                        _loginScreen = false;
                        _registerScreen = false;
                        errored = false;
                    }

                    username = EditorGUILayout.TextField("Username", username);


                    if (_registerScreen)
                    {
                        year = EditorGUILayout.IntField("Year", year);
                        month = EditorGUILayout.IntField("Month", month);
                        day = EditorGUILayout.IntField("Day", day);

                        if (GUILayout.Button("Register User"))
                        {
                            var dob = new DateTime(year, month, day);
                            _ = _dbe.RegisterUser(username, dob);
                        }
                    }
                    else if (_loginScreen)
                    {
                        if (GUILayout.Button("Login User"))
                        {
                            _ = _dbe.Login(username);
                        }
                    }
                }
            }

            if (_dbe.LoggedIn)
            {
                EditorGUILayout.LabelField("Session", _dbe.sessionId >= 0 ? _dbe.sessionId.ToString() : "No Session");
                if (_dbe.sessionId <= 0)
                {
                    if (GUILayout.Button("Create Session"))
                        _ = _dbe.CreateSession();
                }
                else
                {
                    // EditorGUILayout.LabelField("<b>Database connection is fully set up.</b> \nStart the game to log data.",);
                    //     new GUIStyle(EditorStyles.label) { richText = true });
                    EditorGUILayout.LabelField("<b>Database connection is fully set up.</b> Start the game to log data.", new GUIStyle(EditorStyles.label) { richText = true });
                }
                
            }
            // EditorGUILayout.Space();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Reset State"))
        {
            ResetState();
        }

        if (GUI.changed)
            EditorUtility.SetDirty(_dbe);
    }

    private void ResetState()
    {
        errored = false;
        username = "Username";
        year = 2000;
        month = 1;
        day = 1;
        _loginScreen = false;
        _registerScreen = false;
        _dbe.ResetState();
    }
}
#endif