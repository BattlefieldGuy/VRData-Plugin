using System;
using UnityEngine;

public class LogEntries : MonoBehaviour
{
    [Tooltip("Times per second for logging player position")]
    public float logFrequency = 10f;
    private float logInterval => 1f / logFrequency;
    private float timer = 0f;
    private float triggerThreshold = 0.3f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= logInterval)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Controller Position" },
                { "Left Controller Position", global::InputDataRecord.Instance.PositionL }
            });

            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Controller Position" },
                { "Right Controller Position", InputDataRecord.Instance.PositionR }
            });

            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Controller Rotation" },
                { "Left Controller Rotation", InputDataRecord.Instance.RotationL }
            });

            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Controller Rotation" },
                { "Right Controller Rotation", InputDataRecord.Instance.RotationR }
            });
            timer = 0f;
        }

        if (InputDataRecord.Instance.GripL >= triggerThreshold)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Left Grip" },
                { "Left Grip Pressed", InputDataRecord.Instance.GripL }
            });
        }

        if (InputDataRecord.Instance.GripL >= triggerThreshold)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Right Grip" },
                { "Right Grip Pressed", InputDataRecord.Instance.GripR }
            });
        }

        if (InputDataRecord.Instance.TriggerL >= triggerThreshold)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Left Trigger" },
                { "Left Trigger Pressed", InputDataRecord.Instance.TriggerL }
            });
        }

        if (InputDataRecord.Instance.TriggerR >= triggerThreshold)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Right Trigger" },
                { "Right Trigger Pressed", InputDataRecord.Instance.TriggerR }
            });
        }

        if (InputDataRecord.Instance.JoystickL != Vector2.zero)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Left Joystick" },
                { "Right Joystick Moved", InputDataRecord.Instance.JoystickL }
            });
        }

        if (InputDataRecord.Instance.JoystickR != Vector2.zero)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Right Joystick" },
                { "Right Joystick Moved", InputDataRecord.Instance.JoystickR }
            });
        }

        if (InputDataRecord.Instance.Ybutton)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Y Button" },
                { "Y Button Pressed", InputDataRecord.Instance.Ybutton }
            });
        }

        if (InputDataRecord.Instance.Xbutton)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "X Button" },
                { "X Button Pressed", InputDataRecord.Instance.Xbutton }
            });
        }

        if (InputDataRecord.Instance.Bbutton)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "B Button" },
                { "B Button Pressed", InputDataRecord.Instance.Bbutton }
            });
        }

        if (InputDataRecord.Instance.Abutton)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "A Button" },
                { "A Button Pressed", InputDataRecord.Instance.Abutton }
            });
        }
    }
}