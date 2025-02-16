using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class KeyController : MonoBehaviour
{
    public GameObject hmd;
    public GameObject arm;
    private ArmService armService;
    public RenderTexture renderTexture;
    public Material material; 
    private Helper helper;
    private float xAnglePrevious, yAnglePrevious;
    private float previousTime;

    void Start()
    {
        armService = ArmService.Instance;
        armService.Open();
        helper = new Helper();
        previousTime = Time.time;
        xAnglePrevious = -47; // Initialize with -47 center [calibrated value]
        yAnglePrevious = 0; // Initialize with 0 straight [calibrated value]
        material.SetTexture("_BackgroundTex", helper.CreateTexture(Color.white));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)){
            helper.CloneTexture(renderTexture, material);
        }
        float xAngleCurrent = hmd.transform.eulerAngles.x;
        float yAngleCurrent = hmd.transform.eulerAngles.y;
        float currentTime = Time.time;

        if (currentTime - previousTime > 1)
        {
            float deltaXAngle = Math.Abs(xAngleCurrent - xAnglePrevious);
            if(deltaXAngle > 5){
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
            if(deltaYAngle > 5){
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
                arm.transform.rotation = Quaternion.Euler(xAngleCurrent, yAngleCurrent, 0);
            }
            
            xAnglePrevious = xAngleCurrent;
            yAnglePrevious = yAngleCurrent;
            previousTime = currentTime;
        }
    }

    void Destroy(){
        armService.close();
    }
}
