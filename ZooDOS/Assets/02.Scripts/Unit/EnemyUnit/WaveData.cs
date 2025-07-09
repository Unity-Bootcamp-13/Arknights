using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    [SerializeField] private List<EnemySpawnData> _wave;
    [SerializeField] private int _waveCount;
    [SerializeField] private float _startTime;
    
    public List<EnemySpawnData> Wave => _wave;
    public int WaveCount => _waveCount;

    public float StartTime => _startTime;
}
