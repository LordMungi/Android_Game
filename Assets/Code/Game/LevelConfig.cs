using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [field: SerializeField] public float timeLimit {  get; private set; }
    [field: SerializeField] public float nextMoleMinTime {  get; private set; }
    [field: SerializeField] public float nextMoleMaxTime {  get; private set; }
    [field: SerializeField] public float moleHideTime {  get; private set; }
    [field: SerializeField, Range(0, 100)] public float frozenProbability {  get; private set; }

}
