using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    [SerializeField] private List<EnemySpawnData> waveList;
    [SerializeField] private int waveCount;
    
    
    public List<EnemySpawnData> WaveList => waveList;
    public int WaveCount => waveCount;
}
