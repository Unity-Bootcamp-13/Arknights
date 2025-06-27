// 구조체 모음
public struct PlayerUnitAttackRange
{
    public PlayerUnitAttackRange(float backward, float forward, float sideward)
    {
        _backward = -backward;
        _forward = forward;
        _sideward = sideward;
    }

    public float Backward => _backward;
    public float Forward => _forward;
    public float Sideward => _sideward;

    float _backward;
    float _forward;
    float _sideward;
}

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
            _hp = value;
        }
    }

    public float MaxHP => _maxHp;

    float _maxHp;
    float _hp;

}