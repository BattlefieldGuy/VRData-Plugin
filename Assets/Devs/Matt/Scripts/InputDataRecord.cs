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

    [Header("Head")]
    public Vector3 PositionH;
    public Quaternion RotationH;


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

        inputData.LeftController.TryGetFeatureValue(CommonUsages.grip, out GripL);

        inputData.LeftController.TryGetFeatureValue(CommonUsages.trigger, out TriggerL);

        inputData.LeftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out JoystickL);

        inputData.LeftController.TryGetFeatureValue(CommonUsages.secondaryButton, out Ybutton);

        inputData.LeftController.TryGetFeatureValue(CommonUsages.primaryButton, out Xbutton);

        //RightSide
        inputData.RightController.TryGetFeatureValue(CommonUsages.devicePosition, out PositionR);

        inputData.RightController.TryGetFeatureValue(CommonUsages.deviceRotation, out RotationR);

        inputData.RightController.TryGetFeatureValue(CommonUsages.grip, out GripR);

        inputData.RightController.TryGetFeatureValue(CommonUsages.trigger, out TriggerR);

        inputData.RightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out JoystickR);

        inputData.RightController.TryGetFeatureValue(CommonUsages.secondaryButton, out Bbutton);

        inputData.RightController.TryGetFeatureValue(CommonUsages.primaryButton, out Abutton);

        //Head
        inputData.HMD.TryGetFeatureValue(CommonUsages.devicePosition, out PositionH);

        inputData.HMD.TryGetFeatureValue(CommonUsages.deviceRotation, out RotationH);

    }
}