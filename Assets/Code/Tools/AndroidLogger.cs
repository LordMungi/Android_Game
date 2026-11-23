using UnityEngine;

public class AndroidLogger : IService
{
    public bool IsPersistant => true;

    AndroidJavaObject loggerManagerInstance;
    AndroidJavaObject mainActivity;

    public AndroidLogger()
    {
        using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        mainActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        using var loggerClass = new AndroidJavaClass("com.example.logger.LoggerManager");
        loggerClass.SetStatic("mainActivity", mainActivity);
        loggerManagerInstance = loggerClass.CallStatic<AndroidJavaObject>("getInstance");
    }

    public void SendLog(string log)
    {
        loggerManagerInstance?.Call("getLog", log);
    }

    public void ClearLogs()
    {
        loggerManagerInstance?.Call("clearLogs");
    }

    public void ShowLogs()
    {
        loggerManagerInstance.Call("showLogs");
    }

    ~AndroidLogger()
    {
        loggerManagerInstance?.Dispose();
        mainActivity?.Dispose();
    }
}
