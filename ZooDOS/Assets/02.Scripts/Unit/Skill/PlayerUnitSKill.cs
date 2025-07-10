using UnityEngine;
using System.Collections.Generic;
public class PlayerUnitSKill
{
    List<Unit> _targets;
    PlayerUnit _playerUnit;
    List<Maptile> _attackRange;
    Maptile _currentTile;

    float _atk;
    float _atkSpeed;
    AttackType _attackType;
    int _targetCapacity;

    private float _leftAttackTime;
    Projectile _projectile;


    public PlayerUnitSKill(PlayerUnit playerUnit, float atk, float atkSpeed, SkillData skillData)
    {
        _playerUnit = playerUnit;
        _targetCapacity = skillData.TargetCapacity;
        _targets = new List<Unit>(_targetCapacity);
        _attackRange = new List<Maptile>();
        _atk = atk * skillData.AtkCoefficient;
        _atkSpeed = atkSpeed * skillData.AtkSpeedCoefficient;
        _attackType = skillData.AttackType;
        _projectile = skillData.ProjectilePrefab;
    }

    public void SetRange(List<Maptile> range, Maptile currentTile)
    {
        _attackRange = range;
        _currentTile = currentTile;
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


        
        if (_attackType == AttackType.Damage)
        {
            AttackUnit();
        }
        else if (_attackType == AttackType.Heal)
        {
            HealUnit();
        }
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

                _playerUnit.SetTargetValue(target, _atk, _projectile, _playerUnit.ShootDamageProjectile);

            }
        }
    }

    public void HealUnit()
    {
        if (_targets.Count > 0)
        {
            foreach (Unit target in _targets)
            {
                if (IsInRange(target) == false && target.Hp.IsDead == true)
                {
                    continue;
                }

                _playerUnit.SetTargetValue(target, _atk, _projectile, _playerUnit.ShootHealProjectile);

            }
        }
    }

    private void AddEnemyUnitToTargets()
    {
        UnBlockTargets();
        _targets.Clear();

        for (int i = 0; i < _targetCapacity; i++)
        {
            EnemyUnit target = FindEnemyUnitTarget();
            if (target != null && _targets.Contains(target) == false)
            {
                _targets.Insert(0, target);
            }
        }
        if (_playerUnit.TileType == TileType.Ground && _currentTile.GetEnemyUnits().Count > 0)
        {
            for (int i = 0; i < _targetCapacity; i++)
            {
                BlockTarget(i);
            }
        }
    }

    private void AddPlayerUnitToTargets()
    {
        _targets.Clear();

        for (int i = 0; i < _targetCapacity; i++)
        {
            PlayerUnit target = FindPlayerUnitTarget();
            if (target != null && _targets.Contains(target) == false)
            {
                _targets.Insert(0, target);
            }
        }

        _targets.RemoveAll(t=>t.Hp.HpRatio == 1);
    }

    public PlayerUnit FindPlayerUnitTarget()
    {
        PlayerUnit lowestHpUnit = null;
        float lowestHpRatio = 1f;
        foreach (Maptile maptile in _attackRange)
        {
            PlayerUnit target = maptile.GetPlayerUnit();

            if (target == null || target.Hp.IsDead || _targets.Contains(target))
            {
                continue;
            }

            float ratio = target.Hp.HpRatio;

            if (ratio < 1f && ratio < lowestHpRatio)
            {
                lowestHpUnit = target;
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

                if (enemyUnit == null || enemyUnit.Hp.IsDead|| _targets.Contains(enemyUnit))
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

    public void BlockTarget(int i)
    {
        if(i>= _currentTile.GetEnemyUnits().Count)
        {
            return;
        }

        EnemyUnit unit = _currentTile.GetEnemyUnits()[i] as EnemyUnit;
        

       if (unit != null)
       {
            unit.Block(_playerUnit);
       }
        
    }

    public void UnBlockTargets()
    {
        foreach (Unit target in _targets)
        {
            EnemyUnit enemy = target as EnemyUnit;
            if(enemy == null)
            {
                continue;
            }

            enemy.Unblock();
        }
    }

    public bool IsAnyTargetInRange()
    {
        AddTarget();
        if (_targets.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
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

