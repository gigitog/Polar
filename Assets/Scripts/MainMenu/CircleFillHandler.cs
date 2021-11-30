using UnityEngine;
using UnityEngine.UI;

public class CircleFillHandler : MonoBehaviour
{
    [Range(0, 1)] public float fillValue;

    [SerializeField] private Image circle;
    [SerializeField] private RectTransform roundBorderCircleMask;
    [SerializeField] private RectTransform roundBorderContainer;


    // Update is called once per frame
    private void Update()
    {
        FillCircleValue(fillValue);
    }

    private void FillCircleValue(float value)
    {
        fillValue = value;
        circle.fillAmount = fillValue;
        var angle = fillValue * 360;
        roundBorderContainer.localEulerAngles = new Vector3(0, 0, -angle);
        roundBorderCircleMask.localEulerAngles = new Vector3(0, 0, angle);
    }
}