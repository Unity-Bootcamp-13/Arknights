using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyUnit : Unit
{
    private Map _map;
    
    public Maptile currentTile;
    
    private List<Position> _path;
    public int currentPathIndex = 0;
    private float rotationSpeed = 10;
    private bool isBlocked = false;

    [SerializeField] private EnemyUnitData _enemyUnitData;
    void Update()
    {
        if (!isBlocked && _path != null && currentPathIndex < _path.Count - 1)
        {
            MoveStep();
        }
    }
    
    
    /// <summary>
    /// 초기값세팅
    /// </summary>
    /// <param name="map">맵 참조</param>
    /// <param name="path">경로</param>
    public void Initialize(Map map, List<Position> path)
    {
        _map = map;
        _path = path;
        currentPathIndex = 0;
        Position startPos = _path[0];   //시작위치
        transform.position = _map.CoordToVector3(startPos); 
        UpdateTile(startPos);
        
    }
    
    /// <summary>
    /// 다음 경로를 향해 이동
    /// </summary>
    private void MoveStep()
    {
        Vector3 current = transform.position;
        Vector3 target = _map.CoordToVector3(_path[currentPathIndex + 1]);  //다음 path 타겟으로 등록
        Vector3 direction = (target - current); //이동방향
        
        if (direction.sqrMagnitude > 0.001f)    //정지시 회전 방지
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        
        transform.position = Vector3.MoveTowards(current, target, _enemyUnitData.MoveSpeed * Time.deltaTime);

        //해당 위치의 맵 타일의 Enemy 리스트 갱신
        if (Vector3.Distance(current, target) < 0.05f)
        {
            currentPathIndex++;
            UpdateTile(_path[currentPathIndex]);
            if (currentPathIndex >= _path.Count - 1)
            {
                OnArriveAtEnd();
            }
        }
    }
    
    
    /// <summary>
    /// 이동시 위치 업데이트
    /// </summary>
    /// <param name="Position">변경할 위치좌표</param>
    public void UpdateTile(Position pos)
    {
        currentTile?.EnemyUnits.Remove(this);
        currentTile = _map.MapTiles[pos.X, pos.Y];
        currentTile.EnemyUnits.Add(this);
    }
    
    
    //저지당하면 정지
    public void Block() => isBlocked = true;
    
    
    //저지 풀리면 다시 경로탐색 
    public void Unblock() => isBlocked = false;

    
    /// <summary>
    /// 죽으면 EnemyList에서 Remove 요청
    /// </summary>
    public override void OnDeath()
    {
        // 죽을 때 적리스트에서 자기 자신을 제거 요청
        currentTile.EnemyUnits.Remove(this);
        Destroy(gameObject);
    }
    
    
    /// <summary>
    /// 적 목적지 도착
    /// </summary>
    private void OnArriveAtEnd()
    {
        // + 라이프 차감 로직 추가 필요
        currentTile?.EnemyUnits.Remove(this);
        Destroy(gameObject);
    }

    
}
