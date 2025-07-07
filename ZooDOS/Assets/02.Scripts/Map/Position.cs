using System;
using UnityEngine;

[Serializable]
public struct Position
{
    [SerializeField] private int x;
    [SerializeField] private int y;

    public int X => x;
    public int Y => y;

    public Position(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public bool IsValid => !(X == 0 && Y == 0); 

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