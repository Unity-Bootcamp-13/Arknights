// 인터페이스 모음
public interface IAttackable
{
    public Unit FindTarget();
    public bool IsInRange(Unit unit);
}

public interface IDamagable
{
    public void GetDamage();
}

