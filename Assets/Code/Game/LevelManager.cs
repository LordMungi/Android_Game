using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private Mole molePrefab;

    private List<Mole> _moles = new List<Mole>();

    private void Awake()
    {
        foreach (Transform location in spawnLocations)
        {
            _moles.Add(Instantiate(molePrefab, location));
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
