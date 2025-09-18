using UnityEngine;
using UnityEngine.XR;

public class InputDataRecord : MonoBehaviour
{
    public static InputDataRecord Instance;

    [Header("LeftSide")]
    public Vector3 PositionL;
    public Quaternion RotationL;
    public float GripL;
    public float TriggerL;
    public Vector2 JoystickL;
    public bool Ybutton;
    public bool Xbutton;

    [Header("RightSide")]
    public Vector3 PositionR;
    public Quaternion RotationR;
    public float GripR;
    public float TriggerR;
    public Vector2 JoystickR;
    public bool Bbutton;
    public bool Abutton;


    private InputData inputData;


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
        inputData.LeftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 positionL);

        inputData.LeftController.TryGetFeatureValue(CommonUsages.deviceRotation, out RotationL);
        //RotationL.text = "Rotation: " + RotationL.ToString();

        inputData.LeftController.TryGetFeatureValue(CommonUsages.grip, out GripL);
        //GripL.text = "Grip value: " + GripL.ToString();

        inputData.LeftController.TryGetFeatureValue(CommonUsages.trigger, out TriggerL);
        //TriggerL.text = "Trigger value: " + TriggerL.ToString();

        inputData.LeftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out JoystickL);
        //JoystickL.text = "Joystick value: " + JoystickL.ToString();

        inputData.LeftController.TryGetFeatureValue(CommonUsages.secondaryButton, out Ybutton);
        //YButton.text = "Y button: " + Ybutton.ToString();

        inputData.LeftController.TryGetFeatureValue(CommonUsages.primaryButton, out Xbutton);
        //XButton.text = "X button: " + Xbutton.ToString();

        //RightSide
        inputData.RightController.TryGetFeatureValue(CommonUsages.devicePosition, out PositionR);
        //PositionR.text = "Position: " + PositionR.ToString();

        inputData.RightController.TryGetFeatureValue(CommonUsages.deviceRotation, out RotationR);
        //RotationR.text = "Rotation: " + RotationR.ToString();

        inputData.RightController.TryGetFeatureValue(CommonUsages.grip, out GripR);
        //GripR.text = "Grip value: " + GripR.ToString();

        inputData.RightController.TryGetFeatureValue(CommonUsages.trigger, out TriggerR);
        //TriggerR.text = "Trigger value: " + TriggerR.ToString();

        inputData.RightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out JoystickR);
        //JoystickR.text = "Joystick value: " + JoystickR.ToString();

        inputData.RightController.TryGetFeatureValue(CommonUsages.secondaryButton, out Bbutton);
        //BButton.text = "B button: " + Bbutton.ToString();

        inputData.RightController.TryGetFeatureValue(CommonUsages.primaryButton, out Abutton);
        //AButton.text = "A button: " + Abutton.ToString();



        //if (inputData.HMD.TryGetFeatureUsages(CommonUsages.))
    }
}