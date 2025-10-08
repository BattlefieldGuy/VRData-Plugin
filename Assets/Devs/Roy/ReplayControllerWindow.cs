// Language: csharp
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ReplayControllerWindow : EditorWindow
{
    private ReplayPlayer player;
    private float sliderTime = 0f;
    private bool playingLast = false;

    [MenuItem("Tools/Replay Controller")]
    public static void ShowWindow()
    {
        GetWindow<ReplayControllerWindow>("Replay Controller");
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        FindPlayer();
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
            FindPlayer();
    }

    private void FindPlayer()
    {
        player = Object.FindObjectOfType<ReplayPlayer>();
        if (player != null)
        {
            sliderTime = player.CurrentTimeSeconds;
        }
    }

    private void OnGUI()
    {
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Enter Play Mode to control replay", MessageType.Info);
            if (GUILayout.Button("Find ReplayPlayer"))
                FindPlayer();
            return;
        }

        if (player == null)
        {
            EditorGUILayout.HelpBox("No ReplayPlayer in scene. Add `ReplayPlayer` to a GameObject and assign controllers.", MessageType.Warning);
            if (GUILayout.Button("Find ReplayPlayer"))
                FindPlayer();
            return;
        }

        EditorGUILayout.LabelField("Replay Controller", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Speed
        float newSpeed = EditorGUILayout.FloatField("Speed", player.playbackSpeed);
        if (!Mathf.Approximately(newSpeed, player.playbackSpeed))
        {
            Undo.RecordObject(player, "Change Playback Speed");
            player.SetSpeed(newSpeed);
        }

        // Loop
        bool newLoop = EditorGUILayout.Toggle("Loop", player.loop);
        if (newLoop != player.loop)
        {
            Undo.RecordObject(player, "Toggle Loop");
            player.loop = newLoop;
        }

        // Time slider
        float duration = Mathf.Max(0.0001f, player.DurationSeconds);
        sliderTime = EditorGUILayout.Slider("Time", player.CurrentTimeSeconds, 0f, duration);
        if (!Mathf.Approximately(sliderTime, player.CurrentTimeSeconds))
        {
            player.Seek(sliderTime);
            Repaint();
        }

        // Time labels
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"Current: {player.CurrentTimeSeconds:F2}s", GUILayout.Width(150));
        EditorGUILayout.LabelField($"Duration: {duration:F2}s");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Play / Pause / Stop buttons
        EditorGUILayout.BeginHorizontal();
        if (player.IsPlaying)
        {
            if (GUILayout.Button("Pause"))
            {
                player.Pause();
            }
        }
        else
        {
            if (GUILayout.Button("Play"))
            {
                player.Play();
            }
        }

        if (GUILayout.Button("Stop"))
        {
            player.StopPlayback();
        }

        if (GUILayout.Button("Seek to Start"))
        {
            player.Seek(0f);
        }
        EditorGUILayout.EndHorizontal();

        // Refresh reference
        if (GUILayout.Button("Refresh Player Reference"))
            FindPlayer();

       
        if (player.IsPlaying != playingLast)
        {
            playingLast = player.IsPlaying;
            Repaint();
        }

        
        if (player.IsPlaying)
            Repaint();
    }
}
#endif
