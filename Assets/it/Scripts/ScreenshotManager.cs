using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;

public class ScreenshotManager : MonoBehaviour
{
    public InputActionProperty Abutton;
    public Vector3ToJson vecToJson;

    void Update()
    {
        // Check if the screenshot key is pressed.
        if (Abutton.action.WasPressedThisFrame())
        {
            TakeScreenshot();
        }
    }

    public void TakeScreenshot()
    {
        // Define the screenshot filename and path.
        string directoryPath = vecToJson.directoryPath;
        string folderPath = Path.Combine(directoryPath, "Screenshots");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string screenshotName = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullPath = Path.Combine(folderPath, screenshotName);

        // Capture the screenshot.
        ScreenCapture.CaptureScreenshot(fullPath);
        Debug.Log("Screenshot saved to: " + fullPath);
    }
}
