using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    [SerializeField] private List<EnemySpawnData> waveList;
    
    
    public List<EnemySpawnData> WaveList => waveList;
}
