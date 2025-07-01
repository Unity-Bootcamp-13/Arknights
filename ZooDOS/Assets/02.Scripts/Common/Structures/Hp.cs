using System;

public struct Hp
{
    float _maxHp;
    float _hp;
    bool _isDead;
    Unit _unit;

    public Hp(float hp, Unit unit)
    {
        _maxHp = hp;
        _hp = _maxHp;
        _isDead = false;
        _unit = unit;
    }

    public float HP
    {
        get
        {
            return _hp;
        }

        set
        {
            _hp = Math.Clamp(value, 0, _maxHp);
        }
    }


    public float HpRatio => _hp / _maxHp;
    public bool IsDead => _isDead;
    public float MaxHP => _maxHp;


    public void RefillHp()
    {
        _hp = _maxHp;
    }

    public void GetDamage(float value)
    {
        if (_isDead) return;

        _hp += value;

        if (_hp <= 0)
        {
            OnDeath();
        }
    }

    public void GetHeal(float value)
    {
        if (_isDead) return;

        _hp += value;
    }

    public void OnDeath()
    {
        _isDead = true;
        _unit.OnDeath();

    }
}