using UnityEngine;
using UnityEditor;
using System.IO;

public class Toolbar : EditorWindow
{
    public string folderPath = "";
    public string fileName = "logData.json";

    public string replayFileName;


    private const string FolderKey = "Toolbar_FolderPath";
    private const string FileKey = "Toolbar_FileName";
    private const string ReplayKey = "Toolbar_ReplayFileName";

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
        
        GUILayout.Space(10);

        string newFileName = EditorGUILayout.TextField("File Name:", fileName);
        if (newFileName != fileName)
        {
            fileName = newFileName;
            SaveSettings();
        }
        
        GUILayout.Space(10);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Replay File:", GUILayout.Width(70));
        EditorGUILayout.SelectableLabel(replayFileName, EditorStyles.textField, GUILayout.Height(18));
        if (GUILayout.Button("Browse", GUILayout.Width(70)))
        {
            string filePath = EditorUtility.OpenFilePanel(
                "Select Replay File",
                folderPath,
                "json"
            );
            if (!string.IsNullOrEmpty(filePath))
            {
                replayFileName = Path.GetFileName(filePath);
                SaveSettings();
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
    }

    private void SaveSettings()
    {
        EditorPrefs.SetString(FolderKey, folderPath);
        EditorPrefs.SetString(FileKey, fileName);
        EditorPrefs.SetString(ReplayKey, replayFileName);
    }

    private void LoadSettings()
    {
        folderPath = EditorPrefs.GetString(FolderKey, "");
        fileName = EditorPrefs.GetString(FileKey, "logData.json");
        replayFileName = EditorPrefs.GetString(ReplayKey, "");
    }
}