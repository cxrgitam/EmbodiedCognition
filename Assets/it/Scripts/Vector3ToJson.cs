using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[Serializable]
public class Vector3Data
{
    public float x;
    public float y;
    public float z;
    public string v;

    public Vector3Data(float x, float y, float z, string v)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.v = v;
    }
}

[Serializable]
public class Vector3List
{
    public List<Vector3Data> vectors = new List<Vector3Data>();
}

public class Vector3ToJson : MonoBehaviour
{
    private string filePath;
    private Vector3List vector3List = new Vector3List();
    public string directoryPath;

    void Awake()
    {
        // Set the file path using a timestamp for uniqueness
        string parentDirectory = "User_" + DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss");
        directoryPath = Path.Combine(Application.persistentDataPath, parentDirectory);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        filePath = Path.Combine(directoryPath, "points.json");
        LoadVectors();
    }

    public void AddVector(Vector3 vector, string info)
    {
        vector3List.vectors.Add(new Vector3Data(vector.x, vector.y, vector.z, info));
        SaveVectors();
    }

    private void SaveVectors()
    {
        string json = JsonUtility.ToJson(vector3List, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Vectors saved to " + filePath);
    }

    private void LoadVectors()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            vector3List = JsonUtility.FromJson<Vector3List>(json);
            Debug.Log("Vectors loaded from " + filePath);
        }
        else
        {
            // Create a new file if it does not exist
            SaveVectors();
            Debug.Log("No existing file found. A new file created at " + filePath);
        }
    }
}
