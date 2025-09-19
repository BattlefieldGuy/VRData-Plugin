using System.Collections.Generic;
using System.IO;
using Devs.Jesper;
using Newtonsoft.Json;
using UnityEngine;

public class LoggingManager : MonoBehaviour
{
    public static LoggingManager Instance;
    private List<ApiClient.Position> positions = new List<ApiClient.Position>();
    public string folderPath;
    public string fileName;
    public float autosaveFrequency = 10f;
    public string autosaveFileName = "autosave_logData.json";
    public int sessionId = 1; // Set or generate as needed

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
    
    public List<EventData> ToEventDataList()
    {
        var list = new List<EventData>();
        foreach (var pos in positions)
        {
            bool isPosition = pos.pos != null && pos.rot != null && pos.pos.Length == 3 && pos.rot.Length == 4;
            list.Add(new EventData
            {
                controller = pos.controller,
                eventType = isPosition ? "position" : "event",
                timestamp = pos.timestamp,
                controllerPositions = isPosition ? pos.pos : null,
                details = isPosition
                    ? new EventDetails { rotation = pos.rot }
                    : new EventDetails { button = "Pressed" } // Or set appropriately for button events
            });
        }
        return list;
    }
    
    public void AddGenericEntry(string controller, string timestamp, string state)
    {
        positions.Add(new ApiClient.Position
        {
            controller = controller,
            timestamp = timestamp,
            pos = new float[3], // or null if not applicable
            rot = new float[4]  // or null if not applicable
        });
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
        if (autosaveFrequency > 0 && Time.time % autosaveFrequency < 0.02f && positions.Count > 0)
        {
            Autosave();
        }
    }

    public void AddEntry(string controller, string timestamp, Vector3 pos, Quaternion rot)
    {
        positions.Add(new ApiClient.Position
        {
            controller = controller,
            timestamp = timestamp,
            pos = new float[] { pos.x, pos.y, pos.z },
            rot = new float[] { rot.x, rot.y, rot.z, rot.w }
        });
    }

    private void Autosave()
    {
        if (!fileName.EndsWith(".json"))
            fileName += ".json";

        string fullPath = Path.Combine(folderPath, autosaveFileName);
        var wrapper = new ApiClient.JsonWrapper
        {
            sessionId = sessionId,
            positions = positions.ToArray()
        };
        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
        File.WriteAllText(fullPath, json);
        Debug.Log("Autosaved " + autosaveFileName + " to: " + folderPath);

        var events = ToEventDataList();
        _ = ApiGetExample.Instance.PostEvents(sessionId, events);
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

        var wrapper = new ApiClient.JsonWrapper
        {
            sessionId = sessionId,
            positions = positions.ToArray()
        };
        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
        File.WriteAllText(fullPath, json);
        Debug.Log(fileName + " written to: " + folderPath);
    }
}