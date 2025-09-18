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
        record = GetComponent<InputDataRecord>();
    }

    void Update()
    {
        //positionL.text = "Position: " + PositionL.ToString();

    }
}
