using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        ServiceProvider.Instance.AddService<EventBus>(new EventBus());
        //ServiceProvider.Instance.AddService<AndroidLogger>(new AndroidLogger());
        ServiceProvider.Instance.AddService<SceneLoader>(new GameObject("SceneLoader").AddComponent<SceneLoader>());

        ServiceProvider.Instance.GetService<AndroidLogger>().SendLog("Game initialized.");
    }
}