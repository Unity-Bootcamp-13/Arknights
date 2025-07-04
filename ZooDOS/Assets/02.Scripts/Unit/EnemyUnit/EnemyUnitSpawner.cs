using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
public class EnemyUnitSpawner : MonoBehaviour
{
    [SerializeField] private Map map;

    [SerializeField] private WaveData waveData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        StartCoroutine(CallWave());
    }

    /// <summary>
    /// 웨이브호출
    /// </summary>
    IEnumerator  CallWave()
    {

        foreach (var enemy in waveData.WaveList)
        {
            //정해놓은 스폰시간마다 적스폰, spawnTime 간격 대기 
            yield return new WaitForSeconds(enemy.spawnTime); 
            SpawnEnemy(enemy);
        }
        
    }
    
    
    /// <summary>
    /// 적 스폰
    /// </summary>
    /// <param name="enemyData">유닛 데이터</param>
    // EnemySpawnData : 적유닛프리펩, 스폰시간, 경로
    public void SpawnEnemy(EnemySpawnData enemyData)
    {
        var enemy = Instantiate(enemyData.enemyPrefab).GetComponent<EnemyUnit>();
        List<Vector3> path = new List<Vector3>();
        foreach (var waypoint in enemyData.path)
        {
            path.Add(map.CoordToVector3(waypoint));
        }
        enemy.Initialize(path);
        
    }
    
    
}
