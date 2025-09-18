using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devs.Roy
{
    public class LogEntries : MonoBehaviour
    {
        [Tooltip("Turns on/off logging")]
        public bool toggleLogging = true;
    
        [Tooltip("Times per second for logging Positions and Rotations")]
        public float logFrequency = 10f;
        
        [Tooltip("Times per second for logging Joystick movements")]
        public float joyStickLogFrequency = 10f;
        
        [Tooltip("Threshold for considering trigger or grip as pressed")]
        public float triggerThreshold = 0.3f;
        
        [Tooltip("Deadzone for joystick movement")]
        public float deadzone = 0.5f;
    
        private float logInterval => 1f / logFrequency;
        private float joyStickLogInterval => 1f / joyStickLogFrequency;
        private float _timer;
        private float _joystickLLogTimer;
        private float _joystickRLogTimer;
        private bool _prevGripL, _prevGripR, _prevTriggerL, _prevTriggerR;
        private bool _prevJoystickLActive, _prevJoystickRActive;
        private bool _prevY, _prevX, _prevB, _prevA;
    
    
        private void Update()
        {
            if (toggleLogging)
            {
                _timer += Time.deltaTime;
                _joystickLLogTimer += Time.deltaTime;
                _joystickRLogTimer += Time.deltaTime;
                
                if (_timer >= logInterval)
                {
                    LoggingManager.Instance.AddEntry(
                        "Positions and Rotations",
                        new Dictionary<string, object>
                        {
                            { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                            { "Left Controller Position", InputDataRecord.Instance.PositionL },
                            { "Left Controller Rotation", InputDataRecord.Instance.RotationL },
                            { "Right Controller Position", InputDataRecord.Instance.PositionR },
                            { "Right Controller Rotation", InputDataRecord.Instance.RotationR },
                            { "Head Position", InputDataRecord.Instance.RotationR },
                            { "Head Rotation", InputDataRecord.Instance.RotationR }
                        }
                    );
                    _timer = 0f;
                } 
            
                // Grip L
                bool gripLPressed = InputDataRecord.Instance.GripL >= triggerThreshold;
                if (gripLPressed && !_prevGripL)
                {
                    LoggingManager.Instance.AddEntry(
                        "Left Grip Pressed", 
                        new Dictionary<string, object>
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "State", "Pressed"}
                    });
                }
                else if (!gripLPressed && _prevGripL)
                {
                    LoggingManager.Instance.AddEntry(
                        "Left Grip Released", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Released" }
                    });
                }
                _prevGripL = gripLPressed;

                // Grip R
                bool gripRPressed = InputDataRecord.Instance.GripR >= triggerThreshold;
                if (gripRPressed && !_prevGripR)
                {
                    LoggingManager.Instance.AddEntry(
                        "Right Grip Pressed", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "State", "Pressed" }
                    });
                }
                else if (!gripRPressed && _prevGripR)
                {
                    LoggingManager.Instance.AddEntry(
                        "Right Grip Released", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Released" }
                    });
                }
                _prevGripR = gripRPressed;

                // Trigger L
                bool triggerLPressed = InputDataRecord.Instance.TriggerL >= triggerThreshold;
                if (triggerLPressed && !_prevTriggerL)
                {
                    LoggingManager.Instance.AddEntry(
                        "Left Trigger Pressed", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Pressed" }
                    });
                }
                else if (!triggerLPressed && _prevTriggerL)
                {
                    LoggingManager.Instance.AddEntry(
                        "Left Trigger Released", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Released" }
                    });
                }
                _prevTriggerL = triggerLPressed;

                // Trigger R
                bool triggerRPressed = InputDataRecord.Instance.TriggerR >= triggerThreshold;
                if (triggerRPressed && !_prevTriggerR)
                {
                    LoggingManager.Instance.AddEntry(
                        "Right Trigger", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Pressed" }
                    });
                }
                else if (!triggerRPressed && _prevTriggerR)
                {
                    LoggingManager.Instance.AddEntry(
                        "Right Trigger", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Released" }
                    });
                }
                _prevTriggerR = triggerRPressed;

                // Joystick L
                if (InputDataRecord.Instance.JoystickL.x >= deadzone || InputDataRecord.Instance.JoystickL.y >= deadzone && _joystickLLogTimer >= joyStickLogInterval)
                {
                    LoggingManager.Instance.AddEntry("Left Joystick Position", new Dictionary<string, object> {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", InputDataRecord.Instance.JoystickL }
                    });
                    _joystickLLogTimer = 0f;
                }

                // Joystick R
                if (InputDataRecord.Instance.JoystickR.x >= deadzone || InputDataRecord.Instance.JoystickR.y >= deadzone && _joystickRLogTimer >= joyStickLogInterval)
                {
                    LoggingManager.Instance.AddEntry("Right Joystick Position", new Dictionary<string, object> {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        { "Input", InputDataRecord.Instance.JoystickR }
                    });
                    _joystickRLogTimer = 0f;
                }

                // Y Button
                bool yPressed = InputDataRecord.Instance.Ybutton;
                if (yPressed && !_prevY)
                {
                    LoggingManager.Instance.AddEntry(
                        "Y Button", 
                        new Dictionary<string, object> {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Pressed" }
                    });
                }
                else if (!yPressed && _prevY)
                {
                    LoggingManager.Instance.AddEntry(
                        "Y Button ", 
                        new Dictionary<string, object> {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Released" }
                    });
                }
                _prevY = yPressed;

                // X Button
                bool xPressed = InputDataRecord.Instance.Xbutton;
                if (xPressed && !_prevX)
                {
                    LoggingManager.Instance.AddEntry(
                        "X Button ", 
                        new Dictionary<string, object> {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Pressed" }
                    });
                }
                else if (!xPressed && _prevX)
                {
                    LoggingManager.Instance.AddEntry(
                        "X Button ", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Released" }
                    });
                }
                _prevX = xPressed;

                // B Button
                bool bPressed = InputDataRecord.Instance.Bbutton;
                if (bPressed && !_prevB)
                {
                    LoggingManager.Instance.AddEntry(
                        "B Button ", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Pressed" }
                    });
                }
                else if (!bPressed && _prevB)
                {
                    LoggingManager.Instance.AddEntry(
                        "B Button", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Released" }
                    });
                }
                _prevB = bPressed;

                // A Button
                bool aPressed = InputDataRecord.Instance.Abutton;
                if (aPressed && !_prevA)
                {
                    LoggingManager.Instance.AddEntry(
                        "A Button", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Pressed" }
                    });
                }
                else if (!aPressed && _prevA)
                {
                    LoggingManager.Instance.AddEntry(
                        "A Button", 
                        new Dictionary<string, object> 
                        {
                        { "Time", DateTime.Now.ToString("HH:mm:ss.fff") },
                        {"State", "Released" }
                    });
                }
                _prevA = aPressed;
            }
        }
    }
}