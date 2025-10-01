using Devs.Jesper;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class JsonReader : MonoBehaviour
{
    private void Start()
    {
        string filePath = Path.Combine(LoggingManager.Instance.folderPath, LoggingManager.Instance.replayFileName);
        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);
            Debug.Log("JSON Content: " + jsonContent);

            var session = JsonConvert.DeserializeObject<SessionResponse>(jsonContent);

            foreach (var evt in session.events)
            {
                Debug.Log("Controller: " + evt.controller);
                Debug.Log("X: " + evt.details.position[0] + " Y: " + evt.details.position[1] + " Z: " + evt.details.position[2]);
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
}