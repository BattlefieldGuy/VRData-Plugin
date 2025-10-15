using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Devs.Roy;
using Newtonsoft.Json;

namespace Devs.Jesper
{
    [RequireComponent(typeof(DatabaseEditorConnection))]
    public class DatabaseConnection : MonoBehaviour
    {
        public string baseUrl = "http://localhost:3000";
        // public bool SuccessfullyConnected { get; set; } = false;
        [HideInInspector]
        public int userId = -1; // needs to be set via login
        public static DatabaseConnection Instance;
        private DatabaseEditorConnection _dbe;

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
            _dbe = GetComponent<DatabaseEditorConnection>();
        }

        private void Start()
        {
            if (!_dbe.SetupComplete || !LogEntries.LogEnabled)
            {
                if (LogEntries.LogEnabled)
                    Debug.LogWarning(
                        "<b> Not connected to the database! </b> Please check the inspector settings. Data will only be logged locally. Disabling script...");
                enabled = false;
                return;
            }
        }

        // --- POST batch events ---
        public async Task<bool> PostEvents(int sessionId, List<EventData> events)
        {
            if (!enabled)
                return false;
            var body = new BatchEventsRequest
            {
                sessionId = sessionId,
                events = events,
            };

            string json2 = JsonConvert.SerializeObject(body,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            using UnityWebRequest req = new UnityWebRequest($"{baseUrl}/events", "POST");

            byte[] jsonToSend = new UTF8Encoding().GetBytes(json2);
            req.uploadHandler = new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            await req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error posting events: {req.downloadHandler.text}");
                return false;
            }

            Debug.Log($"PostEvents response: {req.downloadHandler.text}");
            return true;
        }
    }


    [Serializable]
    public class ControllerEvent
    {
        public int id;
        public int sessionId;
        public string controller;
        public string eventType;
        public string timestamp;
        public EventDetails details;
    }

    [Serializable]
    public class SessionResponse
    {
        public int sessionId;
        public ControllerEvent[] events;
    }

    [Serializable]
    public class EventData
    {
        // Required
        public Controller controller;
        public string eventType;
        public string timestamp; // ISO8601 string is easiest
        public string value; // optional value field
        public EventDetails details;
    }

    [Serializable]
    public class EventDetails
    {
        public float[] position; // goes inside details
        public float[] rotation; // goes inside details
        public string button; // goes inside details
    }

    [Serializable]
    public class BatchEventsRequest
    {
        public int sessionId;
        public List<EventData> events;
    }

    public enum Controller
    {
        Left,
        Right,
        Head
    }
}