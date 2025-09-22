using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class ApiClient : MonoBehaviour
{
    [SerializeField] private string apiUrl = "http://localhost:3000/positions";
    public static ApiClient Instance;

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

    public void UploadJson(string filePath, int sessionId)
    {
        StartCoroutine(UploadJsonCoroutine(filePath, sessionId));
    }

    private IEnumerator UploadJsonCoroutine(string filePath, int sessionId)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            yield break;
        }

        string jsonData = File.ReadAllText(filePath);
        var jsonObject = JsonUtility.FromJson<JsonWrapper>(jsonData);
        jsonObject.sessionId = sessionId;
        string updatedJson = JsonUtility.ToJson(jsonObject);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(updatedJson);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Upload successful: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Upload failed: " + request.error);
        }
    }

    [System.Serializable]
    public class JsonWrapper
    {
        public int sessionId;
        public Position[] positions;
    }

    [System.Serializable]
    public class Position
    {
        public string controller;
        public string timestamp;
        public float[] pos;
        public float[] rot;
    }
}