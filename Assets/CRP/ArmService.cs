using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using Mycobot.csharp;
using System.Threading.Tasks;

public class ArmService
{
    private static ArmService _instance;
    public static ArmService Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ArmService();
            }
            return _instance;
        }
    }

    private MyCobot mc;
    static readonly int speed = 30;
    int[] angles = { 0, -27, 80, -47, 0, -40 };

    public ArmService()
    {
        mc = new MyCobot("COM3");
    }

    public void Open()
    {
        mc.Open();
        Thread.Sleep(5000);
        mc.SendAngles(angles, speed);
}

    public void TurnHorizontal(int val)
    {
        angles[4] = val;
        mc.SendOneAngle(4, angles[4], speed);
        Thread.Sleep(100);
    }
    public void TurnVertical(int val){
        angles[3]=val;
        mc.SendOneAngle(3, angles[3], speed);
        Thread.Sleep(100);
    }
    public void TurnLeft(int val){
        Debug.Log("Left: "+val);
        angles[4]=val;
        mc.SendOneAngle(4, angles[4], speed);
    }
    public void TurnStraight(){
        angles[4]=0;
        mc.SendOneAngle(4, angles[4], speed);
    }
    public void TurnRight(int val){
        Debug.Log("Right: "+val);
        if(val>=0){
            angles[4]=-1*val;
        } else{
            angles[4] = val;
        }
        mc.SendOneAngle(4, angles[4], speed);
    }
    public void TurnUp(int val){
        angles[3]+=val; 
        mc.SendOneAngle(3, angles[3], speed);
    }
    public void TurnCenter(){
        angles[3]=-47;
        mc.SendOneAngle(3, angles[3], speed);
    }
    public void TurnDown(int val){
        angles[3]-=val;
        mc.SendOneAngle(3, angles[3], speed);
    }

    public int[] GetCoords(){
        return mc.GetCoords();
    }

    public int[] GetAngles(){
        return mc.GetAngles();
    }

    // Async method to get angles
    public async Task<int[]> GetAnglesAsync()
    {
        return await Task.Run(() => mc.GetAngles());
    }

    public void close(){
        mc.Close();
    }
}
