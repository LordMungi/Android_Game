using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private SceneLoader SceneLoader => ServiceProvider.Instance.GetService<SceneLoader>();

    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI totalCoinsText;

    private void Awake()
    {
        gameOverCanvas.gameObject.SetActive(false);
        Debug.Log(ConfigurationManager.totalCoins);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<OnGameEnded>(ShowGameOverScreen);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnGameEnded>(ShowGameOverScreen);
    }

    private void ShowGameOverScreen(in OnGameEnded context)
    {
        pointsText.text = "Points: " + context.points.ToString();
        coinsText.text = "Coins: +" + context.coins.ToString();
        totalCoinsText.text = "Total coins: " + ConfigurationManager.totalCoins.ToString();

        gameOverCanvas.gameObject.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneLoader.LoadScene("Main Menu");
    }

    public void RestartLevel()
    {
        SceneLoader.LoadScene("Game");
    }
}
