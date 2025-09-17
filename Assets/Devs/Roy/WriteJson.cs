using System;
using System.IO;
using UnityEngine;

public class WriteJson : MonoBehaviour
{
    public float logFrequencyHz = 10f;
    private float logInterval => 1f / logFrequencyHz;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= logInterval)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "event", "Player Position" },
                { "position", transform.position }
            });
            timer = 0f;
        }
    }
}