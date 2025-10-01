using System;
using Devs.Jesper;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace Devs.Roy
{
    public class JsonReader : MonoBehaviour
    {
        public static JsonReader Instance;
        private SessionResponse session;

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
            string filePath = Path.Combine(LoggingManager.Instance.folderPath, LoggingManager.Instance.replayFileName);
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                Debug.Log("JSON Content: " + jsonContent);

                session = JsonConvert.DeserializeObject<SessionResponse>(jsonContent);
            }
            else
            {
                Debug.LogError("File not found: " + filePath);
            }
        }

        public SessionResponse GetJsonSession()
        {
            return session;
        }
    }
}