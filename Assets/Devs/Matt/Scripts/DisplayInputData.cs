using TMPro;
using UnityEngine;
using UnityEngine.XR;

public class DisplayInputData : MonoBehaviour
{
    public TextMeshProUGUI LeftScoreDisplay;
    public TextMeshProUGUI RightScoreDisplay;

    private InputData inputData;
    private float leftMaxScore = 0f;
    private float rightMaxScore = 0f;

    void Start()
    {
        inputData = GetComponent<InputData>();
    }

    void Update()
    {
        if (inputData.LeftController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 leftVelocity))
        {
            leftMaxScore = Mathf.Max(leftVelocity.magnitude, leftMaxScore);
            LeftScoreDisplay.text = leftMaxScore.ToString();
        }
        if (inputData.RightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 rightVelocity))
        {
            rightMaxScore = Mathf.Max(rightVelocity.magnitude, rightMaxScore);
            LeftScoreDisplay.text = "" + leftMaxScore.ToString();
        }
    }
}