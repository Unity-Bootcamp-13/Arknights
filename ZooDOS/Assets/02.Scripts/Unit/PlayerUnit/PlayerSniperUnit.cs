using UnityEngine;

public class PlayerSniperUnit : PlayerUnit, IAttackable
{
    EnemyUnit _target;

    protected void Update()
    {
        _leftAttackTime += Time.deltaTime;

        if (_leftAttackTime < _atkSpeed)
            return;

        _leftAttackTime = 0;

        if (_target == null )
        {
            Debug.Log("Sniper : 적 탐색 중");
            _target = FindTarget() as EnemyUnit;
        }

        if(_target != null)
        {
            if (IsInRange(_target))
            {
                if (_target.IsDead == false)
                    Attack(_target);
                Debug.Log("Sniper : 적 공격 중");
            }
            else
            {
                _target = FindTarget() as EnemyUnit;
            }
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
        foreach(Maptile maptile in _attackRange)
        {
            if (maptile.EnemyUnits.Count <= 0)
                continue;

            foreach (EnemyUnit enemyUnit in maptile.EnemyUnits)
            {
                float distance = (enemyUnit.transform.position - transform.position).sqrMagnitude;
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
