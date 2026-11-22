using TMPro;
using UnityEngine;

public class PointsUI : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private TextMeshProUGUI _pointsText;

    private void Awake()
    {
        _pointsText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<OnPointsUpdated>(UpdatePoints);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<OnPointsUpdated>(UpdatePoints);
    }

    private void UpdatePoints(in OnPointsUpdated context)
    {
        _pointsText.text = context.value.ToString();
    }
}