using System;
using System.Threading;
using UnityEngine;

public class Mole : MonoBehaviour
{
    private enum State
    {
        Hidden,
        Rising,
        Shown,
        Hiding
    }

    [SerializeField] private float topLimit;
    [SerializeField] private float bottomLimit;
    [SerializeField] private float moveDuration;

    private State _state;

    private Vector3 _topPosition;
    private Vector3 _bottomPosition;

    private void Awake()
    {
        _state = State.Hidden;
        _topPosition = transform.position + new Vector3(0, topLimit, 0);
        _bottomPosition = transform.position - new Vector3(0, bottomLimit, 0);

        transform.position = _bottomPosition;
    }

    void Start()
    {
        
    }

    public void Rise()
    {
        if (_state == State.Hidden)
        {
            _state = State.Rising;
            Move(_bottomPosition, _topPosition, moveDuration, onFinish: () => _state = State.Shown);
        }
    }

    public void Hide()
    {
        if (_state == State.Shown)
        { 
            _state = State.Hiding;
            Move(_topPosition, _bottomPosition, moveDuration, onFinish : () => _state = State.Hidden);
        }
    }

    public void Hit()
    {
        if (_state == State.Shown)
            _state = State.Hiding;
    }

    private async void Move(Vector3 fromPosition, Vector3 toPosition, float duration, Action onFinish = null)
    {
        float t = 0;
        CancellationToken ct = destroyCancellationToken;

        while (t < duration)
        {
            transform.position = Vector3.Lerp(fromPosition, toPosition, t / duration);
            t += Time.deltaTime;
            await Awaitable.NextFrameAsync(ct);
        }
        transform.position = toPosition;
        onFinish?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(0, topLimit, 0), transform.position - new Vector3(0, bottomLimit, 0)); 
    }
}
