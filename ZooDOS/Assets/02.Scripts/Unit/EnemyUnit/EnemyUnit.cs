﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    
    protected List<Vector3> _path;
    public int currentPathIndex = 0;
    private float rotationSpeed = 10;
    private bool isBlocked = false;


    private List<Unit> _targetList = new List<Unit>();

    private float _attackTime = 0;
    private float _attackAfterMoveTime = 1;

    [SerializeField] private EnemyUnitData _enemyUnitData;
    private Animator _animator;

    public EnemyUnitData EnemyUnitData => _enemyUnitData;
    
    public event Action OnArrived;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_targetList.Count > 0 )
        {
            //원거리유닛 공격하고 0.3초 ~0.6초간 움직임
            if (!isBlocked
                &&_enemyUnitData.EnemyUnitType == EnemyUnitType.LongRange
                && (_attackAfterMoveTime > 1f && _attackAfterMoveTime < _enemyUnitData.AtkSpeed))
            {
                _attackAfterMoveTime += Time.deltaTime;
                _animator.SetFloat("Speed_f", 1f);
                MoveStep();
                _attackTime += Time.deltaTime;
                return;
            }
            
            _animator.SetFloat("Speed_f", 0f);
            if (_attackTime > _enemyUnitData.AtkSpeed)
            {
                _attackTime = 0;
                _animator.SetTrigger("Attack_t");
                Attack();
            }
            _attackAfterMoveTime += Time.deltaTime;
            _attackTime += Time.deltaTime;
        }
        else if (!isBlocked)
        {
            _animator.SetFloat("Speed_f", 1f);
            MoveStep();
        }
    }
    
    
    /// <summary>
    /// 초기값세팅
    /// </summary>
    /// <param name="path">경로</param>
    public virtual void Init(List<Vector3> path) {
        _path = path;
        currentPathIndex = 0;
        transform.position = _path[0]; 
        
        _hp = new Hp(this, _enemyUnitData.Hp);
        _def = _enemyUnitData.Def;
        _atk = _enemyUnitData.Atk;
        _atkSpeed = _enemyUnitData.AtkSpeed;
        _projectileSpeed = _enemyUnitData.ProjectileSpeed;
        
    }
    
    /// <summary>
    /// 다음 경로를 향해 이동
    /// </summary>
    protected virtual void MoveStep()
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

        _unitHpUI.SetUIPosition(transform.position);

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
    public void Block(PlayerUnit unit)
    {
        SetPlayerUnit(unit);
        isBlocked = true;

    }


    //저지 풀리면 다시 경로탐색 
    public void Unblock(Unit unit)
    {
        RemovePlayerUnit(unit);
        isBlocked = false;
    }


    /// <summary>
    /// 죽으면 EnemyList에서 Remove 요청
    /// </summary>
    public override void OnDeath()
    {
        base.OnDeath();

        _sfxSound.PlaySFXSound("Death");
        Die?.Invoke(this);
        Destroy(gameObject);
    }
    
    
    /// <summary>
    /// 적 목적지 도착
    /// </summary>
    protected virtual void OnArriveAtEnd()
    {
        base.OnDeath();
        Die?.Invoke(this);
        OnArrived?.Invoke();
        Destroy(gameObject);
    }


    public void Attack()
    {
        Unit _target = LastDeployUnit(); 
        float dmg = Math.Max(1,_atk - _target.Def);

        _sfxSound.PlaySFXSound("Attack");

        if (_enemyUnitData.ProjectilePrefab != null)
        {
            Projectile projectile = Instantiate(_enemyUnitData.ProjectilePrefab).GetComponent<Projectile>();
            projectile.Init(_target, _projectileSpeed);
            projectile.transform.position = transform.position;
            projectile.SetProjectileAction(() => _target.Hp.GetDamage(dmg));
        }
        else
        {
            _target.Hp.GetDamage(dmg);
        }
        //Debug.Log("적 -> 아군 공격");
        _attackAfterMoveTime = 0;

    }
    public void SetPlayerUnit(PlayerUnit unit)
    {
        if (!_targetList.Contains(unit)) // 중복 방지
        {
            _targetList.Add(unit);
            unit.Die += RemovePlayerUnit;
        }
    }
    public void RemovePlayerUnit(Unit unit)
    {
        _targetList.Remove(unit);
        unit.Die -= RemovePlayerUnit;

    }

    public Unit LastDeployUnit()
    {
        Unit last = null;
        if (_targetList.Count > 0)
        {
            last = _targetList[_targetList.Count - 1];
        }
        return last;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(_enemyUnitData.EnemyUnitType == EnemyUnitType.ShortRange)
        {
            return;
        }

        if (other.TryGetComponent(out PlayerUnit unit))
        {
            SetPlayerUnit(unit);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (_enemyUnitData.EnemyUnitType == EnemyUnitType.ShortRange)
        {
            return;
        }

        if (other.TryGetComponent(out EnemyUnit enemy))
        {
            RemovePlayerUnit(enemy);
        }



    }


}
