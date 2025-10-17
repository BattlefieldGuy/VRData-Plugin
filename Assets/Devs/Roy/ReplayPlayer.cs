// Language: csharp
using Devs.Jesper;
using Devs.Roy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class ReplayPlayer : MonoBehaviour
{
    [Tooltip("Assign controller GameObjects (transforms will be updated during replay)")]
    public Transform LeftController;
    public Transform RightController;
    public Transform HMD;
    public Transform Body;

    [Tooltip("Playback speed multiplier")]
    public float playbackSpeed = 1f;

    [Tooltip("Loop playback when finished")]
    public bool loop = false;

    public bool IsPlaying { get; private set; } = false;
    public float CurrentTimeSeconds { get; private set; } = 0f;
    public float DurationSeconds { get; private set; } = 0f;

    private List<ControllerEvent> events = new List<ControllerEvent>();
    private List<DateTime> eventTimes = new List<DateTime>();
    private DateTime sessionStart = DateTime.MinValue;
    private int nextEventIndex = 0;
    private Coroutine playbackCoroutine;

    private void Start()
    {
        LoadSession();
    }

    public void LoadSession()
    {
        var session = JsonReader.Instance.GetJsonSession();
        if (session == null || session.events == null || session.events.Length == 0)
        {
            Debug.LogWarning("ReplayPlayer: No session/events found.");
            events.Clear();
            DurationSeconds = 0;
            return;
        }

        events = session.events
            .Where(e => e != null && !string.IsNullOrEmpty(e.timestamp))
            .OrderBy(e => e.timestamp)
            .ToList();

        eventTimes = events.Select(e => DateTime.Parse(e.timestamp, null, DateTimeStyles.RoundtripKind)).ToList();

        sessionStart = eventTimes.Min();
        var sessionEnd = eventTimes.Max();
        DurationSeconds = (float)(sessionEnd - sessionStart).TotalSeconds;
        CurrentTimeSeconds = 0f;
        nextEventIndex = 0;
        IsPlaying = false;
    }

    public void Play()
    {
        if (events == null || events.Count == 0) LoadSession();
        if (playbackCoroutine != null) StopCoroutine(playbackCoroutine);
        IsPlaying = true;
        playbackCoroutine = StartCoroutine(PlaybackRoutine());
    }

    public void Pause()
    {
        IsPlaying = false;
        if (playbackCoroutine != null)
        {
            StopCoroutine(playbackCoroutine);
            playbackCoroutine = null;
        }
    }

    public void StopPlayback()
    {
        Pause();
        Seek(0f);
    }

    public void SetSpeed(float speed)
    {
        playbackSpeed = Mathf.Max(0.01f, speed);
    }

    public void Seek(float seconds)
    {
        if (events == null || events.Count == 0) return;

        CurrentTimeSeconds = Mathf.Clamp(seconds, 0f, DurationSeconds);
        DateTime target = sessionStart.AddSeconds(CurrentTimeSeconds);
        nextEventIndex = eventTimes.FindIndex(t => t > target);
        if (nextEventIndex == -1) nextEventIndex = events.Count;
        ApplyLatestStateBefore(target);
    }

    private IEnumerator PlaybackRoutine()
    {
        while (IsPlaying)
        {
            float dt = Time.deltaTime * playbackSpeed;
            CurrentTimeSeconds += dt;
            DateTime now = sessionStart.AddSeconds(CurrentTimeSeconds);


            while (nextEventIndex < events.Count && eventTimes[nextEventIndex] <= now)
            {
                ApplyEvent(events[nextEventIndex]);
                nextEventIndex++;
            }

            if (CurrentTimeSeconds >= DurationSeconds)
            {
                if (loop)
                {
                    Seek(0f);
                }
                else
                {
                    IsPlaying = false;
                    playbackCoroutine = null;
                    yield break;
                }
            }

            yield return null;
        }
    }

    private void ApplyLatestStateBefore(DateTime target)
    {
        ControllerEvent lastLeft = null;
        ControllerEvent lastRight = null;
        ControllerEvent lastHead = null;

        for (int i = 0; i < events.Count; i++)
        {
            if (eventTimes[i] > target) break;
            var e = events[i];
            if (e.eventType != null && e.eventType.Equals("position", StringComparison.OrdinalIgnoreCase))
            {
                string controllerStr = e.controller.ToString();
                if (IsLeftController(controllerStr))
                    lastLeft = e;
                else if (IsRightController(controllerStr))
                    lastRight = e;
                else if (IsHeadController(controllerStr))
                    lastHead = e;
            }
        }

        if (lastLeft != null) ApplyEvent(lastLeft);
        if (lastRight != null) ApplyEvent(lastRight);
        if (lastHead != null) ApplyEvent(lastHead);
    }

    private bool IsLeftController(string controllerValue)
    {
        if (int.TryParse(controllerValue, out var numeric))
            return numeric == 0;
        return controllerValue.Equals("left", StringComparison.OrdinalIgnoreCase) || controllerValue.Equals(Controller.Left.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    private bool IsRightController(string controllerValue)
    {
        if (int.TryParse(controllerValue, out var numeric))
            return numeric == 1;
        return controllerValue.Equals("right", StringComparison.OrdinalIgnoreCase) || controllerValue.Equals(Controller.Right.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    private bool IsHeadController(string controllerValue)
    {
        if (int.TryParse(controllerValue, out var numeric))
        {
            return numeric == 2;
        }
        return controllerValue.Equals("head", StringComparison.OrdinalIgnoreCase) || controllerValue.Equals(Controller.Head.ToString(), StringComparison.OrdinalIgnoreCase);
    }
    
    private bool IsBodyController(string controllerValue)
    {
        if (int.TryParse(controllerValue, out var numeric))
        {
            return numeric == 2;
        }
        return controllerValue.Equals("body", StringComparison.OrdinalIgnoreCase) || controllerValue.Equals(Controller.Body.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    private void ApplyEvent(ControllerEvent evt)
    {
        if (evt == null || evt.details == null) return;

        bool targetIsLeft = IsLeftController(evt.controller);
        bool targetIsRight = IsRightController(evt.controller);
        bool targetIsHead = IsHeadController(evt.controller);
        bool targetIsBody = IsBodyController(evt.controller);


        if (evt.details.position != null && evt.details.position.Length >= 3)
        {
            var pos = new Vector3(evt.details.position[0], evt.details.position[1], evt.details.position[2]);
            if (targetIsLeft && LeftController != null)
                LeftController.localPosition = pos;
            else if (targetIsRight && RightController != null)
                RightController.localPosition = pos;
            else if (targetIsHead && HMD != null)
            {
                HMD.position = pos;
            }
            else if (targetIsBody && Body != null)
            {
                Body.position = pos;
            }
        }


        if (evt.details.rotation != null && evt.details.rotation.Length >= 4)
        {
            var rot = new Quaternion(evt.details.rotation[0], evt.details.rotation[1], evt.details.rotation[2], evt.details.rotation[3]);
            if (targetIsLeft && LeftController != null)
                LeftController.localRotation = rot;
            else if (targetIsRight && RightController != null)
                RightController.localRotation = rot;
            else if (targetIsHead && HMD != null)
                HMD.rotation = rot;
            else if (targetIsBody && Body != null)
                Body.rotation = rot;
        }

    }
}
