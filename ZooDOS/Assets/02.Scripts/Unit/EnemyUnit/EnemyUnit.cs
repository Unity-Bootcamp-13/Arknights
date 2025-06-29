using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    //적스포너
    public EnemyUnitSpawner _spawner;
    
    //현재 있는 타일
    public EnemyTile currentTile;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //초기값세팅
    public void Initialize(EnemyUnitSpawner spawner)
    {
        _spawner = spawner;
    }
    
    //이동시 위치 업데이트
    public void UpdateTile(Vector2Int vec2)
    {
        currentTile.enemiesOnTile.Remove(this);
        currentTile = _spawner.enemyTileMap[vec2.x, vec2.y];
        currentTile.enemiesOnTile.Add(this);
    }
    
    
    //저지당하면 정지
    
    
    
    
    //저지 풀리면 다시 경로탐색 
    
    
    
    
    //죽으면 EnemyList에서 Remove 요청
    [ContextMenu("OnDead")]
    public void OnDead()
    {
        // 죽을 때 적리스트에서 자기 자신을 제거 요청
        currentTile.enemiesOnTile.Remove(this);
        Destroy(gameObject);
    }
    
    
}
