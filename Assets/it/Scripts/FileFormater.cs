using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileFormater : MonoBehaviour
{
    // Start is called before the first frame update
    public string filePath;
    public string startString = "{\n\t\"jsonFile\": [\n";
    public string endString = "]}";
    public string comma = ",";
    public int insertPos = 0;
    public int commaPos = 0;
    void Start()
    {
        string fileContent = File.ReadAllText(filePath);
        
        string modifiedContent = fileContent.Insert(insertPos, startString);
        modifiedContent = modifiedContent.Insert(modifiedContent.Length, endString);

        for (int i = 0; i < modifiedContent.Length - 3; i++)
        {
            if (modifiedContent[i] == '\"' && modifiedContent[i + 1] == '}')
            {
                Debug.Log("hi: " + modifiedContent[i]);
                commaPos = i + 2; // Position to insert the comma after the closing curly brace
                modifiedContent = modifiedContent.Insert(commaPos, comma);
                // Adjust the loop index to skip the inserted comma and the next character
                i += comma.Length;
            }    
        }

        int rmIdx = modifiedContent.Length - 5;
        modifiedContent = modifiedContent.Remove(rmIdx, 1);

        File.WriteAllText(filePath, modifiedContent);
        
   
    }

}
