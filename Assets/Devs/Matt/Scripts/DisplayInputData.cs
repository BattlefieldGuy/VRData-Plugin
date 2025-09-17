using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class DisplayInputData : MonoBehaviour
{
    [Header("LeftSide")]
    public TextMeshProUGUI PositionL;
    public TextMeshProUGUI RotationL;
    public TextMeshProUGUI GripL;
    public TextMeshProUGUI TriggerL;
    public TextMeshProUGUI JoystickL;
    public TextMeshProUGUI YButton;
    public TextMeshProUGUI XButton;

    [Header("RightSide")]
    public TextMeshProUGUI PositionR;
    public TextMeshProUGUI RotationR;
    public TextMeshProUGUI GripR;
    public TextMeshProUGUI TriggerR;
    public TextMeshProUGUI JoystickR;
    public TextMeshProUGUI BButton;
    public TextMeshProUGUI AButton;

    private InputData inputData;
    private float leftMaxScore = 0f;
    private float rightMaxScore = 0f;

    void Start()
    {
        inputData = GetComponent<InputData>();
    }

    void Update()
    {
        //LeftSide
        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 positionL))
            PositionL.text = "Position: " + positionL.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotationL))
            RotationL.text = "Rotation: " + rotationL.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.grip, out float gripL))
            GripL.text = "Grip value: " + gripL.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.trigger, out float triggerL))
            TriggerL.text = "Trigger value: " + triggerL.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickL))
            JoystickL.text = "Joystick value: " + joystickL.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool Ybutton))
            YButton.text = "Y button: " + Ybutton.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool Xbutton))
            XButton.text = "X button: " + Xbutton.ToString();

        //RightSide
        if (inputData.RightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 positionR))
            PositionR.text = "Position: " + positionR.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotationR))
            RotationR.text = "Rotation: " + rotationR.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.grip, out float gripR))
            GripR.text = "Grip value: " + gripR.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.trigger, out float triggerR))
            TriggerR.text = "Trigger value: " + triggerR.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickR))
            JoystickR.text = "Joystick value: " + joystickR.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool Bbutton))
            BButton.text = "B button: " + Bbutton.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool Abutton))
            AButton.text = "A button: " + Abutton.ToString();



        //if (inputData.HMD.TryGetFeatureUsages(CommonUsages.))
    }
}