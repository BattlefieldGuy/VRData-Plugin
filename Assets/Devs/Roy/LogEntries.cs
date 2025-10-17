using System;
using Devs.Jesper;
using Unity.VisualScripting;
using UnityEngine;

namespace Devs.Roy
{
    public class LogEntries : MonoBehaviour
    {
        [Tooltip("Turns on/off logging")]
        public bool toggleLogging = true;
        public static bool LogEnabled;

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
        private bool _prevY, _prevX, _prevB, _prevA;

        private void Awake()
        {
            LogEnabled = toggleLogging;
        }

        private void Update()
        {
            if (!toggleLogging) return;

            _timer += Time.deltaTime;
            _joystickLLogTimer += Time.deltaTime;
            _joystickRLogTimer += Time.deltaTime;

            // Log positions and rotations
            if (_timer >= logInterval)
            {
                LoggingManager.Instance.AddEntry(
                    Controller.Left,
                    DateTime.Now.ToString("o"),
                    InputDataRecord.Instance.PositionL,
                    InputDataRecord.Instance.RotationL
                );
                LoggingManager.Instance.AddEntry(
                    Controller.Right,
                    DateTime.Now.ToString("o"),
                    InputDataRecord.Instance.PositionR,
                    InputDataRecord.Instance.RotationR
                );
                LoggingManager.Instance.AddEntry(
                    Controller.Head,
                    DateTime.Now.ToString("o"),
                    InputDataRecord.Instance.PositionH,
                    InputDataRecord.Instance.RotationH
                    );
                LoggingManager.Instance.AddEntry(
                    Controller.Body,
                    DateTime.Now.ToString("o"),
                    InputDataRecord.Instance.PositionB,
                    InputDataRecord.Instance.RotationB
                );
                _timer = 0f;
            }

            // Grip L
            bool gripLPressed = InputDataRecord.Instance.GripL >= triggerThreshold;
            if (gripLPressed && !_prevGripL)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Left",
                    "Grip Pressed",
                    DateTime.Now.ToString("o")
                );
            }
            else if (!gripLPressed && _prevGripL)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Left",
                    "Grip Released",
                    DateTime.Now.ToString("o")
                );
            }
            _prevGripL = gripLPressed;

            // Grip R
            bool gripRPressed = InputDataRecord.Instance.GripR >= triggerThreshold;
            if (gripRPressed && !_prevGripR)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Right",
                    "Grip Pressed",
                    DateTime.Now.ToString("o")
                );
            }
            else if (!gripRPressed && _prevGripR)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Right",
                    "Grip Released",
                    DateTime.Now.ToString("o")
                );
            }
            _prevGripR = gripRPressed;

            // Trigger L
            bool triggerLPressed = InputDataRecord.Instance.TriggerL >= triggerThreshold;
            if (triggerLPressed && !_prevTriggerL)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Left",
                    "Trigger Pressed",
                    DateTime.Now.ToString("o")
                );
            }
            else if (!triggerLPressed && _prevTriggerL)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Left",
                    "Trigger Released",
                    DateTime.Now.ToString("o")
                );
            }
            _prevTriggerL = triggerLPressed;

            // Trigger R
            bool triggerRPressed = InputDataRecord.Instance.TriggerR >= triggerThreshold;
            if (triggerRPressed && !_prevTriggerR)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Right",
                    "Trigger Pressed",
                    DateTime.Now.ToString("o")
                );
            }
            else if (!triggerRPressed && _prevTriggerR)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Right",
                    "Trigger Released",
                    DateTime.Now.ToString("o")
                );
            }
            _prevTriggerR = triggerRPressed;

            // Joystick L
            if ((Mathf.Abs(InputDataRecord.Instance.JoystickL.x) >= deadzone || Mathf.Abs(InputDataRecord.Instance.JoystickL.y) >= deadzone) && _joystickLLogTimer >= joyStickLogInterval)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Left",
                    "Joystick Moved",
                    DateTime.Now.ToString("o"),
                    InputDataRecord.Instance.JoystickL.ToString()
                );
                _joystickLLogTimer = 0f;
            }

            // Joystick R
            if ((Mathf.Abs(InputDataRecord.Instance.JoystickR.x) >= deadzone || Mathf.Abs(InputDataRecord.Instance.JoystickR.y) >= deadzone) && _joystickRLogTimer >= joyStickLogInterval)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Right",
                    "Joystick Moved",
                    DateTime.Now.ToString("o"),
                    InputDataRecord.Instance.JoystickR.ToString()
                );
                _joystickRLogTimer = 0f;
            }

            // Y Button
            bool yPressed = InputDataRecord.Instance.Ybutton;
            if (yPressed && !_prevY)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Y Button",
                    "Pressed",
                    DateTime.Now.ToString("o")
                );
            }
            else if (!yPressed && _prevY)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "Y Button",
                    DateTime.Now.ToString("o"),
                    "Released"
                );
            }
            _prevY = yPressed;

            // X Button
            bool xPressed = InputDataRecord.Instance.Xbutton;
            if (xPressed && !_prevX)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "X Button",
                    DateTime.Now.ToString("o"),
                    "Pressed"
                );
            }
            else if (!xPressed && _prevX)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "X Button",
                    DateTime.Now.ToString("o"),
                    "Released"
                );
            }
            _prevX = xPressed;

            // B Button
            bool bPressed = InputDataRecord.Instance.Bbutton;
            if (bPressed && !_prevB)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "B Button",
                    DateTime.Now.ToString("o"),
                    "Pressed"
                );
            }
            else if (!bPressed && _prevB)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "B Button",
                    DateTime.Now.ToString("o"),
                    "Released"
                );
            }
            _prevB = bPressed;

            // A Button
            bool aPressed = InputDataRecord.Instance.Abutton;
            if (aPressed && !_prevA)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "A Button",
                    DateTime.Now.ToString("o"),
                    "Pressed"
                );
            }
            else if (!aPressed && _prevA)
            {
                LoggingManager.Instance.AddGenericEntry(
                    "A Button",
                    DateTime.Now.ToString("o"),
                    "Released"
                );
            }
            _prevA = aPressed;
        }
    }
}