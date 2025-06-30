// 인터페이스 모음
public interface IAttackable
{
    public void Attack(Unit unit);
    public Unit FindTarget();
    public bool IsInRange(Unit unit);
}

