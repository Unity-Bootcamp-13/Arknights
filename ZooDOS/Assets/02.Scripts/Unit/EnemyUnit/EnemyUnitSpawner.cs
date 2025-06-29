using System.Collections.Generic;
using UnityEngine;
public class EnemyTile
{
    public List<EnemyUnit> enemiesOnTile = new();
}
public class EnemyUnitSpawner : MonoBehaviour
{
    public EnemyTile[,] enemyTileMap;
    
    //public IReadOnlyList<EnemyUnit> enemys => enemyList.AsReadOnly();
    
    [SerializeField] private EnemySpawnData enemy;

    private float i=0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int width = 10;
        int height = 10;
        enemyTileMap = new EnemyTile[10, 10];

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                enemyTileMap[x, y] = new EnemyTile();
            }
        }
        
        SpawnEnemy(enemy);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        //enemyList.Add(enemy);

    }
    
    
    //유닛이 죽으면 리스트에서 제거
    public void RemoveEnemy(EnemyUnit enemy)
    {
        
        //enemyList.Remove(enemy);
    }
    
    //적리스트 타일좌표값으로 리턴
    public List<EnemyUnit> OnTileEnemyList()
    {
        
        
        return new List<EnemyUnit>();
    }
    
    //경로 시각화
    public void PathVisualization()
    {
        
    }
    
    
}
