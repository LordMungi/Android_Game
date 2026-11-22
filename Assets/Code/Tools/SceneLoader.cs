using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IService
{
    public bool IsPersistant => true;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public async void LoadScene(string scene)
    {
        Scene activeScene = SceneManager.GetActiveScene();
        bool shouldLoad = false;

        await Load("Loading");
        await Unload(activeScene.name);
        Awaitable loadScene = Load(scene, canActivate : () => shouldLoad);
        await Awaitable.WaitForSecondsAsync(3f);
        shouldLoad = true;
        await loadScene;
        await Unload("Loading");

        ServiceProvider.Instance.ClearNonPersistantServices();
        ServiceProvider.Instance.AddService<EventBus>(new EventBus());
    }

    private async Awaitable Load(string scene, Func<bool> canActivate = null)
    {
        canActivate ??= () => true;
        CancellationToken ct = Application.exitCancellationToken;

        AsyncOperation op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        op.allowSceneActivation = canActivate();

        while (op.progress < 0.9f)
        {
            Debug.Log(op.progress);
            await Awaitable.NextFrameAsync(ct);
        }

        while (!canActivate())
        {
            await Awaitable.NextFrameAsync(ct);
        }
        op.allowSceneActivation = true;
        await op;
    }

    private async Awaitable Unload(string scene)
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync(scene);
        if (op != null)
            await op;
    }
}
