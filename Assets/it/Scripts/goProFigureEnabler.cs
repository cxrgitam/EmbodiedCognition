using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class goProFigureEnabler : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> goProItemList;
    public InputActionProperty leftTrigger;
    public ScreenshotManager scm;
    private int index=0;

    void Start()
    {
        scm.enabled = true;
        goProItemList[0].SetActive(true);
        index++;
    }

    // Update is called once per frame
    void Update()
    {
        if (leftTrigger.action.WasPressedThisFrame())
        {
            if(index < goProItemList.Count && goProItemList != null)
            {
                goProItemList[index - 1].SetActive(false);
                goProItemList[index].SetActive(true);
            }
            index++;
            
        }
    }
}
