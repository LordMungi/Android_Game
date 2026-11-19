using UnityEngine;
using UnityEngine.SceneManagement;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        ServiceProvider.Instance.AddService<EventBus>(new EventBus());
    }
}