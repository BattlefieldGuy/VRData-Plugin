using UnityEngine;
using UnityEditor;
using System.IO;

public class Toolbar : EditorWindow
{
    public string folderPath = "";
    public string fileName = "logData.json";

    private bool logInfo = true;
    private bool logWarnings = true;
    private bool logErrors = true;

    private const string FolderKey = "Toolbar_FolderPath";
    private const string FileKey = "Toolbar_FileName";
    private const string InfoKey = "Toolbar_LogInfo";
    private const string WarnKey = "Toolbar_LogWarnings";
    private const string ErrorKey = "Toolbar_LogErrors";

    [MenuItem("Tools/Logging Settings")]
    public static void ShowWindow()
    {
        GetWindow<Toolbar>("Logging Settings");
    }

    private void OnEnable()
    {
        LoadSettings();
    }

    private void OnGUI()
    {
        GUILayout.Label("Logging Configuration", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Folder:", GUILayout.Width(50));
        EditorGUILayout.SelectableLabel(folderPath, EditorStyles.textField, GUILayout.Height(18));
        if (GUILayout.Button("Browse", GUILayout.Width(70)))
        {
            string path = EditorUtility.OpenFolderPanel(
                "Select Folder for Logs",
                Application.persistentDataPath,
                ""
            );
            if (!string.IsNullOrEmpty(path))
            {
                folderPath = path;
                SaveSettings();
            }
        }
        EditorGUILayout.EndHorizontal();

        string newFileName = EditorGUILayout.TextField("File Name:", fileName);
        if (newFileName != fileName)
        {
            fileName = newFileName;
            SaveSettings();
        }

        GUILayout.Space(10);

        bool newLogInfo = EditorGUILayout.Toggle("Log Info", logInfo);
        if (newLogInfo != logInfo)
        {
            logInfo = newLogInfo;
            SaveSettings();
        }

        bool newLogWarnings = EditorGUILayout.Toggle("Log Warnings", logWarnings);
        if (newLogWarnings != logWarnings)
        {
            logWarnings = newLogWarnings;
            SaveSettings();
        }

        bool newLogErrors = EditorGUILayout.Toggle("Log Errors", logErrors);
        if (newLogErrors != logErrors)
        {
            logErrors = newLogErrors;
            SaveSettings();
        }

        GUILayout.Space(10);
    }

    private void SaveSettings()
    {
        EditorPrefs.SetString(FolderKey, folderPath);
        EditorPrefs.SetString(FileKey, fileName);
        EditorPrefs.SetBool(InfoKey, logInfo);
        EditorPrefs.SetBool(WarnKey, logWarnings);
        EditorPrefs.SetBool(ErrorKey, logErrors);
    }

    private void LoadSettings()
    {
        folderPath = EditorPrefs.GetString(FolderKey, "");
        fileName = EditorPrefs.GetString(FileKey, "logData.json");
        logInfo = EditorPrefs.GetBool(InfoKey, true);
        logWarnings = EditorPrefs.GetBool(WarnKey, true);
        logErrors = EditorPrefs.GetBool(ErrorKey, true);
    }
}