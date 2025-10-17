using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputData : MonoBehaviour
{
    public InputDevice RightController;
    public InputDevice LeftController;
    public InputDevice HMD;
    public InputDevice Body;


    void Update()
    {
        if (!RightController.isValid || !LeftController.isValid || !HMD.isValid)
            InitializeInputDevices();
    }

    private void InitializeInputDevices()
    {
        if (!RightController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref RightController);
        if (!LeftController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref LeftController);
        if (!HMD.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref HMD);
        if (!Body.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Simulated6DOF, ref Body);
    }

    private void InitializeInputDevice(InputDeviceCharacteristics inputCharateristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(inputCharateristics, devices);

        if (devices.Count > 0)
            inputDevice = devices[0];
    }
}