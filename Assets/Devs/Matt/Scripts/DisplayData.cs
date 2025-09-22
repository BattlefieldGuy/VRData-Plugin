using TMPro;
using UnityEngine;

public class DisplayData : MonoBehaviour
{

    [Header("LeftSide")]
    [SerializeField] private TextMeshProUGUI positionL;
    [SerializeField] private TextMeshProUGUI rotationL;
    [SerializeField] private TextMeshProUGUI gripL;
    [SerializeField] private TextMeshProUGUI triggerL;
    [SerializeField] private TextMeshProUGUI joystickL;
    [SerializeField] private TextMeshProUGUI yButton;
    [SerializeField] private TextMeshProUGUI xButton;

    [Header("RightSide")]
    [SerializeField] private TextMeshProUGUI positionR;
    [SerializeField] private TextMeshProUGUI rotationR;
    [SerializeField] private TextMeshProUGUI gripR;
    [SerializeField] private TextMeshProUGUI triggerR;
    [SerializeField] private TextMeshProUGUI joystickR;
    [SerializeField] private TextMeshProUGUI bButton;
    [SerializeField] private TextMeshProUGUI aButton;

    private InputDataRecord record;

    void Start()
    {
        record = FindFirstObjectByType<InputDataRecord>();
    }

    void Update()
    {
        //Left Side
        positionL.text = "Position: " + record.PositionL.ToString();
        rotationL.text = "Rotation: " + record.RotationL.ToString();
        gripL.text = "Grip value: " + record.GripL.ToString();
        triggerL.text = "Trigger value: " + record.TriggerL.ToString();
        joystickL.text = "Joystick value: " + record.JoystickL.ToString();
        yButton.text = "Y button: " + record.Ybutton.ToString();
        xButton.text = "X button: " + record.Xbutton.ToString();

        //Right Side
        positionR.text = "Position: " + record.PositionR.ToString();
        rotationR.text = "Rotation: " + record.RotationR.ToString();
        gripR.text = "Grip value: " + record.GripR.ToString();
        triggerR.text = "Trigger value: " + record.TriggerR.ToString();
        joystickR.text = "Joystick value: " + record.JoystickR.ToString();
        bButton.text = "B button: " + record.Bbutton.ToString();
        aButton.text = "A button: " + record.Abutton.ToString();

    }
}
