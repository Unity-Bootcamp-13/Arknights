using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
public class StageData : ScriptableObject
{
    [SerializeField] private List<WaveData> _stage;

    public IReadOnlyList<WaveData> Stage => _stage;


    public int StageCount
    {
        get
        {
            int sum = 0;
            foreach (WaveData wave in _stage)
            {
                sum += wave.WaveCount;
            }
            return sum;
        }
    }

}
