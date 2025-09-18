using System;
using System.Collections.Generic;
using UnityEngine;

public class LogEntries : MonoBehaviour
{
    public bool toggleLogging = true;
    
    [Tooltip("Times per second for logging player position")]
    public float logFrequency = 10f;
    private float logInterval => 1f / logFrequency;
    private float timer = 0f;
    [Tooltip("Threshold for considering trigger or grip as pressed")]
    public float triggerThreshold = 0.3f;

    [Tooltip("Deadzone for joystick movement")]
    public float deadzone = 0.5f;
    

    private void Update()
    {
        if (toggleLogging)
        {
            timer += Time.deltaTime;
            if (timer >= logInterval)
            {
                LoggingManager.Instance.AddEntry(
                    "Left Controller",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Position", InputDataRecord.Instance.PositionL },
                        { "Rotation", InputDataRecord.Instance.RotationL }
                    }
                );

                LoggingManager.Instance.AddEntry(
                    "Right Controller",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Position", InputDataRecord.Instance.PositionL },
                        { "Rotation", InputDataRecord.Instance.RotationL }
                    }
                );
                timer = 0f;
            }

            if (InputDataRecord.Instance.GripL >= triggerThreshold)
            {
                LoggingManager.Instance.AddEntry(
                    "Left Grip",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Triggered", InputDataRecord.Instance.GripL }
                    }
                );
            }

            if (InputDataRecord.Instance.GripL >= triggerThreshold)
            {
                LoggingManager.Instance.AddEntry(
                    "Right Grip",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Triggered", InputDataRecord.Instance.GripR }
                    }
                );
            }

            if (InputDataRecord.Instance.TriggerL >= triggerThreshold)
            {
                LoggingManager.Instance.AddEntry(
                    "Left Trigger",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Triggered", InputDataRecord.Instance.TriggerL }
                    }
                );
            }

            if (InputDataRecord.Instance.TriggerR >= triggerThreshold)
            {
                LoggingManager.Instance.AddEntry(
                    "Left Trigger",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Triggered", InputDataRecord.Instance.TriggerL }
                    }
                );
            }

            if (InputDataRecord.Instance.JoystickL.x >= deadzone || InputDataRecord.Instance.JoystickL.y >= deadzone)
            {
                LoggingManager.Instance.AddEntry(
                    "Left Joystick",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", InputDataRecord.Instance.JoystickL }
                    }
                );
            }

            if (InputDataRecord.Instance.JoystickR.x >= deadzone || InputDataRecord.Instance.JoystickR.y >= deadzone)
            {
                LoggingManager.Instance.AddEntry(
                    "Right Joystick",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", InputDataRecord.Instance.JoystickR }
                    }
                );
            }

            if (InputDataRecord.Instance.Ybutton)
            {
                LoggingManager.Instance.AddEntry(
                    "Y Button",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", InputDataRecord.Instance.Ybutton }
                    }
                );
            }

            if (InputDataRecord.Instance.Xbutton)
            {
                LoggingManager.Instance.AddEntry(
                    "X Button",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", InputDataRecord.Instance.Xbutton }
                    }
                );
            }

            if (InputDataRecord.Instance.Bbutton)
            {
                LoggingManager.Instance.AddEntry(
                    "B Button",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", InputDataRecord.Instance.Bbutton }
                    }
                );
            }

            if (InputDataRecord.Instance.Abutton)
            {
                LoggingManager.Instance.AddEntry(
                    "A Button",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", InputDataRecord.Instance.Abutton }
                    }
                );
            }
        }
    }
}