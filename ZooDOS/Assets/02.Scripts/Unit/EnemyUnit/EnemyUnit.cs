using UnityEngine;

public class EnemyUnit : Unit
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
    /// <summary>
    /// 초기값세팅
    /// </summary>
    /// <param name="spawner">적 스포너 참조</param>
    
    public void Initialize(EnemyUnitSpawner spawner)
    {
        _spawner = spawner;
    }
    
    /// <summary>
    /// 이동시 위치 업데이트
    /// </summary>
    /// <param name="vec2">변경할 위치좌표</param>
    public void UpdateTile(Vector2Int vec2)
    {
        currentTile.enemiesOnTile.Remove(this);
        currentTile = _spawner.enemyTileMap[vec2.x, vec2.y];
        currentTile.enemiesOnTile.Add(this);
    }
    
    
    //저지당하면 정지
    
    
    
    
    //저지 풀리면 다시 경로탐색 
    
    
    
    /// <summary>
    /// 죽으면 EnemyList에서 Remove 요청
    /// </summary>
    
    [ContextMenu("OnDead")]
    public void OnDead()
    {
        // 죽을 때 적리스트에서 자기 자신을 제거 요청
        currentTile.enemiesOnTile.Remove(this);
        Destroy(gameObject);
    }
    
    
}
