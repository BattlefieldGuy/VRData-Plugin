using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace Devs.Jesper
{
    public class ApiGetExample : MonoBehaviour
    {

        public bool successfullyConnected { get; private set; } = false;
        private const string BaseUrl = "http://localhost:3000";
        public static ApiGetExample Instance;

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

        async void Start()
        {
            // StartCoroutine(GetSession(1)); // fetch session with ID = 1
            // Fetch session
            var api = this;
            return; // TESTING
            // successfullyConnected = await api.TestConnection();
            // if (!successfullyConnected)
            //     return;
            string sessionData = await api.FetchSession(1);
            Debug.Log("Session Data: " + sessionData);

            // Send some events
            var events = new List<EventData>
            {
                new EventData
                {
                    controller = "left",
                    eventType = "trigger_press",
                    timestamp = DateTime.UtcNow.ToString("o"), // ISO8601
                    controllerPositions = new { x = 0, y = 1, z = 2 }.ToString(),
                    details = new { button = "trigger" }.ToString()
                },
                new EventData
                {
                    controller = "right",
                    eventType = "grip_press",
                    timestamp = DateTime.UtcNow.ToString("o"),
                    controllerPositions = new { x = 3, y = 4, z = 5 }.ToString(),
                    details = new { button = "grip" }.ToString()
                },
                new EventData
                {
                    controller = "left",
                    eventType = "grip_release",
                    timestamp = DateTime.UtcNow.ToString("o")
                }
            };

            bool success = await api.PostEvents(1, events);
            Debug.Log("Posted events: " + success);
        }

        public async Task<string> FetchSession(int sessionId)
        {
            string url = $"{baseUrl}/sessions/{sessionId}";
            using (UnityWebRequest req = UnityWebRequest.Get(url))
            {
                await req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error fetching session: {req.error}");
                    return null;
                }

                return req.downloadHandler.text;
            }
        }

        // --- POST batch events ---
        public async Task<bool> PostEvents(int sessionId, List<EventData> events)
        {
            BatchEventsRequest body = new BatchEventsRequest
            {
                sessionId = sessionId,
                events = events,
            };

            string json = JsonUtility.ToJson(body);
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
                Debug.LogError($"Error posting events: {req.error}");
                return false;
            }

            Debug.Log($"PostEvents response: {req.downloadHandler.text}");
            return true;
        }

        public async Task TestConnection()
        {
            string url = $"{baseUrl}/ping";
            using (UnityWebRequest req = UnityWebRequest.Get(url))
            {
                await req.SendWebRequest();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error testing connection: {req.error}");
                    return;
                }

                successfullyConnected = true;
            }
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
        public string controller;             // maps to DB: controller
        public string eventType;              // maps to DB: event_type
        public string timestamp;              // maps to DB: timestamp
        public EventDetails details;          // maps to DB: details
    }

    [Serializable]
    public class EventDetails
    {
        public float[] position;  // goes inside details
        public float[] rotation;  // goes inside details
        public string button;     // goes inside details
    }

    [Serializable]
    public class BatchEventsRequest
    {
        public int sessionId;
        public List<EventData> events;
    }
