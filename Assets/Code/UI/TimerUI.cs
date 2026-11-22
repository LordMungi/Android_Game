using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private TextMeshProUGUI _timerText;

    private void Awake()
    {
        _timerText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<OnTimerUpdated>(UpdateTimer);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<OnTimerUpdated>(UpdateTimer);
    }

    private void UpdateTimer(in OnTimerUpdated context)
    {
        _timerText.text = context.value.ToString();
    }
}