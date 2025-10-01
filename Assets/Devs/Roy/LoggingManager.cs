using System;
using System.Collections.Generic;
using System.IO;
using Devs.Jesper;
using Devs.Roy;
using Newtonsoft.Json;
using UnityEngine;
using JsonReader = Devs.Roy.JsonReader;

public class LoggingManager : MonoBehaviour
{
    public static LoggingManager Instance;

    // Internal buffer of logged events
    private readonly List<EventData> eventsBuffer = new List<EventData>();

    public string folderPath;
    public string fileName;
    public string replayFileName;
    public float autosaveFrequency = 10f;
    public string autosaveFileName = "autosave_logData.json";
    public string tempStorageFileName = "temp_logData.json";
    public int sessionId = 1; // Set dynamically if needed

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
        
#if UNITY_EDITOR
        folderPath = UnityEditor.EditorPrefs.GetString("Toolbar_FolderPath", Application.persistentDataPath);
        fileName = UnityEditor.EditorPrefs.GetString("Toolbar_FileName", "logData.json");
        replayFileName = UnityEditor.EditorPrefs.GetString("Toolbar_ReplayFileName");
#else
        folderPath = Application.persistentDataPath;
        fileName = "logData.json";
        replayFileName = fileName;
#endif
    }

    private void Start()
    {
        foreach (var evt in JsonReader.Instance.GetJsonSession().events)
        {
            Debug.Log("Controller: " +evt.controller + 
                      " EventType: " + evt.eventType + 
                      " Timestamp: " + evt.timestamp + 
                      " Position: " + evt.details.position[0]+ ","  + evt.details.position[1] + "," + evt.details.position[2] + 
                      " Rotation: " + evt.details.rotation[0]+ ","  + evt.details.rotation[1] + "," + evt.details.rotation[2] + "," + evt.details.rotation[3]);
        }
    }

    private void Update()
    {
        if (autosaveFrequency > 0 && Time.time % autosaveFrequency < 0.02f && eventsBuffer.Count > 0 && LogEntries.LogEnabled)
        {
            Autosave();
        }
    }

    // ---- API-friendly loggers ----

    public void AddEntry(Controller controller, string timestamp, Vector3 pos, Quaternion rot)
    {
        eventsBuffer.Add(new EventData
        {
            controller = controller,
            eventType = "position",
            timestamp = timestamp,
            details = new EventDetails
            {
                position = new float[] { pos.x, pos.y, pos.z },
                rotation = new float[] { rot.x, rot.y, rot.z, rot.w }
            }
        });
    }

    public void AddGenericEntry(string controller, string eventType, string timestamp, string value = null)
    {
        // tape fix
        Debug.Log(controller.ToLower());
        Controller ctrl = controller.ToLower() switch
        {
            "left" => Controller.Left,
            "right" => Controller.Right,
        };
        eventsBuffer.Add(new EventData
        {
            controller = ctrl,
            eventType = eventType,
            timestamp = timestamp,
            value = value
        });
    }

    // ---- Save + Send ----

    private void Autosave()
    {
        if (!fileName.EndsWith(".json"))
            fileName += ".json";

        string fullPath = Path.Combine(folderPath, autosaveFileName);

        var wrapper = new BatchEventsRequest
        {
            sessionId = sessionId,
            events = new List<EventData>(eventsBuffer)
        };
        var wrapperCopy =new BatchEventsRequest()
        {
            sessionId = sessionId,
            events = new List<EventData>(eventsBuffer)
        };;
        // get the old json from the temp file if it exists
        if (File.Exists(Path.Combine(folderPath, tempStorageFileName)))
        {
            string oldJson = File.ReadAllText(fullPath);
            var oldWrapper = JsonConvert.DeserializeObject<BatchEventsRequest>(oldJson);
            if (oldWrapper != null && oldWrapper.events != null)
            {
                wrapper.events.InsertRange(0, oldWrapper.events);
            }
        }

        

        string json = JsonConvert.SerializeObject(wrapper, Formatting.None);
        // overwrite the temp file with the current buffer
        File.WriteAllText(Path.Combine(folderPath, tempStorageFileName), json);
        File.WriteAllText(fullPath, json);
        Debug.Log("Autosaved " + autosaveFileName + " to: " + folderPath);

        // Also push to API
        _ = DatabaseConnection.Instance.PostEvents(sessionId,
            wrapperCopy.events); // should not have the old events as they are already on the server
        eventsBuffer.Clear();
    }

    private void OnApplicationQuit()
    {
        if (LogEntries.LogEnabled)
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

            var wrapper = new BatchEventsRequest
            {
                sessionId = sessionId,
                events = new List<EventData>(eventsBuffer)
            };

            string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
            File.WriteAllText(fullPath, json);
            Debug.Log(fileName + " written to: " + folderPath);
            // delete the temp file
            if (File.Exists(Path.Combine(folderPath, tempStorageFileName)))
                File.Delete(Path.Combine(folderPath, tempStorageFileName));
        }
    }
}