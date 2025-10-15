using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Devs.Jesper
{
    [ExecuteInEditMode] // ensures it runs without pressing Play
    // should reset on restart of editor
    [RequireComponent(typeof(DatabaseConnection))]
    public class DatabaseEditorConnection : MonoBehaviour
    {
        [Header("Settings")]
        public string baseUrl = "http://localhost:3000";


        [Header("State (Editor Only)")]
        public bool connected;

        public bool LoggedIn => userId >= 0;

        public int userId = -1;
        public int sessionId = -1;
        public bool SetupComplete => LoggedIn && sessionId >= 0;
        
        // private void Awake()
        // {
        //     ResetState();
        // }
        private DatabaseConnection _databaseConnection;

        private void Awake()
        {
            if(_databaseConnection == null)
                _databaseConnection = GetComponent<DatabaseConnection>();
        }

        public void ResetState()
        {
            connected = false;
            userId = -1;
            sessionId = -1;
            
            Debug.Log("[EditorDb] Reset connection state");
            
            // _databaseConnection.SuccessfullyConnected = false;
        }

        public async Task<bool> TestConnection()
        {
            string url = $"{baseUrl}/ping";
            using UnityWebRequest req = UnityWebRequest.Get(url);
            await req.SendWebRequest();

            connected = req.result == UnityWebRequest.Result.Success;
            return connected;
        }

        public async Task<bool> RegisterUser(string username, DateTime dob)
        {
            string url = $"{baseUrl}/register";
            using UnityWebRequest req = new UnityWebRequest(url, "POST");
            var body = new { username, date_of_birth = dob.ToString("yyyy-MM-dd") };
            string json = JsonConvert.SerializeObject(body);
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);


            req.uploadHandler = new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

            await req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success)
            {
                var response = JsonConvert.DeserializeObject<NewUserRequestResponse>(req.downloadHandler.text);
                userId = response.userId; // or replace with real userId if returned
                Debug.Log($"[EditorDb] Registered user, id={userId}");
                return true;
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(req.downloadHandler.text);
                if (errorResponse != null)
                {
                    Debug.LogError($"<b>[EditorDb]</b> Register failed: {errorResponse.error}");
                    
                    // Debug.LogError($"[EditorDb] Register failed: {req.downloadHandler.text}");
                }
                return false;
            }
        }

        public async Task Login(string username)
        {
            string url = $"{baseUrl}/login";
            using UnityWebRequest req = new UnityWebRequest(url, "POST");
            var body = new { username };
            string json = JsonConvert.SerializeObject(body);
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            await req.SendWebRequest();
            if (req.result == UnityWebRequest.Result.Success)
            {
                var response = JsonConvert.DeserializeObject<NewUserRequestResponse>(req.downloadHandler.text);
                userId = response.userId; // or replace with real userId if returned
                Debug.Log($"[EditorDb] Logged in, id={userId}");
            }
            else
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(req.downloadHandler.text);
                if (errorResponse != null)
                {
                    Debug.LogError($"<b>[EditorDb]</b> Login failed: {errorResponse.error}");
                }
                else
                {
                    Debug.LogError($"[EditorDb] Login failed: {req.downloadHandler.text}");
                }
            }
        }

        public async Task<int?> CreateSession()
        {
            string url = $"{baseUrl}/sessions";
            var body = new { userId, metadata = "" };
            string json = JsonConvert.SerializeObject(body);
            byte[] jsonToSend = new UTF8Encoding().GetBytes(json);
            using UnityWebRequest req = new UnityWebRequest(url, "POST");
            req.uploadHandler = new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            await req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                var response = JsonConvert.DeserializeObject<NewSessionRequestResponse>(req.downloadHandler.text);
                sessionId = response.sessionId;
                Debug.Log($"[EditorDb] Created session {sessionId}");
                LoggingManager.Instance.sessionId = sessionId;
                return sessionId;
            }

            Debug.LogError($"[EditorDb] Session creation failed: {req.downloadHandler.text}");
            return null;
        }
    }

    [Serializable]
    public class NewSessionRequestResponse
    {
        public bool success;
        public int sessionId;
    }

    [Serializable]
    public class NewUserRequestResponse
    {
        public bool success;
        public int userId;
    }

    [Serializable]
    public class ErrorResponse
    {
        public bool success;
        public string error;
    }
}