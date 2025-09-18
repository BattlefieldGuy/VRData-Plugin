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
                        { "Position", DisplayInputData.Instance.positionL },
                        { "Rotation", DisplayInputData.Instance.rotationL }
                    }
                );

                LoggingManager.Instance.AddEntry(
                    "Right Controller",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Position", DisplayInputData.Instance.positionL },
                        { "Rotation", DisplayInputData.Instance.rotationL }
                    }
                );
                timer = 0f;
            }

            if (DisplayInputData.Instance.gripL >= triggerThreshold)
            {
                LoggingManager.Instance.AddEntry(
                    "Left Grip",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Triggered", DisplayInputData.Instance.gripL }
                    }
                );
            }

            if (DisplayInputData.Instance.gripL >= triggerThreshold)
            {
                LoggingManager.Instance.AddEntry(
                    "Right Grip",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Triggered", DisplayInputData.Instance.gripR }
                    }
                );
            }

            if (DisplayInputData.Instance.triggerL >= triggerThreshold)
            {
                LoggingManager.Instance.AddEntry(
                    "Left Trigger",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Triggered", DisplayInputData.Instance.triggerL }
                    }
                );
            }

            if (DisplayInputData.Instance.triggerR >= triggerThreshold)
            {
                LoggingManager.Instance.AddEntry(
                    "Left Trigger",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Triggered", DisplayInputData.Instance.triggerL }
                    }
                );
            }

            if (DisplayInputData.Instance.joystickL.x >= deadzone || DisplayInputData.Instance.joystickL.y >= deadzone)
            {
                LoggingManager.Instance.AddEntry(
                    "Left Joystick",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", DisplayInputData.Instance.joystickL }
                    }
                );
            }

            if (DisplayInputData.Instance.joystickR.x >= deadzone || DisplayInputData.Instance.joystickR.y >= deadzone)
            {
                LoggingManager.Instance.AddEntry(
                    "Right Joystick",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", DisplayInputData.Instance.joystickR }
                    }
                );
            }

            if (DisplayInputData.Instance.Ybutton)
            {
                LoggingManager.Instance.AddEntry(
                    "Y Button",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", DisplayInputData.Instance.Ybutton }
                    }
                );
            }

            if (DisplayInputData.Instance.Xbutton)
            {
                LoggingManager.Instance.AddEntry(
                    "X Button",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", DisplayInputData.Instance.Xbutton }
                    }
                );
            }

            if (DisplayInputData.Instance.Bbutton)
            {
                LoggingManager.Instance.AddEntry(
                    "B Button",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", DisplayInputData.Instance.Bbutton }
                    }
                );
            }

            if (DisplayInputData.Instance.Abutton)
            {
                LoggingManager.Instance.AddEntry(
                    "A Button",
                    new Dictionary<string, object>
                    {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", DisplayInputData.Instance.Abutton }
                    }
                );
            }
        }
    }
}