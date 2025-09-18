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
                { "Left Controller Position", DisplayInputData.Instance.PositionL }
            });
            
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Controller Position" },
                { "Right Controller Position", DisplayInputData.Instance.PositionR }
            });
            
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Controller Rotation" },
                { "Left Controller Rotation", DisplayInputData.Instance.RotationL }
            });
            
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Controller Rotation" },
                { "Right Controller Rotation", DisplayInputData.Instance.RotationR }
            });
            timer = 0f;
        }
        
        if(DisplayInputData.Instance.gripL >= triggerThreshold)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Left Grip" },
                { "Left Grip Pressed", DisplayInputData.Instance.gripL }
            });
        }

        if (DisplayInputData.Instance.gripL >= triggerThreshold)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Right Grip" },
                { "Right Grip Pressed", DisplayInputData.Instance.gripR }
            });
        }
        
        if (DisplayInputData.Instance.triggerL >= triggerThreshold)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Left Trigger" },
                { "Left Trigger Pressed", DisplayInputData.Instance.triggerL }
            });
        }
        
        if (DisplayInputData.Instance.triggerR >= triggerThreshold)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Right Trigger" },
                { "Right Trigger Pressed", DisplayInputData.Instance.triggerR }
            });
        }

        if (DisplayInputData.Instance.joystickL != Vector2.zero)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Left Joystick" },
                { "Right Joystick Moved", DisplayInputData.Instance.joystickL }
            });
        }
        
        if (DisplayInputData.Instance.joystickR != Vector2.zero)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Right Joystick" },
                { "Right Joystick Moved", DisplayInputData.Instance.joystickR }
            });
        }
        
        if (DisplayInputData.Instance.Ybutton)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "Y Button" },
                { "Y Button Pressed", DisplayInputData.Instance.Ybutton }
            });
        }
        
        if (DisplayInputData.Instance.Xbutton)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "X Button" },
                { "X Button Pressed", DisplayInputData.Instance.Xbutton }
            });
        }
        
        if (DisplayInputData.Instance.Bbutton)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "B Button" },
                { "B Button Pressed", DisplayInputData.Instance.Bbutton }
            });
        }
        
        if (DisplayInputData.Instance.Abutton)
        {
            LoggingManager.Instance.AddEntry(new System.Collections.Generic.Dictionary<string, object>
            {
                { "Timestamp", DateTime.Now.ToString("HH:mm:ss.fff") },
                { "Event", "A Button" },
                { "A Button Pressed", DisplayInputData.Instance.Abutton }
            });
        }
    }
}