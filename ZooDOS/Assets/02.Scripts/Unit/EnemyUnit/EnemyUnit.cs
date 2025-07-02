using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyUnit : Unit
{
    
    private List<Vector3> _path;
    public int currentPathIndex = 0;
    private float rotationSpeed = 10;
    private bool isBlocked = false;

    private EnemyUnitType _enemyUnitType;
    protected Projectile _projectilePrefab;
    protected float _RangeRadius;

    

    [SerializeField] private EnemyUnitData _enemyUnitData;
    void Update()
    {
        if (!isBlocked)
        {
            MoveStep();
        }
    }
    
    
    /// <summary>
    /// 초기값세팅
    /// </summary>
    /// <param name="path">경로</param>
    public void Initialize(List<Vector3> path)
    {
        _path = path;
        currentPathIndex = 0;
        transform.position = _path[0]; 
        
        _hp = new Hp(_enemyUnitData.Hp, this);
        _def = _enemyUnitData.Def;
        _atk = _enemyUnitData.Atk;
        _atkSpeed = _enemyUnitData.AtkSpeed;
        _enemyUnitType = _enemyUnitData.EnemyUnitType;
        _RangeRadius = _enemyUnitData.RangeRadius;
        _projectilePrefab = _enemyUnitData.ProjectilePrefab;
        _projectileSpeed = _enemyUnitData.ProjectileSpeed;
        
        
    }
    
    /// <summary>
    /// 다음 경로를 향해 이동
    /// </summary>
    private void MoveStep()
    {
        Vector3 current = transform.position;
        Vector3 target =_path[currentPathIndex + 1];
        Vector3 direction = (target - current); //이동방향
        
        if (direction.sqrMagnitude > 0.001f)    //정지시 회전 방지
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        
        transform.position = Vector3.MoveTowards(current, target, _enemyUnitData.MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(current, target) < 0.05f)
        {
            currentPathIndex++;
            if (currentPathIndex >= _path.Count - 1)
            {
                OnArriveAtEnd();
            }
        }
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
        Die?.Invoke(this);
        Destroy(gameObject);
    }
    
    
    /// <summary>
    /// 적 목적지 도착
    /// </summary>
    private void OnArriveAtEnd()
    {
        // + 라이프 차감 로직 추가 필요
        Die?.Invoke(this);
        Destroy(gameObject);
    }

    
}
