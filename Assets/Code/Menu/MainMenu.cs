using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private AndroidLogger AndroidLogger => ServiceProvider.Instance.GetService<AndroidLogger>();
    private SceneLoader SceneLoader => ServiceProvider.Instance.GetService<SceneLoader>();


    public bool isSignedIn { get; private set; }

    [SerializeField] private Canvas mainMenuCanvas;
    [SerializeField] private Canvas loggerCanvas;
    [SerializeField] private Canvas levelsCanvas;

    void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_welcome, 5, success => { Debug.Log("First achievment!"); });
    }

    void Update()
    {
        
    }

    public void StartGame(LevelConfig config)
    {
        ConfigurationManager.selectedLevel = config;
        SceneLoader.LoadScene("Game");
    }

    public void LoadScene(string scene)
    {
        SceneLoader.LoadScene(scene);
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

    public void SetLogger(bool v)
    {
        loggerCanvas.gameObject.SetActive(v);
    }

    public void SetMainMenu(bool v)
    {
        mainMenuCanvas.gameObject.SetActive(v);
    }
    public void SetLevels(bool v)
    {
        levelsCanvas.gameObject.SetActive(v);
    }

    public void ShowLogs()
    {
        AndroidLogger.ShowLogs();
    }

    public void ClearLogs()
    {
        AndroidLogger.ClearLogs();
    }
}
