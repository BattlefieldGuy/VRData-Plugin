using Devs.Jesper;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

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
            string filePath = Path.Combine(LoggingManager.Instance.folderPath, LoggingManager.Instance.replayFileName);
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);

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