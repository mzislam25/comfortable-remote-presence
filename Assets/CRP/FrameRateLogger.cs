using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class FrameDataLogger : MonoBehaviour
{
    private List<string> frameDataLog = new List<string>();
    private float startTime;
    private bool start = false;
    void Start()
    {
        startTime = Time.time;
        frameDataLog.Add("Time,Frame Count,FPS");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            start = !start;
        }
        if(start){
            float currentTime = (Time.time - startTime);
            int frameNumber = Time.frameCount;
            frameDataLog.Add($"{currentTime},{frameNumber},{frameNumber/currentTime}");
        }
    }

    void OnApplicationQuit()
    {
        string path = Application.dataPath + "/frame_data_log.csv";
        using (StreamWriter writer = new StreamWriter(path))
        {
            foreach (string line in frameDataLog)
            {
                writer.WriteLine(line);
            }
        }
        Debug.Log("Frame data log saved to: " + path);
    }
}