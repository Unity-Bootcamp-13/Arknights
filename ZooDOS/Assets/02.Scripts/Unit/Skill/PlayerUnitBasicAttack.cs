using UnityEngine;
using System.Collections.Generic;
using System;

public class PlayerUnitBasicAttack
{
    List<Unit> _targets;
    PlayerUnit _playerUnit;
    List<Maptile> _attackRange;

    float _atk;
    AttackType _attackType;

    public PlayerUnitBasicAttack(PlayerUnit playerUnit, int resistCapacity, List<Maptile> attackRange, float atk, AttackType attackType)
    {
        _playerUnit = playerUnit;
        _targets = new List<Unit>(resistCapacity);
        _attackRange = attackRange;
        _atk = atk;
        _attackType = attackType;
    }

    public void Attack()
    {
        if(_attackType == AttackType.Damage)
        {
            AttackToEnemyUnit();
        }
        else if(_attackType == AttackType.Heal)
        {
            HealToPlayerUnit();
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// FindEnemyUnitTarget + ShooDamageProjectile
    ///  = 적 타겟 탐지 + 공격 투사체 발사 
    ///  = 적에게 공격
    /// </summary>
    public void AttackToEnemyUnit()
    {
        _targets.RemoveAll(t => t == null || t.Hp.IsDead);

        if (_targets.Count < _targets.Capacity)
        {
            EnemyUnit target = FindEnemyUnitTarget() as EnemyUnit;
            if (target != null && _targets.Contains(target)==false)
            {
                _targets.Add(target);
            }
            Debug.Log("적 탐지 중");
        }
        if (_targets.Count > 0)
        {
            foreach (EnemyUnit target in _targets)
            {
                if (IsInRange(target) == false)
                {
                    continue;
                }

                if (target.Hp.IsDead == false)
                {

                    _playerUnit.ShootDamageProjectile(target, _atk);
                }

            }
            Debug.Log("적 공격 중");
        }
    }

    /// <summary>
    /// FindPlayerUnitTarget + ShooHealProjectile
    /// = 아군 타겟 탐지 + 공격 투사체 발사
    /// = 아군에게 힐
    /// </summary>
    public void HealToPlayerUnit()
    {
        _targets.RemoveAll(t => t == null || t.Hp.IsDead);

        if (_targets.Count < _targets.Capacity)
        {
            PlayerUnit target = FindPlayerUnitTarget() as PlayerUnit;
            if (target != null && _targets.Contains(target) == false)
            {
                _targets.Add(target);
            }
            Debug.Log("아군 탐지 중");
        }
        if (_targets.Count > 0)
        {
            foreach (PlayerUnit target in _targets)
            {
                if (IsInRange(target) == false)
                {
                    continue;
                }

                if (target.Hp.IsDead == false)
                {

                    _playerUnit.ShootHealProjectile(target, _atk);
                }

            }
            Debug.Log("아군 회복 중");
        }
    }



    public Unit FindPlayerUnitTarget()
    {
        Unit lowestHpUnit = null;
        float lowestHpRatio = 1f;
        foreach (Maptile maptile in _attackRange)
        {
            if (maptile.PlayerUnit == null)
            {
                continue;
            }

            float ratio = maptile.PlayerUnit.Hp.HpRatio;

            if (ratio < 1f && ratio < lowestHpRatio)
            {
                lowestHpUnit = maptile.PlayerUnit;
                lowestHpRatio = ratio;
            }
        }

        return lowestHpUnit;
    }

    public Unit FindEnemyUnitTarget()
    {
        Unit ClosestUnit = null;
        float ClosestDistance = 9999;
        foreach (Maptile maptile in _attackRange)
        {
            if (maptile.EnemyUnits.Count <= 0)
            {
                continue;
            }
            foreach (EnemyUnit enemyUnit in maptile.EnemyUnits)
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


    public bool IsInRange(Unit target)
    {
        EnemyUnit enemy = target as EnemyUnit;
        foreach (Maptile maptile in _attackRange)
        {
            if (maptile.EnemyUnits.Contains(enemy))
            {
                return true;
            }
        }

        return false;
    }
}

