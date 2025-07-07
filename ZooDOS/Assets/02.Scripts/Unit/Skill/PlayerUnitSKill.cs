using UnityEngine;
using System.Collections.Generic;

public class PlayerUnitSKill
{
    List<Unit> _targets;
    PlayerUnit _playerUnit;
    List<Maptile> _attackRange;

    float _atk;
    float _atkSpeed;
    AttackType _attackType;

    private float _leftAttackTime;
    Projectile _projectile;


    public PlayerUnitSKill(PlayerUnit playerUnit, float atk, float atkSpeed, SkillData skillData)
    {
        _playerUnit = playerUnit;
        _targets = new List<Unit>(skillData.TargetCapacity);
        _attackRange = new List<Maptile>();
        _atk = atk * skillData.AtkCoefficient;
        _atkSpeed = atkSpeed * skillData.AtkSpeedCoefficient;
        _attackType = skillData.AttackType;
        _projectile = skillData.ProjectilePrefab;
    }

    public void SetRange(List<Maptile> range)
    {
        _attackRange = range;
    }

    public void Init()
    {
        _leftAttackTime = 0;
    }

    public void Attack()
    {
        // 공격 딜레이
        _leftAttackTime += Time.deltaTime;

        if (_leftAttackTime < _atkSpeed)
        {
            return;
        }

        _leftAttackTime = 0;


        AttackUnit();
    }

    public void AddTarget()
    {
        if (_attackType == AttackType.Damage)
        {
            AddEnemyUnitToTargets();
        }
        else if (_attackType == AttackType.Heal)
        {
            AddPlayerUnitToTargets();
        }
        else
        {
            return;
        }
    }

    public void AttackUnit()
    {
        if (_targets.Count > 0)
        {
            foreach (Unit target in _targets)
            {
                if (IsInRange(target) == false && target.Hp.IsDead == true)
                {
                    continue;
                }

                if(_attackType == AttackType.Damage)
                {
                    _playerUnit.ShootDamageProjectile(target, _atk, _projectile);
                }

                if(_attackType == AttackType.Heal)
                {
                    _playerUnit.ShootHealProjectile(target, _atk, _projectile);
                }
            }
        }
    }

    private void AddEnemyUnitToTargets()
    {
        _targets.RemoveAll(t => t == null || t.Hp.IsDead);

        if (_targets.Count < _targets.Capacity)
        {
            EnemyUnit target = FindEnemyUnitTarget();
            if (target != null && _targets.Contains(target) == false)
            {
                _targets.Add(target);
            }

        }

        if (_playerUnit.TileType == TileType.Ground)
        {
            foreach (EnemyUnit target in _targets)
            {
                target.Block(_playerUnit);
            }
        }
    }

   

    private void AddPlayerUnitToTargets()
    {
        _targets.RemoveAll(t => t == null || t.Hp.IsDead);

        if (_targets.Count < _targets.Capacity)
        {
            PlayerUnit target = FindPlayerUnitTarget();
            if (target != null && _targets.Contains(target) == false)
            {
                _targets.Add(target);
            }
        }
    }

    public PlayerUnit FindPlayerUnitTarget()
    {
        PlayerUnit lowestHpUnit = null;
        float lowestHpRatio = 1f;
        foreach (Maptile maptile in _attackRange)
        {
            if (maptile.GetPlayerUnit() == null)
            {
                continue;
            }

            float ratio = maptile.GetPlayerUnit().Hp.HpRatio;

            if (ratio < 1f && ratio < lowestHpRatio)
            {
                lowestHpUnit = maptile.GetPlayerUnit();
                lowestHpRatio = ratio;
            }
        }

        return lowestHpUnit;
    }

    public EnemyUnit FindEnemyUnitTarget()
    {
        EnemyUnit ClosestUnit = null;
        float ClosestDistance = 9999;
        foreach (Maptile maptile in _attackRange)
        {
            if (maptile.GetEnemyUnits().Count <= 0)
            {
                continue;
            }
            foreach (EnemyUnit enemyUnit in maptile.GetEnemyUnits())
            {

                if (enemyUnit == null || enemyUnit.Hp.IsDead)
                {
                    continue;
                }

                float distance = (enemyUnit.transform.position - _playerUnit.transform.position).sqrMagnitude;

                if (distance < ClosestDistance)
                {
                    ClosestUnit = enemyUnit;
                    ClosestDistance = distance;
                }
            }
        }
        return ClosestUnit;
    }

    public void UnBlockTargets()
    {
        if (_attackType == AttackType.Heal || _attackType == AttackType.Nothing || _playerUnit.TileType != TileType.Ground)
        {
            return;
        }

        foreach (Unit target in _targets)
        {
            EnemyUnit enemy = target as EnemyUnit;
            enemy.Unblock();
        }
    }

    public bool IsInRange(Unit target)
    {
        EnemyUnit enemy = target as EnemyUnit;
        foreach (Maptile maptile in _attackRange)
        {
            if (maptile.HasEnemyUnit(enemy))
            {
                return true;
            }
        }

        return false;
    }
}

