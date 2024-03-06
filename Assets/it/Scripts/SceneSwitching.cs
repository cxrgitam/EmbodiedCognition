using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitching : MonoBehaviour
{

    private void Start()
    {
        //SceneManager.LoadScene("Rec Scene",LoadSceneMode.Single);
    }
    public void LoadVisScene()
    {
        Debug.Log("itzzz working...");
        SceneManager.LoadScene("Vis Scene",LoadSceneMode.Additive);
    }
}
