using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArmController : MonoBehaviour
{
    public float offsetX = 0.25f;
    public float offsetY = 0.25f;
    public float X = 0.295f;
    public float Y = 0.2f;
    public float Z = 0.455f;
    public float W = 0.8f;

    public Material material; 

    private ArmService armService;
    private Vector4 currentRegion;
    public float interpolationSpeed = 2.5f; 

    private float currentX,currentY,currentZ,currentW;

    // Start is called before the first frame update
    void Start(){
        armService = ArmService.Instance;
        currentRegion = material.GetVector("_Region");
        currentX = X;
        currentY = Y;
        currentZ = Z;
        currentW = W;
    }

    // Update is called once per frame
    void Update(){
        float yAngleCurrent = transform.eulerAngles.y;
        float xAngleCurrent = transform.eulerAngles.x;
        Vector4 targetRegion = currentRegion;

        if (xAngleCurrent >= 0 && xAngleCurrent < 30)//Down
        {
            float xAngle = xAngleCurrent;
            float offset = ((offsetX/30)*xAngle)/2;
            currentY = Y - offset;
            currentW = W - offset;
            targetRegion = new Vector4(currentX, currentY, currentZ, currentW);
        }
        if (xAngleCurrent > 330 && xAngleCurrent < 360)//Up
        {
            float xAngle = 360 - (int)xAngleCurrent;
            float offset = ((offsetX/30)*xAngle)/2;
            currentY = Y + offset;
            currentW = W + offset;
            targetRegion = new Vector4(currentX, currentY, currentZ, currentW);
        }

        if (yAngleCurrent >= 0 && yAngleCurrent < 90)//Right
        {
            float yAngle = yAngleCurrent;
            float offset = ((offsetY/60)*yAngle)/2;
            currentX = X + offset;
            currentZ = Z + offset;
            targetRegion = new Vector4(currentX, currentY, currentZ, currentW);
        }
        if (yAngleCurrent > 270 && yAngleCurrent < 360)//Left
        {
            float yAngle = 360 - (int)yAngleCurrent;
            float offset = ((offsetY/60)*yAngle)/2;
            currentX = X - offset;
            currentZ = Z - offset;
            targetRegion = new Vector4(currentX, currentY, currentZ, currentW);
        }
        currentRegion = Vector4.Lerp(currentRegion, targetRegion, interpolationSpeed * Time.deltaTime);
        material.SetVector("_Region", currentRegion);
    }
}
