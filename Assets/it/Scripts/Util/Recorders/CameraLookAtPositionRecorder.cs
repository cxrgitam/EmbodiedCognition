using UnityEngine;
using System;

/// <summary>
/// Example of event recorder, that tracks and saves camera look at position (Raycast) in specified interval.
/// </summary>
public class CameraLookAtPositionRecorder : AbstractEventIntervalRecorder
{
    [SerializeField]
    private Camera cameraToRecord = null;
    [SerializeField]
    public string dataPath;
    [SerializeField]
    private string eventName;
    [SerializeField]
    private bool createFileIfNonFound;
    //private String dateTime;

    private IEventWriter eventWriter;

    private readonly Vector3 centerOfScreen = new(0.5F, 0.5F, 0.5F);


    void Awake()
    {
        dataPath = Application.persistentDataPath;
        PlayerPrefs.SetString("dateTime", System.DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss"));
        dataPath += "/" + PlayerPrefs.GetString("dateTime");

        if (!record) return;

        if (!IsCameraOnTheScene())
        {
            record = false;
            return;
        }

        eventWriter = new JSONEventWriter(dataPath, createFileIfNonFound);
        record = eventWriter.IsWriterAvailable();

        if (record) StartCoroutine(StartEventRecording());
    }
    private void Update()
    {
        
    }

    protected override void RecordAndSaveEvent()
    {
        BaseEvent baseEvent = PrepareData();

        if (baseEvent == null) return;

        eventWriter.SaveEvent(baseEvent);
    }

    private BaseEvent PrepareData()
    {
        int layerMasknum = 1 << 6;
        Ray ray = cameraToRecord.ViewportPointToRay(centerOfScreen);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 11F, layerMasknum, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("The object layer is:" + hit.transform.gameObject.layer + "and name: " + hit.transform.gameObject.name);
            return new BaseEvent(eventName, hit.point);
        }

        return null;
    }

    private bool IsCameraOnTheScene()
    {
        return cameraToRecord != null;
    }
}
