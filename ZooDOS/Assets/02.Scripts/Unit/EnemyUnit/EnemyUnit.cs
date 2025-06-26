using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public EnemyUnitSpawner _spawner;
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
    
    
    //저지당하면 정지
    
    
    
    
    //풀리면 다시 경로탐색
    
    
    
    //죽으면 EnemyList에서 Remove 요청
    public void OnDead()
    {
        // 죽을 때 자기 자신을 제거 요청
        _spawner.RemoveEnemy(this);
        Destroy(gameObject);
    }
    
    
}
