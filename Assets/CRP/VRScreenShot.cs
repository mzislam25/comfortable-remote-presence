using UnityEngine;

public class VRScreenShot : MonoBehaviour
{
    public KeyCode screenshotKey = KeyCode.P;

    void Update()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        string screenshotFilename = "VRScreenshot_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        ScreenCapture.CaptureScreenshot(screenshotFilename);
        Debug.Log("Screenshot saved: " + screenshotFilename);
    }
}