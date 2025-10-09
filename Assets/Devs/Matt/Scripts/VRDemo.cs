// Language: csharp
// File: `Assets/Devs/Matt/Scripts/VRDemo.cs`
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using JsonReader = Devs.Roy.JsonReader;

public class VRDemo : MonoBehaviour
{
    [Header("Head")]
    [SerializeField] private GameObject VRHeadSet;

    [Space(10)]
    [Header("Controllers")]
    [SerializeField] private GameObject RightController;
    [SerializeField] private GameObject LeftController;

    [Header("Replay Settings")]
    public float playbackSpeed = 1f;
    public bool autoStart = true;
    public bool loop = false;

    private Coroutine replayCoroutine;
    private bool isPaused = false;

    void Start()
    {
        if (autoStart)
            StartReplay();
    }

    public void StartReplay()
    {
        StopReplay();
        replayCoroutine = StartCoroutine(ReplaySession());
    }

    public void StopReplay()
    {
        if (replayCoroutine != null)
        {
            StopCoroutine(replayCoroutine);
            replayCoroutine = null;
        }
        isPaused = false;
    }

    public void PauseReplay()
    {
        isPaused = true;
    }

    public void ResumeReplay()
    {
        isPaused = false;
    }

    private IEnumerator ReplaySession()
    {
        if (JsonReader.Instance == null)
        {
            Debug.LogError("JsonReader.Instance is null.");
            yield break;
        }

        var session = JsonReader.Instance.GetJsonSession();
        if (session == null || session.events == null || session.events.Length == 0)
        {
            Debug.LogWarning("No session or events to replay.");
            yield break;
        }

        // Convert to list and sort by timestamp
        var events = new List<Devs.Jesper.ControllerEvent>(session.events);
        events.Sort((a, b) =>
        {
            if (DateTime.TryParse(a.timestamp, null, DateTimeStyles.RoundtripKind, out var da) &&
                DateTime.TryParse(b.timestamp, null, DateTimeStyles.RoundtripKind, out var db))
                return da.CompareTo(db);
            return 0;
        });

        do
        {
            DateTime? prevTime = null;
            foreach (var evt in events)
            {

                while (isPaused)
                    yield return null;


                if (!DateTime.TryParse(evt.timestamp, null, DateTimeStyles.RoundtripKind, out var curTime))
                {

                    ApplyEvent(evt);
                    yield return null;
                    continue;
                }

                if (prevTime != null)
                {
                    var delta = (curTime - prevTime.Value).TotalSeconds;
                    var wait = Mathf.Max(0f, (float)(delta / Math.Max(0.0001f, playbackSpeed)));
                    if (wait > 0f)
                    {
                        float elapsed = 0f;
                        while (elapsed < wait)
                        {

                            if (!isPaused)
                                elapsed += Time.deltaTime;
                            yield return null;
                        }
                    }
                }

                ApplyEvent(evt);
                prevTime = curTime;
            }
        } while (loop);

        replayCoroutine = null;
    }

    private void ApplyEvent(Devs.Jesper.ControllerEvent evt)
    {
        if (evt == null || string.IsNullOrEmpty(evt.eventType))
            return;


        if (!evt.eventType.Equals("position", StringComparison.OrdinalIgnoreCase))
            return;


        bool targetIsLeft = false;
        bool targetIsRight = false;

        if (int.TryParse(evt.controller, out var numeric))
        {

            targetIsLeft = numeric == 0;
            targetIsRight = numeric == 1;
        }
        else
        {
            var c = evt.controller.ToLowerInvariant();
            targetIsLeft = c.Contains("left") || c == "l";
            targetIsRight = c.Contains("right") || c == "r";
        }


        if (evt.details != null)
        {
            var pos = evt.details.position;
            var rot = evt.details.rotation;
            if (pos != null && pos.Length >= 3)
            {
                var position = new Vector3(pos[0], pos[1], pos[2]);
                if (targetIsLeft && LeftController != null)
                    LeftController.transform.localPosition = position;
                else if (targetIsRight && RightController != null)
                    RightController.transform.localPosition = position;
            }

            if (rot != null && rot.Length >= 4)
            {
                var rotation = new Quaternion(rot[0], rot[1], rot[2], rot[3]);
                if (targetIsLeft && LeftController != null)
                    LeftController.transform.localRotation = rotation;
                else if (targetIsRight && RightController != null)
                    RightController.transform.localRotation = rotation;
            }
        }
    }
}
