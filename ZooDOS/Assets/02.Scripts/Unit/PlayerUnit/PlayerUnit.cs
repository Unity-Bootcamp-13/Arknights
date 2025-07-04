﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    [SerializeField] private Canvas _healthUI;
    [SerializeField] private Animator _animator;
    List<Maptile> _attackRange;
    PlayerUnitStatus _status;
    
    private float _leftAttackTime;
    private int _resistCapacity;
    private int _placeCost;
    private float _replaceTime;
    private PlayerUnitType _playerUnitType;
    private TileType _tileType;
    private AttackType _attackType;
    private Projectile _projectilePrefab;



    PlayerUnitBasicAttack _basicAttack;

    
    public TileType TileType => _tileType;

    void Update()
    {

        _basicAttack.AddTarget();

        // 공격 딜레이
        _leftAttackTime += Time.deltaTime;

        if (_leftAttackTime < _atkSpeed)
            return;
        _leftAttackTime = 0;


        _basicAttack.Attack();
    }


    /// <summary>
    /// 배치 시 실행되는 메서드
    /// 공격 딜레이 초기화
    /// 이번 배치에서 지정된 사거리와 위치 지정
    /// 배치되는 타일에 참조 전달
    /// 체력 초기화
    /// 배치 애니메이션 재생
    /// </summary>
    /// <param name="attackRange"></param>
    /// <param name="placedTile"></param>
    public virtual void OnPlace(List<Maptile> attackRange)
    {
        _leftAttackTime = 0;
        _attackRange = attackRange;
        _hp.RefillHp();
        _basicAttack = new PlayerUnitBasicAttack(this, _resistCapacity, _attackRange, _atk, _attackType); 
        
        if (_healthUI != null)
        {
            // 회전을 항상 고정 (예: 정면 고정, XZ 평면 상에서 수평)
            _healthUI.transform.rotation = Quaternion.Euler(0, 270, 0); // 필요 시 다른 각도로 조정 가능
        }

        transform.position += Vector3.up * Constants.FALLING_POS;
        StartCoroutine(C_FallingCoroutine());
    }


    /// <summary>
    /// 유닛 데이터 최초 초기화 함수
    /// 
    /// </summary>
    /// <param name="playerUnitData"> Spawner에게 전달받은 유닛 데이터 </param>
    /// <param name="enemyUnitSpawner"> Spawner에게 전달받은 enemyUnitSpawner 참조 </param>
    public void Init(PlayerUnitData playerUnitData)
    {
        gameObject.SetActive(false);

        _status = new PlayerUnitStatus(playerUnitData.UnitPortrait, playerUnitData.StandingIllust, playerUnitData.Name, playerUnitData.PlaceCost);
        _playerUnitType = playerUnitData.PlayerUnitType;
        _tileType = playerUnitData.TileType;
        _attackType = playerUnitData.AttackType;
        _hp = new Hp(playerUnitData.Hp, this);
        _def = playerUnitData.Def;
        _atk = playerUnitData.Atk;
        _atkSpeed = playerUnitData.AtkSpeed;
        _projectileSpeed = playerUnitData.ProjectileSpeed;
        _resistCapacity = playerUnitData.ResistCapacity;
        _placeCost = playerUnitData.PlaceCost;
        _replaceTime = playerUnitData.ReplaceTime;
        _projectilePrefab = playerUnitData.UnitProjectilePrefab;
    }

    public PlayerUnitStatus GetStatus()
    {
        _status.SetChangableStatus(_atk, _def, _resistCapacity, _hp.HP, _hp.MaxHP);

        return _status;
    }

    public void ShootDamageProjectile(Unit target, float value)
    {
        _animator.SetTrigger("Attack_t");

        StartCoroutine(C_AttackDelay(target, value));
    }


    public void ShootHealProjectile(Unit target, float value)
    {
        _animator.SetTrigger("Attack_t");

        StartCoroutine(C_HealDelay(target, value));
    }

    IEnumerator C_AttackDelay(Unit target, float value)
    {

        yield return new WaitForSeconds(1);
        float damage = Math.Max(1, value - target.Def);

        if (_projectilePrefab != null)
        {
            Projectile projectile = Instantiate(_projectilePrefab);
            projectile.Init(target, _projectileSpeed);
            projectile.transform.position = transform.position + Vector3.up;
            projectile.SetProjectileAction(() => target.Hp.GetDamage(damage));
        }
        else
        {
            target.Hp.GetDamage(damage);
        }
    }

    IEnumerator C_HealDelay(Unit target, float value)
    {
        yield return new WaitForSeconds(1);
        if (_projectilePrefab != null)
        {
            Projectile projectile = Instantiate(_projectilePrefab);
            projectile.Init(target, _projectileSpeed);
            projectile.transform.position = transform.position + Vector3.up;
            projectile.SetProjectileAction(() => target.Hp.GetHeal(value));
        }
        else
        {
            target.Hp.GetHeal(value);
        }
    }

    IEnumerator C_FallingCoroutine()
    {
        _animator.SetTrigger("Place_t");
        float targetY = transform.position.y - Constants.FALLING_POS;

        while (transform.position.y > targetY)
        {
            transform.position += Vector3.down * Time.deltaTime * Constants.FALLING_SPEED;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }


    
    public override void OnDeath()
    {
        if(TileType == TileType.Ground)
        {
            List<Unit> targets = _basicAttack.GetTargets();
            foreach(Unit target in targets)
            {
                EnemyUnit enemy = target as EnemyUnit;
                enemy.Unblock();
            }

        }

        Die?.Invoke(this);
        gameObject.SetActive(false);
    }
}
