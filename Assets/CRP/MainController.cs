using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainController : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Material material; 
    public GameObject Hmd;
    public GameObject arm;
    public int angleThreshold = 5;
    public float timeThreshold = 1f;

    private ArmService armService;
    private float previousTime;
    private float xAnglePrevious, yAnglePrevious;
    private Helper helper;

    private bool start = false;

    void Start()
    {
        armService = ArmService.Instance;
        armService.Open();
        helper = new Helper();
        previousTime = Time.time;
        xAnglePrevious = -47; // Initialize with -47 center [calibrated]
        yAnglePrevious = 0; // Initialize with 0 straight [calibrated]
        material.SetTexture("_BackgroundTex", helper.CreateTexture(Color.white));
    }

    // Update is called once per frame
    void Update()
    {
        if(start){
            Quaternion rotation = Hmd.transform.rotation;
            Vector3 eulerAngles = rotation.eulerAngles;

            float xAngleCurrent = eulerAngles.x;
            float yAngleCurrent = eulerAngles.y;

            float currentTime = Time.time;
            
            if (currentTime - previousTime > timeThreshold)
            {
                float deltaXAngle = Math.Abs(xAngleCurrent - xAnglePrevious);
                if(deltaXAngle > angleThreshold){
                    int xAngle = -47;
                    if (xAngleCurrent > 0 && xAngleCurrent < 30)//Down
                    {
                        xAngle = -47 - (int)xAngleCurrent;
                    }
                    if (xAngleCurrent > 330 && xAngleCurrent < 360)//Up
                    {
                        xAngle = -47 + (360 - (int)xAngleCurrent);
                    }
                    helper.CloneTexture(renderTexture, material);
                    armService.TurnVertical(xAngle);
                    arm.transform.rotation = Quaternion.Euler(xAngleCurrent, yAngleCurrent, 0);
                }
                float deltaYAngle = Math.Abs(yAngleCurrent - yAnglePrevious);
                if(deltaYAngle > angleThreshold){
                    int yAngle = 0;
                    if (yAngleCurrent > 0 && yAngleCurrent < 90)//Right
                    {
                        yAngle = 0 - (int)yAngleCurrent;
                    }
                    if (yAngleCurrent > 270 && yAngleCurrent < 360)//Left
                    {
                        yAngle = 360 - (int)yAngleCurrent;
                    }
                    helper.CloneTexture(renderTexture, material);
                    armService.TurnHorizontal(yAngle);
                    arm.transform.rotation = Quaternion.Euler(0, yAngleCurrent, 0);
                }
                
                xAnglePrevious = xAngleCurrent;
                yAnglePrevious = yAngleCurrent;
                previousTime = currentTime;
            }
        }
    }
}
