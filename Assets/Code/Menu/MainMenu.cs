using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public bool isSignedIn { get; private set; }

    void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_welcome, 5, success => { Debug.Log("First achievment!"); });
    }

    void Update()
    {
        
    }

    public void LoadScene(string scene)
    {
        ServiceProvider.Instance.GetService<SceneLoader>().LoadScene(scene);
    }

    public void ShowAchievements()
    {
        if (isSignedIn)
            PlayGamesPlatform.Instance.ShowAchievementsUI();
    }

    private void ProcessAuthentication(SignInStatus status)
    {
        isSignedIn = status == SignInStatus.Success;
    }

    public void SignInManually()
    {
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }
}
