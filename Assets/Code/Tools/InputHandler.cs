using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour, IService
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    public bool IsPersistant => false;

    private float shakeThreshold = 1.5f;
    private float minShakeInterval = 0.2f;

    private float _lastShakeTime;
    private float _sqrThreshold;

    private void Awake()
    {
        _sqrThreshold = shakeThreshold * shakeThreshold;

    }

    private void OnEnable()
    {
        if (Accelerometer.current != null)
            InputSystem.EnableDevice(Accelerometer.current);
    }

    private void OnDisable()
    {
        if (Accelerometer.current != null && Accelerometer.current.enabled)
            InputSystem.DisableDevice(Accelerometer.current);
    }

    private void Update()
    {
        if (Accelerometer.current == null) return;

        Vector3 acceleration = Accelerometer.current.acceleration.ReadValue();

        if (acceleration.sqrMagnitude >= _sqrThreshold && Time.unscaledTime - _lastShakeTime >= minShakeInterval)
        {
            _lastShakeTime = Time.unscaledTime;
            EventBus.Raise<OnShakeDetected>();
        }
    }


}
