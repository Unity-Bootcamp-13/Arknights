using UnityEngine;

public class PlayerMedicUnit : PlayerUnit, IAttackable
{
    PlayerUnit _target;

    protected void Update()
    {
        _leftAttackTime += Time.deltaTime;

        if (_leftAttackTime < _atkSpeed)
            return;

        _leftAttackTime = 0;

        _target = FindTarget() as PlayerUnit;
        Debug.Log("Medic : 아군 탐색 중");


        if (_target != null)
        {
            Attack(_target);
            Debug.Log("Medic : 아군 회복 중");
        }
    }

    public void Attack(Unit unit)
    {
        _animator.SetTrigger("Attack_t");
        unit.GetDamage(-_atk);
    }

    public Unit FindTarget()
    {
        Unit lowestHpUnit = null;
        float lowestHpRatio = 1f;
        foreach (Maptile maptile in _attackRange)
        {
            if (maptile.PlayerUnit == null)
                continue;

            float ratio = maptile.PlayerUnit.HpRatio;
            if (ratio < 1f && ratio < lowestHpRatio)
            {
                lowestHpUnit = maptile.PlayerUnit;
                lowestHpRatio = ratio;
            }
        }

        return lowestHpUnit;
    }

    public bool IsInRange(Unit target)
    {
        PlayerUnit playerUnit = target as PlayerUnit;
        foreach (Maptile maptile in _attackRange)
        {
            if (maptile.PlayerUnit == playerUnit)
            {
                return true;
            }
        }
        return false;
    }
}
