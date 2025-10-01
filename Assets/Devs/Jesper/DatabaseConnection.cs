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
    public class DatabaseConnection : MonoBehaviour
    {
        public string baseUrl = "http://localhost:3000";
        public bool SuccessfullyConnected { get; private set; } = false;
        public int userId = -1; // needs to be set via login
        public static DatabaseConnection Instance;

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

        private async void Start()
        {
            if (!SuccessfullyConnected || !LogEntries.LogEnabled)
            {
                if (LogEntries.LogEnabled)
                    Debug.LogWarning(
                        "Not connected to the database! Please check the inspector settings. Data will not be logged. Disabling script...");
                enabled = false;
                return;
            }

            // await TestConnection();
            // if (SuccessfullyConnected)
            // {
            //     Debug.Log("Successfully connected to the database.");
            // }
            // else
            // {
            //     Debug.LogError("Failed to connect to the database.");
            // }
            // create a new session
            var newSessionid = await CreateSession();
            if (newSessionid != null)
                LoggingManager.Instance.sessionId = (int)newSessionid;
            else
                Debug.LogError("Failed to create a new session.");
        }

        private async Task<int?> CreateSession()
        {
            if (!enabled)
                return null;
            string url = $"{baseUrl}/sessions";
            using UnityWebRequest req = new UnityWebRequest(url, "POST");

            // req.uploadHandler = new UploadHandlerRaw();
            req.downloadHandler = new DownloadHandlerBuffer();


            await req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error creating session: {req.downloadHandler.text}");
                return null;
            }

            var response = JsonConvert.DeserializeObject<NewSessionRequestResponse>(req.downloadHandler.text);
            return response.sessionId;
        }

        public async Task<string> FetchSession(int sessionId)
        {
            if (!enabled)
                return null;
            string url = $"{baseUrl}/sessions/{sessionId}";
            using (UnityWebRequest req = UnityWebRequest.Get(url))
            {
                await req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error fetching session: {req.downloadHandler.text}");
                    return null;
                }

                return req.downloadHandler.text;
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

        public async Task TestConnection()
        {
            if (!enabled)
                return;
            string url = $"{baseUrl}/ping";
            using UnityWebRequest req = UnityWebRequest.Get(url);
            await req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error testing connection: {req.error}");
                return;
            }

            SuccessfullyConnected = true;
        }
        // public async Task<int> LoginUser()

        public async Task RegisterUser(string username1, DateTime dateOfBirth)
        {
            string url = $"{baseUrl}/register";
            using UnityWebRequest req = new UnityWebRequest(url, "POST");
            var body = new
            {
                username = username1,
                dateOfBirth = dateOfBirth.ToString("yyyy-MM-dd")
            };
            string json = JsonConvert.SerializeObject(body);
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = new DownloadHandlerBuffer();

            await req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error registering user: {req.downloadHandler.text}");
                return;
            }

            Debug.Log($"User registered successfully: {req.downloadHandler.text}");
            var response = JsonConvert.DeserializeObject<NewSessionRequestResponse>(req.downloadHandler.text);
            userId = response.sessionId;
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
    public class NewSessionRequestResponse
    {
        public bool success;
        public int sessionId;
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
        Right
    }
}