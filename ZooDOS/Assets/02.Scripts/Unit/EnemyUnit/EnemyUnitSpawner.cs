using System.Collections;
using System.Collections.Generic;
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
        enemy.Initialize(this);
        
        //enemyList.Add(enemy);

    }
    
    /// <summary>
    /// 적리스트 타일좌표값으로 리턴
    /// </summary>
    /// <param name="pos">위치 좌표</param>
    /// <returns>적 리스트</returns>
    
    public List<EnemyUnit> OnTileEnemyList(Position pos)
    {
        
        
        return map.MapTiles[pos.X, pos.Y].EnemyUnits;
    }
    
    
    
}
