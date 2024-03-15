using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GizmosTest : MonoBehaviour
{
    [SerializeField] private List<Vector3> plotPoints = new List<Vector3>();
    public string filePath;

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
        Debug.Log(deserialized.jsonFile[2].Position.y);

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
            Gizmos.DrawSphere(point, 0.25f);  //drawing spheres at 
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
