using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LoggingManager : MonoBehaviour
{
    public static LoggingManager Instance;
    private List<Dictionary<string, object>> logEntries = new List<Dictionary<string, object>>();
    private Toolbar toolbar;
    public string folderPath;
    public string fileName;
    private Vector3 position = new Vector3(0, 0, 0);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    #if UNITY_EDITOR
            folderPath = UnityEditor.EditorPrefs.GetString("Toolbar_FolderPath", Application.persistentDataPath);
            fileName = UnityEditor.EditorPrefs.GetString("Toolbar_FileName", "logData.json");
    #else
        folderPath = Application.persistentDataPath;
        fileName = "logData.json";
    #endif
    }

    public void AddEntry(Dictionary<string, object> entry)
    {
        logEntries.Add(entry);
    }

    public void RemoveEntry(Dictionary<string, object> entry)
    {
        logEntries.Remove(entry);
    }

    void OnApplicationQuit()
    {
        
        // Convert entries to a serializable format
        var serializableEntries = new List<SerializableEntry>();
        foreach (var entry in logEntries)
        {
            serializableEntries.Add(new SerializableEntry(entry));
        }
        
        // Ensure file ends with .json
        if (!fileName.EndsWith(".json"))
        {
            fileName += ".json";
        }

        string fullPath = Path.Combine(folderPath, fileName);
        string baseName = Path.GetFileNameWithoutExtension(fileName);
        string ext = Path.GetExtension(fileName);
        int count = 1;

        // Find a unique file name
        while (File.Exists(fullPath))
        {
            string tempFileName = $"{baseName}({count}){ext}";
            fullPath = Path.Combine(folderPath, tempFileName);
            count++;
        }
        
        string json = JsonUtility.ToJson(new SerializableEntryList { entries = serializableEntries }, true);
        File.WriteAllText(fullPath, json);
        Debug.Log(fileName + " written to: " + folderPath);
    }

    [System.Serializable]
    private class SerializableEntry
    {
        public List<string> keys = new List<string>();
        public List<string> values = new List<string>();

        public SerializableEntry(Dictionary<string, object> dict)
        {
            foreach (var kvp in dict)
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value != null ? kvp.Value.ToString() : "null");
            }
        }
    }

    [System.Serializable]
    private class SerializableEntryList
    {
        public List<SerializableEntry> entries;
    }
}