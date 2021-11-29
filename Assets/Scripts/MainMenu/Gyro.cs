using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using static System.Math;

public class Gyro : MonoBehaviour
{
    [SerializeField] private float shiftModifier;
    [SerializeField] private float maxRotation = 20;
    

    private float startingRotation;
    private Transform trns;
    private Gyroscope gyro;

    // Start is called before the first frame update
    void Start()
    {
        trns = transform;
        gyro = Input.gyro;
        gyro.enabled = true;
        startingRotation = trns.rotation.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaRotationY = (float) Round(gyro.rotationRateUnbiased.y, 1) * shiftModifier * Time.deltaTime; 
        float deltaRotationX = (float) Round(gyro.rotationRateUnbiased.x, 1) * shiftModifier * Time.deltaTime;
        Quaternion rotation = trns.rotation;
        
        float currentY = rotation.eulerAngles.y;
        float currentX = rotation.eulerAngles.x;
        
        if (currentX > 180)
            currentX -= 360;
        float rotateValueX = currentX + deltaRotationX;
        float rotateX = Mathf.Clamp(rotateValueX, -maxRotation, maxRotation);
        if (rotateX < 0)
            rotateX += 360;
        
        if (currentY > 180)
            currentY -= 360;
        float rotateValueY = currentY + deltaRotationY;
        float rotateY = Mathf.Clamp(rotateValueY, -maxRotation, maxRotation);
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
        if (angle < 0)
        {
            angle = 360 + angle;
        }

        return angle;
    }
}
