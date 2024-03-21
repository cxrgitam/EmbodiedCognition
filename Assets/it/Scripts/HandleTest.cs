using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using TMPro;

public class HandleTest : MonoBehaviour
{
    public Texture icon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(0, 1, 0), 0.1f);
        Gizmos.DrawSphere(new Vector3(1, 1, 0), 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(0, 1, 0), new Vector3(1, 1, 0));

        Vector3 targetPoint = new Vector3(1, 1, 0);
        Vector3 direction = targetPoint - new Vector3(0, 1, 0);//transform.position;

        if(direction != Vector3.zero)
        {
            Handles.ArrowHandleCap(0, new Vector3(0, 1, 0), Quaternion.LookRotation(direction, Vector3.up), 0.5f, EventType.Repaint);
        }

        // for the next point i.e (1,2,0)

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(1, 2, 0), 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(1, 1, 0), new Vector3(1, 2, 0));

        targetPoint = new Vector3(1, 2, 0);
        direction = targetPoint - new Vector3(1, 1, 0);//transform.position;

        if (direction != Vector3.zero)
        {
            Handles.ArrowHandleCap(0, new Vector3(1, 1, 0), Quaternion.LookRotation(direction, Vector3.up), 0.5f, EventType.Repaint);
        }

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.black;
        labelStyle.fontSize = 22;
        //labelStyle.normal.background = Texture2D.blackTexture;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.alignment = TextAnchor.MiddleCenter;


        //Handles.Label(new Vector3(0, 1.5f, 0), new GUIContent(icon));
        Handles.Label(new Vector3(0, 1.5f, 0), new GUIContent("1"), labelStyle);

    }
}
