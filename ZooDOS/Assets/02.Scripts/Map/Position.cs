using System;

[Serializable]
public struct Position
{
    public int X { get; }
    public int Y { get; }

    // 생성자
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    // == 연산자 오버로드
    public static bool operator ==(Position a, Position b)
    {
        return a.X == b.X && a.Y == b.Y;
    }

    // != 연산자 오버로드
    public static bool operator !=(Position a, Position b)
    {
        return !(a == b);
    }

    // + 연산자 오버로드
    public static Position operator +(Position a, Position b)
    {
        return new Position(a.X + b.X, a.Y + b.Y);
    }

    // - 연산자 오버로드
    public static Position operator -(Position a, Position b)
    {
        return new Position(a.X - b.X, a.Y - b.Y);
    }

    // Equals, GetHashCode 구현 (==, != 비교시 권장)
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