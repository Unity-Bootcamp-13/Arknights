using UnityEngine;

public struct PlayerUnitStatus
{
    public int Id { get; }
    public Sprite ClassIcon { get; }
    public Sprite StandingIllust{get;}
    public string Name{get;}
    public int Cost { get; }
    public float Atk => _atk;
    public float Def => _def;
    public int MaxTarget => _maxTarget;
    public float CurrentHp => _currentHp;
    public float MaxHp => _maxHp;

    private float _atk;
    private float _def;
    private int _maxTarget;
    private float _currentHp;
    private float _maxHp;


    public PlayerUnitStatus(Sprite classIcon, Sprite standingIllust, string name, int id, int cost, float atk =0, float def = 0, int maxTarget = 0, float currentHp = 0, float maxHp = 0)
    {
        ClassIcon = classIcon;
        StandingIllust = standingIllust;
        Name = name;
        Id = id;
        Cost = cost;
        _atk = atk;
        _def = def;
        _maxTarget = maxTarget;
        _currentHp = currentHp;
        _maxHp = maxHp;
    }

    public void SetChangableStatus(float atk, float def, int maxTarget, float currentHp, float maxHp)
    {
        _atk = atk;
        _def = def;
        _maxTarget = maxTarget;
        _currentHp = currentHp;
        _maxHp = maxHp;
    }

}