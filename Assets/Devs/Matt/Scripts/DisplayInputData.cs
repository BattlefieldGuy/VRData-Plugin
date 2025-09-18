using System;
using System.Buffers;
using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class DisplayInputData : MonoBehaviour
{
    public static DisplayInputData Instance;
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

    [Header("LeftSide")]
    public Vector3 positionL;
    public Quaternion rotationL;
    public float gripL;
    public float triggerL;
    public Vector2 joystickL;
    public bool Ybutton;
    public bool Xbutton;
    
    [Header("RightSide")]
    public Vector3 positionR;
    public Quaternion rotationR;
    public float gripR;
    public float triggerR;
    public Vector2 joystickR;
    public bool Bbutton;
    public bool Abutton;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        inputData = GetComponent<InputData>();
    }

    void Update()
    {
        //LeftSide
        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.devicePosition, out positionL))
            PositionL.text = "Position: " + positionL.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.deviceRotation, out rotationL))
            RotationL.text = "Rotation: " + rotationL.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.grip, out gripL))
            GripL.text = "Grip value: " + gripL.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.trigger, out triggerL))
            TriggerL.text = "Trigger value: " + triggerL.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out joystickL))
            JoystickL.text = "Joystick value: " + joystickL.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.secondaryButton, out Ybutton))
            YButton.text = "Y button: " + Ybutton.ToString();

        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.primaryButton, out Xbutton))
            XButton.text = "X button: " + Xbutton.ToString();

        //RightSide
        if (inputData.RightController.TryGetFeatureValue(CommonUsages.devicePosition, out positionR))
            PositionR.text = "Position: " + positionR.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.deviceRotation, out rotationR))
            RotationR.text = "Rotation: " + rotationR.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.grip, out gripR))
            GripR.text = "Grip value: " + gripR.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.trigger, out triggerR))
            TriggerR.text = "Trigger value: " + triggerR.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out joystickR))
            JoystickR.text = "Joystick value: " + joystickR.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.secondaryButton, out Bbutton))
            BButton.text = "B button: " + Bbutton.ToString();

        if (inputData.RightController.TryGetFeatureValue(CommonUsages.primaryButton, out Abutton))
            AButton.text = "A button: " + Abutton.ToString();



        //if (inputData.HMD.TryGetFeatureUsages(CommonUsages.))
    }
}