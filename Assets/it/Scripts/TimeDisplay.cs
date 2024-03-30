using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Timeline;

public class TimeDisplay : MonoBehaviour
{
    float now = 0.0f;
    float next = 0.5f;
    private void Update()
    {
        if(Time.time > now)
        {
            PlayerPrefs.SetString("dateTime", System.DateTime.Now.ToString("HH_mm_ss_MM"));
            Debug.Log(PlayerPrefs.GetString("dateTime"));

            now += next;
        }
    }
}
