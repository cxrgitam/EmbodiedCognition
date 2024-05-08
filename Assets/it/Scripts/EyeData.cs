using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class EyeData : MonoBehaviour
{
    // Start is called before the first frame update
    //public InputActionProperty g_pose;
    private GameObject reticle;
    //public XRInteractorReticleVisual reticle;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector3 getReticlePos()
    {
        reticle = GameObject.Find("Gaze Reticle(Clone)");
        Vector3 pos = reticle.transform.position;

        return pos;
    }
}
