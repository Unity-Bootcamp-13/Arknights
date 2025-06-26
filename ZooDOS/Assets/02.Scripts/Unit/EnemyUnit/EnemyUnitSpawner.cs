using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitSpawner : MonoBehaviour
{
    private List<EnemyUnit> enemyList;

    
    public IReadOnlyList<EnemyUnit> enemys => enemyList.AsReadOnly();
    
    [SerializeField] private EnemySpawnData enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyList = new List<EnemyUnit>();
        
        SpawnEnemy(enemy);
    }

    // Update is called once per frame
    void Update()
    {
        float i = 0;
        if (i>1)
        {
            
            Debug.Log(enemyList);
            i = 0;
        }

        i += Time.deltaTime;
    }
    
    
    
    //웨이브 
    //정해놓은 스폰시간마다 적스폰 
    public void CallWave()
    {
        
        
        
        
    }
    
    
    
    // 적 스폰
    // input : 스폰위치, 적유닛, 경로
    public void SpawnEnemy(EnemySpawnData enemyData)
    {
        var enemy = Instantiate(enemyData.enemyPrefab).GetComponent<EnemyUnit>();
        enemy.Initialize(this);
        enemyList.Add(enemy);

    }
    
    
    //유닛이 죽으면 리스트에서 제거
    public void RemoveEnemy(EnemyUnit enemy)
    {
        enemyList.Remove(enemy);
    }
    
    
    //경로 시각화
    public void PathVisualization()
    {
        
    }
    
    
}
