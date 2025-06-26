using System;

[Serializable]
public struct Position
{
    public int X { get; }
    public int Y { get; }

    // ������
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    // == ������ �����ε�
    public static bool operator ==(Position a, Position b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    // != ������ �����ε�
    public static bool operator !=(Position a, Position b)
    {
        return !(a == b);
    }

    // + ������ �����ε�
    public static Position operator +(Position a, Position b)
    {
        return new Position(a.X + b.X, a.Y + b.Y);
    }

    // - ������ �����ε�
    public static Position operator -(Position a, Position b)
    {
        return new Position(a.X - b.X, a.Y - b.Y);
    }

    // Equals, GetHashCode ���� (==, != �񱳽� ����)
    public override bool Equals(object obj)
    {
        if (obj is Position pos)
        {
            return this == pos;
        }
        return false;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}