using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class JSONEventWriter : IEventWriter
{
    private readonly bool createFileIfNonFound;
    private readonly string path;
    private readonly bool hasFileToWrite;
    private static readonly int MaxRetryCount = 5;
    private static readonly int DelayBetweenRetries = 1000; // milliseconds

    public JSONEventWriter(string path, bool createFileIfNonFound)
    {
        this.path = path;
        this.createFileIfNonFound = createFileIfNonFound;
        hasFileToWrite = Startup();
    }

    bool IEventWriter.IsWriterAvailable()
    {
        return hasFileToWrite;
    }

    bool IEventWriter.SaveEvent(BaseEvent baseEvent)
    {
        if (!hasFileToWrite || !File.Exists(path))
        {
            return false;
        }

        int retryCount = 0;

        while (retryCount < MaxRetryCount)
        {
            try
            {
                using (var writer = new StreamWriter(new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read)))
                {
                    writer.WriteLine(ComposeJsonString(baseEvent));
                }
                return true;
            }
            catch (IOException ex)
            {
                if (IsSharingViolation(ex))
                {
                    retryCount++;
                    Thread.Sleep(DelayBetweenRetries);
                }
                else
                {
                    Debug.Log(ex.ToString());
                    return false;
                }
            }
        }

        return false;
    }

    bool IEventWriter.SaveEvents(List<BaseEvent> baseEvents)
    {
        if (!hasFileToWrite || !File.Exists(path))
        {
            return false;
        }

        int retryCount = 0;

        while (retryCount < MaxRetryCount)
        {
            try
            {
                using (var writer = new StreamWriter(new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read)))
                {
                    foreach (BaseEvent baseEvent in baseEvents)
                    {
                        writer.WriteLine(ComposeJsonString(baseEvent));
                    }
                }
                return true;
            }
            catch (IOException ex)
            {
                if (IsSharingViolation(ex))
                {
                    retryCount++;
                    Thread.Sleep(DelayBetweenRetries);
                }
                else
                {
                    Debug.Log(ex.ToString());
                    return false;
                }
            }
        }

        return false;
    }

    private bool CreateFile(string path)
    {
        if (!createFileIfNonFound) return false;

        try
        {
            using (File.Create(path)) { }
            return true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }
    }

    private string ComposeJsonString(BaseEvent baseEvent)
    {
        return JsonUtility.ToJson(baseEvent);
    }

    private bool Startup()
    {
        if (File.Exists(path)) return true;
        if (!createFileIfNonFound) return false;
        return CreateFile(path);
    }

    private bool IsSharingViolation(IOException ex)
    {
        return ex.Message.Contains("Sharing violation");
    }
}
