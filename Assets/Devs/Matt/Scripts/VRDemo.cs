using UnityEngine;
using JsonReader = Devs.Roy.JsonReader;
public class VRDemo : MonoBehaviour
{
    [Header("Head")]
    [SerializeField] private GameObject VRHeadSet;

    [Space(10)]
    [Header("Controllers")]
    [SerializeField] private GameObject RightController;
    [SerializeField] private GameObject LeftController;

    void Start()
    {

    }

    void Update()
    {
        foreach (var evt in JsonReader.Instance.GetJsonSession().events)
        {
            if (evt.controller == "0")
                RightController.transform.position = new Vector3(evt.details.position[0], evt.details.position[1], evt.details.position[2]);
        }
    }
}