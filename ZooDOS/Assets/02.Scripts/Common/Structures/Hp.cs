using System;

public struct Hp
{
    public Hp(float hp)
    {
        _maxHp = hp;
        _hp = _maxHp;
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

    public void Replace()
    {
        _hp = _maxHp;
    }

    public float MaxHP => _maxHp;

    float _maxHp;
    float _hp;

}