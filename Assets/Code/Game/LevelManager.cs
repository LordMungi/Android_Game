using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private TaskScheduler TaskScheduler => ServiceProvider.Instance.GetService<TaskScheduler>();
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
    private State _state = State.Starting;

    private Action _showMoleAction;
    
    private void Awake()
    {
        ServiceProvider.Instance.AddService<TaskScheduler>(new GameObject("TaskScheduler").AddComponent<TaskScheduler>());

        foreach (Transform location in spawnLocations)
        {
            _moles.Add(Instantiate(molePrefab, location));
        }
        _showMoleAction = ShowMole;
    }

    void Start()
    {

    }

    void Update()
    {
        _timer += Time.deltaTime;

        switch (_state)
        {
            case State.Starting:
                {
                    if (_timer >= timeToStart)
                    {
                        _state = State.Playing;
                        ShowMole();
                        _timer = 0f;
                        break;
                    }
                    break;
                }
            case State.Playing:
                {
                    if (_timer >= config.timeLimit)
                    {
                        _state = State.Finished;
                        foreach (Mole mole in _moles)
                        {
                            mole.Hide();
                        }
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
                TaskScheduler.Schedule(m.hideAction, config.moleHideTime);
            }

            TaskScheduler.Schedule(_showMoleAction, UnityEngine.Random.Range(config.nextMoleMinTime, config.nextMoleMaxTime));
        }
    }

    public void HideMole(Mole mole)
    {
        mole.Hide();
    }
}
