using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private TaskScheduler TaskScheduler => ServiceProvider.Instance.GetService<TaskScheduler>();
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private enum State
    {
        Starting,
        Playing,
        Finished
    }

    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private Mole molePrefab;
    [SerializeField] private float timeToStart = 3f;
    [SerializeField] private LevelConfig config;

    private List<Mole> _moles = new List<Mole>();
    private float _timer = 0f;
    private int _timerInt = 0;
    private int _totalPoints = 0;

    private State _state = State.Starting;

    private Action _showMoleAction;

    private ConcurrentPool _pool;
    
    private void Awake()
    {
        ServiceProvider.Instance.AddService<TaskScheduler>(new GameObject("TaskScheduler").AddComponent<TaskScheduler>());

        foreach (Transform location in spawnLocations)
        {
            _moles.Add(Instantiate(molePrefab, location));
        }
        _showMoleAction = ShowMole;

        _timer = timeToStart;
        _totalPoints = 0;
        EventBus.Raise<OnPointsUpdated>(_totalPoints);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<OnMoleHit>(UpdatePoints);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<OnMoleHit>(UpdatePoints);
    }

    void Update()
    {
        UpdateTimer();

        switch (_state)
        {
            case State.Starting:
                {
                    if (_timer <= 0)
                    {
                        _state = State.Playing;
                        ShowMole();
                        _timer = config.timeLimit;
                        break;
                    }
                    break;
                }
            case State.Playing:
                {
                    if (_timer <= 0)
                    {
                        _state = State.Finished;
                        foreach (Mole mole in _moles)
                        {
                            mole.Hide();
                        }

                        int coins = _totalPoints / 100;
                        ConfigurationManager.totalCoins += coins;
                        EventBus.Raise<OnGameEnded>(_totalPoints, coins);                        
                        break;
                    }
                    break;
                }
            case State.Finished:
                {


                    break;
                }
            default:
                break;
        }
    }

    public void ShowMole()
    {
        if (_state == State.Playing)
        {
            Mole m = null;
            int hiddenCount = 0;

            for (int i = 0; i < _moles.Count; i++)
            {
                if (_moles[i].state == Mole.State.Hidden)
                {
                    hiddenCount++;
                    if (UnityEngine.Random.Range(0, hiddenCount) == 0)
                        m = _moles[i];
                }
            }

            if (m)
            {
                m.Rise();
                m.ScheduleHiding(config.moleHideTime);
                if (UnityEngine.Random.Range(0, 100) <= config.frozenProbability)
                    m.AddProperty(new FrozenProperty(m.animator));
            }

            TaskScheduler.Schedule(_showMoleAction, UnityEngine.Random.Range(config.nextMoleMinTime, config.nextMoleMaxTime));
        }
    }

    private void UpdateTimer()
    {
        _timer -= Time.deltaTime;
        int remainingSeconds = Mathf.CeilToInt(Mathf.Max(0, _timer));
        if (_timerInt != remainingSeconds)
        {
            _timerInt = remainingSeconds;
            EventBus.Raise<OnTimerUpdated>(_timerInt);
        }
    }

    private void UpdatePoints(in OnMoleHit context)
    {
        _totalPoints += context.pointsValue;
        EventBus.Raise<OnPointsUpdated>(_totalPoints);
    }
}
