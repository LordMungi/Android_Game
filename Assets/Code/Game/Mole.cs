using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mole : MonoBehaviour, IPointerClickHandler
{
    [Serializable]
    public enum State 
    {
        Hidden,
        Rising,
        Shown,
        Hiding
    }

    [SerializeField] private float topLimit;
    [SerializeField] private float bottomLimit;
    [SerializeField] private float moveDuration;

    [field: SerializeField] public State state { get; private set; }
    public Action hideAction { get; private set; }
    private Action _setStateHidden;
    private Action _setStateShown;

    private Vector3 _topPosition;
    private Vector3 _bottomPosition;

    private CancellationTokenSource _riseCt = new CancellationTokenSource();
    private CancellationTokenSource _hideCt = new CancellationTokenSource();

    private void Awake()
    {
        state = State.Hidden;
        _topPosition = transform.position + new Vector3(0, topLimit, 0);
        _bottomPosition = transform.position - new Vector3(0, bottomLimit, 0);

        transform.position = _bottomPosition;
        hideAction = Hide;
        _setStateHidden = () => state = State.Hidden;
        _setStateShown = () => state = State.Shown;
    }

    void Start()
    {

    }

    public void Rise()
    {
        if (state == State.Hidden)
        {
            state = State.Rising;
            Move(_bottomPosition, _topPosition, moveDuration, State.Rising, onFinish: _setStateShown);
        }
    }

    public void Hide()
    {
        if (state == State.Shown || state == State.Rising)
        {
            _riseCt.Cancel();
            state = State.Hiding;
            Move(transform.position, _bottomPosition, Mathf.InverseLerp(_bottomPosition.y, _topPosition.y, transform.position.y) * moveDuration, State.Hiding, onFinish: _setStateHidden);
        }
    }

    public void Hit()
    {
        Hide();
    }

    private async void Move(Vector3 fromPosition, Vector3 toPosition, float duration, State doWhileInState, Action onFinish = null)
    {
        float t = 0;
        CancellationToken dct = destroyCancellationToken;

        while (t < duration)
        {            
            transform.position = Vector3.Lerp(fromPosition, toPosition, t / duration);
            t += Time.deltaTime;
            await Awaitable.NextFrameAsync(dct);

            if (state != doWhileInState)
                return;
        }
        transform.position = toPosition;
        onFinish?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + new Vector3(0, topLimit, 0), transform.position - new Vector3(0, bottomLimit, 0));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Hit();
    }
}
