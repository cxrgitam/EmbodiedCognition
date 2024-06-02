using UnityEngine;
using TMPro;
using System.IO;

public class DropdownSaver : MonoBehaviour
{
    public TMP_Dropdown[] dropdowns; // Array to hold your dropdown objects
    private string filePath;
    private string[] currentSelections;
    public Vector3ToJson vecToJson;

    void Start()
    {
        string directoryPath = vecToJson.directoryPath;
        filePath = Path.Combine(directoryPath, "dropdownSelections.txt");
        currentSelections = new string[dropdowns.Length];

        // Add listeners to all dropdowns
        for (int i = 0; i < dropdowns.Length; i++)
        {
            int index = i; // Capture the index for the listener
            dropdowns[i].onValueChanged.AddListener(delegate { SaveDropdownValue(index); });
        }

        // Load previous selections if available
        LoadDropdownValues();
    }

    public void SaveDropdownValue(int dropdownIndex)
    {
        string selectedText = dropdowns[dropdownIndex].options[dropdowns[dropdownIndex].value].text;
        currentSelections[dropdownIndex] = selectedText;

        // Save all current selections to the file
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (string selection in currentSelections)
            {
                writer.WriteLine(selection);
            }
        }
        Debug.Log("Dropdown selection updated and saved to " + filePath);
    }

    public void LoadDropdownValues()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length && i < dropdowns.Length; i++)
            {
                string selectedText = lines[i];
                TMP_Dropdown dropdown = dropdowns[i];
                int optionIndex = dropdown.options.FindIndex(option => option.text == selectedText);

                if (optionIndex != -1)
                {
                    dropdown.value = optionIndex;
                    currentSelections[i] = selectedText; // Keep track of the loaded selections
                }
            }
            Debug.Log("Dropdown selections loaded from " + filePath);
        }
    }
}
