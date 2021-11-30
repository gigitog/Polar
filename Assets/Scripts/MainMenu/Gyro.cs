using UnityEngine;
using static System.Math;

public class Gyro : MonoBehaviour
{
    [SerializeField] private float shiftModifier;
    [SerializeField] private float maxRotation = 20;
    private Gyroscope gyro;


    private float startingRotation;
    private Transform trns;

    // Start is called before the first frame update
    private void Start()
    {
        trns = transform;
        gyro = Input.gyro;
        gyro.enabled = true;
        startingRotation = trns.rotation.eulerAngles.x;
    }

    // Update is called once per frame
    private void Update()
    {
        var deltaRotationY = (float) Round(gyro.rotationRateUnbiased.y, 1) * shiftModifier * Time.deltaTime;
        var deltaRotationX = (float) Round(gyro.rotationRateUnbiased.x, 1) * shiftModifier * Time.deltaTime;
        var rotation = trns.rotation;

        var currentY = rotation.eulerAngles.y;
        var currentX = rotation.eulerAngles.x;

        if (currentX > 180)
            currentX -= 360;
        var rotateValueX = currentX + deltaRotationX;
        var rotateX = Mathf.Clamp(rotateValueX, -maxRotation, maxRotation);
        if (rotateX < 0)
            rotateX += 360;

        if (currentY > 180)
            currentY -= 360;
        var rotateValueY = currentY + deltaRotationY;
        var rotateY = Mathf.Clamp(rotateValueY, -maxRotation, maxRotation);
        if (rotateY < 0)
            rotateY += 360;

        // rotateY = (rotateY < 0) ? 
        trns.rotation = Quaternion.Euler(rotateX, rotateY, rotation.z);
        //trns.rotation = Quaternion.Euler(rotateX, rotateY, rotation.eulerAngles.z);
        // CheckRotation(trns, maxRotation, maxRotation, deltaRotationX, deltaRotationY);
    }

    // private void CheckRotation(Transform obj, float borderX, float borderY , float deltaRotationX, float deltaRotationY)
    // {
    //     obj.Rotate(deltaRotationX, 0, 0, Space.World);
    //     obj.Rotate(0, deltaRotationY, 0, Space.World);
    //
    //     if (obj.eulerAngles.x >= borderX + startingRotation|| obj.eulerAngles.x <= -borderX + startingRotation)
    //         obj.Rotate(-deltaRotationX, 0, 0, Space.World);
    //     
    //     if (obj.eulerAngles.y >= borderY + startingRotation || obj.eulerAngles.y <= -borderY + startingRotation)
    //         obj.Rotate(0,-deltaRotationY, 0, Space.World);
    //
    //     if (obj.eulerAngles.z != 0)
    //     {
    //         obj.DORotate(new Vector3(obj.eulerAngles.x, obj.eulerAngles.y, 0), 0);
    //     }
    // }
    private float NormalizeAngle(float angle)
    {
        if (angle < 0) angle = 360 + angle;

        return angle;
    }
}