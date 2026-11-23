using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mole : MonoBehaviour, IPointerClickHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [Serializable]
    public enum State
    {
        Hidden,
        Rising,
        Shown,
        Hiding,
        Hiding_Hit
    }

    [SerializeField] private float topLimit;
    [SerializeField] private float bottomLimit;
    [SerializeField] private float moveDuration;
    [SerializeField] private int pointValue;
    [field: SerializeField] public Animator animator { get; private set; }

    [field: SerializeField] public State state { get; private set; }

    private Action _setStateHidden;
    private Action _setStateShown;

    private Vector3 _topPosition;
    private Vector3 _bottomPosition;

    private List<IMoleProperty> _properties = new List<IMoleProperty>();

    private MoleAnimator _animator;


    private void Awake()
    {
        state = State.Hidden;
        _topPosition = transform.position + new Vector3(0, topLimit, 0);
        _bottomPosition = transform.position - new Vector3(0, bottomLimit, 0);

        transform.position = _bottomPosition;

        _setStateHidden = () => 
        { 
            state = State.Hidden;
            _properties.Clear(); 
        };
        _setStateShown = () => state = State.Shown;

        _animator = new MoleAnimator(animator);
    }

    void Start()
    {

    }

    private void Update()
    {
        _animator?.Animate(state);
        foreach (IMoleProperty property in _properties)
        {
            property.Animate();
        }
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
            state = State.Hiding;
            Move(transform.position, _bottomPosition, Mathf.InverseLerp(_bottomPosition.y, _topPosition.y, transform.position.y) * moveDuration, State.Hiding, onFinish: _setStateHidden);
        }
    }

    public void Hit()
    {
        if (state == State.Shown || state == State.Rising)
        {
            bool canKill = true;
            foreach (IMoleProperty property in _properties)
            {
                if (!property.CanKill())
                {
                    canKill = false;
                    break;
                }
            }

            if (canKill)
            {
                state = State.Hiding_Hit;
                Move(transform.position, _bottomPosition, Mathf.InverseLerp(_bottomPosition.y, _topPosition.y, transform.position.y) * moveDuration, State.Hiding_Hit, onFinish: _setStateHidden);
                EventBus.Raise<OnMoleHit>(pointValue);
            }
            else
            {
                foreach (IMoleProperty property in _properties)
                {
                    property.Tap();
                }
            }
        }
    }

    public void AddProperty(IMoleProperty property)
    {
        _properties.Add(property);
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
    public async void ScheduleHiding(float time)
    {
        float t = 0;
        CancellationToken dct = destroyCancellationToken;
        while (t < time)
        {
            t += Time.deltaTime;
            await Awaitable.NextFrameAsync(dct);
            if (state != State.Rising && state != State.Shown)
                return;
        }
        Hide();
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
