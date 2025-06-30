using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenderUnit : PlayerUnit, IAttackable
{
    List<EnemyUnit> _targets;

    public override void OnPlace(List<Maptile> attackRange, Maptile placedTile)
    {
        base.OnPlace(attackRange, placedTile);
        _targets = new List<EnemyUnit>(_resistCapacity);
    }


    protected void Update()
    {
        // 공격 딜레이
        _leftAttackTime += Time.deltaTime;

        if (_leftAttackTime < _atkSpeed)
            return;

        _leftAttackTime = 0;

        _targets.RemoveAll(t => t == null || t.IsDead);

        if (_targets.Count<_resistCapacity)
        {
            EnemyUnit target = FindTarget() as EnemyUnit;
            if (target != null)
            {
                _targets.Add(target);
            }
            Debug.Log("Defender : 적 탐지 중");
        }
        if (_targets.Count>0)
        {
            foreach(EnemyUnit target in _targets)
            {
                if(target.IsDead== false) 
                    Attack(target);
            }
            Debug.Log("Defender : 적 저지 중");
        }
    }

    public void Attack(Unit unit)
    {
        _animator.SetTrigger("Attack_t");
        unit.GetDamage(_atk);
    }

    public Unit FindTarget()
    {
        Unit ClosestUnit = null;
        float ClosestDistance = 9999;
        foreach (Maptile maptile in _attackRange)
        {
            if (maptile.EnemyUnits.Count <= 0)
                continue;

            foreach (EnemyUnit enemyUnit in maptile.EnemyUnits)
            {
                float distance = (enemyUnit.transform.position - transform.position).sqrMagnitude;
                if (distance < ClosestDistance && !_targets.Contains(enemyUnit))
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
