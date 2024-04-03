using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;

public class GizmosTest : MonoBehaviour
{
    [SerializeField] private List<Vector3> plotPoints = new List<Vector3>();
    public string filePath;
    private int numOfPoints;

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

        for (int i = 0; i < numOfPoints; i++)
        {
            plotPoints.Add(new Vector3((float)deserialized.jsonFile[i].Position.x, (float)deserialized.jsonFile[i].Position.y, (float)deserialized.jsonFile[i].Position.z));
        }
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < plotPoints.Count - 1; i++)
        {
            Vector3 currentPoint = plotPoints[i];
            Vector3 nextPoint = plotPoints[i + 1];

            Vector3 centerPoint = (currentPoint + nextPoint) / 2f;

            Vector3 direction = nextPoint - currentPoint;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(currentPoint, 0.1f);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(currentPoint, nextPoint);

            if(direction != Vector3.zero)
            {
                Handles.color = Color.green;
                Handles.ConeHandleCap(0, centerPoint, Quaternion.LookRotation(direction, Vector3.up), 0.35f, EventType.Repaint);
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
}


/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;

public class GizmosTest : MonoBehaviour
{
    [SerializeField] private List<Vector3> plotPoints = new List<Vector3>();
    public string filePath;
    private int numOfPoints;

    public class Coordinates
    {
        public double x;
        public double y;
        public double z;
    }
    public class Content
    {
        //public Dictionary<string,Dictionary<string,double>> 
        public Coordinates Position;
        public string EventName;
        public float time;
    }
    public class JsonFile
    {
        public List<Content> jsonFile;
    }

    public JsonFile myJsonFileList = new JsonFile();


    //Vector3[] points;

    // Start is called before the first frame update
    void Start()
    {
        //points = new Vector3[4]
        //{
        //    new Vector3(-1, 0, 0),
        //    new Vector3(1, 0, 0),
        //    new Vector3(-1, 1, 0),
        //    new Vector3(1, 1, 0)
        //};

        // Path to your JSON file
        string jsonText = System.IO.File.ReadAllText(filePath);
        Debug.Log(jsonText);

        var deserialized = JsonConvert.DeserializeObject<JsonFile>(jsonText);
        //myJsonFileList = JsonUtility.FromJson<JsonFile>(jsonText);

        // Read JSON file contents


        // Parse JSON data
        //JObject jsonObject = JObject.Parse(jsonText);

        // Extract values by key
        //string value1 = (string)jsonObject["Position"];
        //int value2 = (int)jsonObject["key2"];

        //Debug.Log(myJsonFileList.jsonFile[0].Position.y);
        foreach(var str in jsonText)
        {
            if(str == 'P')
            {
                numOfPoints++;
            }
        }
        plotPoints.Capacity = numOfPoints;
        Debug.Log(plotPoints.Capacity);

        for(int i =0; i< numOfPoints; i++)
        {
            //plotPoints[i] = new Vector3((float)deserialized.jsonFile[i].Position.x, (float)deserialized.jsonFile[i].Position.y, (float)deserialized.jsonFile[i].Position.z); 
            plotPoints.Add(new Vector3((float)deserialized.jsonFile[i].Position.x, (float)deserialized.jsonFile[i].Position.y, (float)deserialized.jsonFile[i].Position.z));

            Debug.Log("x =" + plotPoints[i].x);
            Debug.Log("y =" + plotPoints[i].y);
            Debug.Log("z =" + plotPoints[i].z);
            Debug.Log("\n");
        }

        Debug.Log(deserialized.jsonFile[2].Position.y);
        Debug.Log(numOfPoints);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        // Drawing Gizmo Spheres and Gizmo lines at world coordinates in the list
        foreach (var point in plotPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(point, 0.1f);  //drawing spheres at 
            Gizmos.color = Color.blue;
            if(plotPoints.IndexOf(point) == 0)
            {
                continue;
            }
            else
            {
                Gizmos.DrawLine(plotPoints[plotPoints.IndexOf(point) - 1], point);
            }
            
        }

        // This is the code for drawing arrows
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.yellow;
        labelStyle.fontSize = 22;
        //labelStyle.normal.background = Texture2D.blackTexture;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        for (int i = 0; i < plotPoints.Count; i++)
        {
            if (i > 0) {
                Vector3 targetPoint = plotPoints[i];
                Vector3 direction = targetPoint - plotPoints[i - 1];
                Handles.color = Color.green;

                if(direction != Vector3.zero)
                {
                    Handles.ArrowHandleCap(0, plotPoints[i - 1], Quaternion.LookRotation(direction, Vector3.up), 0.5f, EventType.Repaint);
                }
            }

            //Handles.Label(new Vector3(0, 1.5f, 0), new GUIContent(icon));
            Handles.Label(new Vector3(plotPoints[i].x, plotPoints[i].y + 0.5f, plotPoints[i].z), new GUIContent(i.ToString()), labelStyle);
        }



        // Draws a 5 unit long red line in front of the object
        //Gizmos.color = Color.blue;
        //Vector3 direction = transform.TransformDirection(Vector3.forward) * 5;
        //Gizmos.DrawSphere(transform.position, 0.25f);
        //Gizmos.color = Color.red;
        //Gizmos.DrawRay(transform.position, direction);
        //Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(transform.position + new Vector3(0, 0, 5), 0.25f);
    }




    //void OnDrawGizmosSelected()
    //{
    //    // Draws two parallel blue lines
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLineList(points);
    //}
}
 
 */