using System;

public class Hp
{
    float _maxHp;
    float _hp;
    bool _isDead;
    Unit _unit;

    public Action OnHpChanged;


  
    public Hp(Unit unit, float hp)
    {
        _maxHp = hp;
        _hp = _maxHp;
        _isDead = false;
        _unit = unit;
        OnHpChanged = delegate { }; 
    }

    public float HP
    {
        get
        {
            return _hp;
        }

        set
        {
            _hp = value;
            OnHpChanged?.Invoke();
            
        }
    }


    public float HpRatio => _hp / _maxHp;
    public bool IsDead => _isDead;
    public float MaxHP => _maxHp;

    public void SubscribeHpEvent(Action action)
    {
        OnHpChanged += action;
    }

    public void ResetHp()
    {
        HP = _maxHp;
        _isDead = false;
    }

    public void GetDamage(float value)
    {
        if (_isDead) return;

        float newHp = Math.Clamp(_hp - value, 0, _maxHp);
        HP = newHp;

        _unit.OnGetDamage();

        if (_hp <= 0)
        {
            OnDeath();
        }
    }

    public void GetHeal(float value)
    {
        if (_isDead) return;

        float newHp = Math.Clamp(_hp + value, 0, _maxHp);
        HP = newHp;

        _unit.OnGetHeal();
    }

    public void OnDeath()
    {
        if (_isDead) return;

        _isDead = true;
        _unit.OnDeath();

    }
}