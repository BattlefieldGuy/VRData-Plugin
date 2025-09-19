using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Devs.Jesper
{
    public class ApiGetExample : MonoBehaviour
    {
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
            string sessionData = await api.FetchSession(1);
            Debug.Log("Session Data: " + sessionData);
        }

        public async Task<string> FetchSession(int sessionId)
        {
            string url = $"{BaseUrl}/sessions/{sessionId}";
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
                events = events
            };

            string json = JsonUtility.ToJson(body);

            using (UnityWebRequest req = new UnityWebRequest($"{BaseUrl}/events", "POST"))
            {
                byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
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
        }
    }
}


    [Serializable]
    public class EventDetails
    {
        public float[] position;
        public float[] rotation;
        public string button;
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
        public string controller;
        public string eventType;
        public string timestamp; // ISO8601 string is easiest
        public float[] controllerPositions; // could be Vector3, etc., adjust to match server
        public EventDetails details;
    }

    [Serializable]
    public class BatchEventsRequest
    {
        public int sessionId;
        public List<EventData> events;
    }
