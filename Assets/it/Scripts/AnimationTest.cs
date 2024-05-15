using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;

public class AnimationTest : MonoBehaviour
{
    public List<Vector3> plotPoints = new List<Vector3>();
    public string filePath;
    private int numOfPoints;

    private List<Color> gizmoColors = new List<Color>(); // List to store gizmo colors
    private int currentColorIndex = 0; // Track the index of the gizmo color to change
    private bool colorChanging = false; // Flag to indicate if color changing is in progress
    public float colorChangeInterval = 1.0f; // Change color every 1 second

    public class Coordinates
    {
        public double x;
        public double y;
        public double z;
    }

    public class Content
    {
        public Coordinates Position;
        public string EventName;
        public float time;
    }

    public class JsonFile
    {
        public List<Content> jsonFile;
    }

    public JsonFile myJsonFileList = new JsonFile();

    void Start()
    {
        string jsonText = System.IO.File.ReadAllText(filePath);
        Debug.Log(jsonText);

        var deserialized = JsonConvert.DeserializeObject<JsonFile>(jsonText);

        numOfPoints = deserialized.jsonFile.Count;
        plotPoints.Capacity = numOfPoints;
        gizmoColors.Capacity = numOfPoints; // Ensure gizmoColors list has the same capacity as plotPoints

        for (int i = 0; i < numOfPoints; i++)
        {
            plotPoints.Add(new Vector3((float)deserialized.jsonFile[i].Position.x, (float)deserialized.jsonFile[i].Position.y, (float)deserialized.jsonFile[i].Position.z));
            gizmoColors.Add(Color.red); // Initialize gizmo colors to red
        }

        StartCoroutine(ChangeColorsRoutine()); // Start the coroutine to change colors
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < plotPoints.Count - 1; i++)
        {
            Vector3 currentPoint = plotPoints[i];
            Vector3 nextPoint = plotPoints[i + 1];
            Vector3 centerPoint = (currentPoint + nextPoint) / 2f;

            Vector3 direction = nextPoint - currentPoint;

            // Check if the current point should be red or white based on the color of the next point
            if (i == currentColorIndex)
            {
                Gizmos.color = gizmoColors[i];
            }
            else
            {
                Gizmos.color = Color.white;
            }
            Gizmos.DrawSphere(currentPoint, 0.1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(currentPoint, nextPoint);

            if (direction != Vector3.zero)
            {
                Handles.color = Color.green;
                Handles.ConeHandleCap(0, centerPoint, Quaternion.LookRotation(direction, Vector3.up), 0.2f, EventType.Repaint);
            }
        }

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.yellow;
        labelStyle.fontSize = 22;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        for (int i = 0; i < plotPoints.Count; i++)
        {
            Handles.Label(plotPoints[i] + new Vector3(0, 0.5f, 0), new GUIContent(i.ToString()), labelStyle);
        }
    }

    IEnumerator ChangeColorsRoutine()
    {
        while (true)
        {
            if (!colorChanging)
            {
                colorChanging = true; // Set flag to indicate color changing is in progress
                gizmoColors[currentColorIndex] = Color.black; // Change the color of the gizmo sphere
                yield return new WaitForSeconds(colorChangeInterval);
                currentColorIndex = (currentColorIndex + 1) % plotPoints.Count; // Increment color index
                colorChanging = false; // Reset flag after color change is done
            }
            yield return null;
        }
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using UnityEditor;

//public class AnimationTest : MonoBehaviour
//{
//    public List<Vector3> plotPoints = new List<Vector3>();
//    public string filePath;
//    private int numOfPoints;

//    private List<Color> gizmoColors = new List<Color>(); // List to store gizmo colors
//    private int currentColorIndex = 0; // Track the index of the gizmo color to change
//    private bool colorChanging = false; // Flag to indicate if color changing is in progress
//    public float colorChangeInterval = 1.0f; // Change color every 1 second

//    public class Coordinates
//    {
//        public double x;
//        public double y;
//        public double z;
//    }

//    public class Content
//    {
//        public Coordinates Position;
//        public string EventName;
//        public float time;
//    }

//    public class JsonFile
//    {
//        public List<Content> jsonFile;
//    }

//    public JsonFile myJsonFileList = new JsonFile();

//    void Start()
//    {
//        string jsonText = System.IO.File.ReadAllText(filePath);
//        Debug.Log(jsonText);

//        var deserialized = JsonConvert.DeserializeObject<JsonFile>(jsonText);

//        numOfPoints = deserialized.jsonFile.Count;
//        plotPoints.Capacity = numOfPoints;
//        gizmoColors.Capacity = numOfPoints; // Ensure gizmoColors list has the same capacity as plotPoints

//        for (int i = 0; i < numOfPoints; i++)
//        {
//            plotPoints.Add(new Vector3((float)deserialized.jsonFile[i].Position.x, (float)deserialized.jsonFile[i].Position.y, (float)deserialized.jsonFile[i].Position.z));
//            gizmoColors.Add(Color.red); // Initialize gizmo colors to red
//        }

//        StartCoroutine(ChangeColorsRoutine()); // Start the coroutine to change colors
//    }

//    void OnDrawGizmos()
//    {
//        for (int i = 0; i < plotPoints.Count - 1; i++)
//        {
//            Vector3 currentPoint = plotPoints[i];
//            Vector3 nextPoint = plotPoints[i + 1];
//            Vector3 centerPoint = (currentPoint + nextPoint) / 2f;

//            Vector3 direction = nextPoint - currentPoint;

//            // Check if the current point should be red or white based on the color of the next point
//            if (i == currentColorIndex)
//            {
//                Gizmos.color = gizmoColors[i];
//            }
//            else
//            {
//                Gizmos.color = Color.red;
//            }
//            Gizmos.DrawSphere(currentPoint, 0.1f);

//            Gizmos.color = Color.blue;
//            Gizmos.DrawLine(currentPoint, nextPoint);

//            if (direction != Vector3.zero)
//            {
//                Handles.color = Color.green;
//                Handles.ConeHandleCap(0, centerPoint, Quaternion.LookRotation(direction, Vector3.up), 0.2f, EventType.Repaint);
//            }
//        }

//        GUIStyle labelStyle = new GUIStyle();
//        labelStyle.normal.textColor = Color.yellow;
//        labelStyle.fontSize = 22;
//        labelStyle.fontStyle = FontStyle.Bold;
//        labelStyle.alignment = TextAnchor.MiddleCenter;

//        for (int i = 0; i < plotPoints.Count; i++)
//        {
//            Handles.Label(plotPoints[i] + new Vector3(0, 0.5f, 0), new GUIContent(i.ToString()), labelStyle);
//        }
//    }

//    IEnumerator ChangeColorsRoutine()
//    {
//        while (true)
//        {
//            if (!colorChanging)
//            {
//                colorChanging = true; // Set flag to indicate color changing is in progress
//                gizmoColors[currentColorIndex] = Color.black; // Change the color of the gizmo sphere
//                yield return new WaitForSeconds(colorChangeInterval);
//                currentColorIndex = (currentColorIndex + 1) % plotPoints.Count; // Increment color index
//                colorChanging = false; // Reset flag after color change is done
//            }
//            yield return null;
//        }
//    }
//}
