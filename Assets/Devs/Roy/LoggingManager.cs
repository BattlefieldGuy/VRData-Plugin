using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class LoggingManager : MonoBehaviour
{
    public static LoggingManager Instance;
    private List<Dictionary<string, object>> logEntries = new List<Dictionary<string, object>>();
    private Toolbar toolbar;
    public string folderPath;
    public string fileName;
    [Tooltip("Frequency in seconds for autosaving log data. Set to 0 to disable autosave. Setting autosave frequency to a low value may impact performance depending on the frequency of log entries.")]
    public float autosaveFrequency = 10f;
    public string autosaveFileName = "autosave_logData.json";

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

    private void Update()
    {
        if (Time.time % autosaveFrequency < 0.02f && logEntries.Count > 0)
        {
            Autosave();
        }

    }

    private object ToSerializable(object value)
    {
        if (value is Vector3 v)
            return v.ToString(); // or $"{v.x},{v.y},{v.z}"
        if (value is Quaternion q)
            return q.ToString(); // or $"{q.x},{q.y},{q.z},{q.w}"
        // Add more Unity types as needed
        return value;
    }

    public void AddEntry(string eventName, Dictionary<string, object> entries)
    {
        string log = eventName + "\n";
        var entry = new Dictionary<string, object>();
        entry["Event"] = eventName;
        foreach (var kvp in entries)
        {
            var serializableValue = ToSerializable(kvp.Value);
            log += $"{kvp.Key}: {serializableValue}\n";
            entry[kvp.Key] = serializableValue;
        }
        logEntries.Add(entry);
    }

    public void RemoveEntry(Dictionary<string, object> entry)
    {
        logEntries.Remove(entry);
    }

    private void Autosave()
    {
        if (!fileName.EndsWith(".json"))
            fileName += ".json";

        string fullPath = Path.Combine(folderPath, autosaveFileName);
        string json = JsonConvert.SerializeObject(logEntries, Formatting.Indented);
        File.WriteAllText(fullPath, json);
        Debug.Log("Autosaved " + fileName + " to: " + folderPath);
    }

    void OnApplicationQuit()
    {
        if (!fileName.EndsWith(".json"))
            fileName += ".json";

        string fullPath = Path.Combine(folderPath, fileName);
        string baseName = Path.GetFileNameWithoutExtension(fileName);
        string ext = Path.GetExtension(fileName);
        int count = 1;

        while (File.Exists(fullPath))
        {
            string tempFileName = $"{baseName}({count}){ext}";
            fullPath = Path.Combine(folderPath, tempFileName);
            count++;
        }

        string json = JsonConvert.SerializeObject(logEntries, Formatting.Indented);
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