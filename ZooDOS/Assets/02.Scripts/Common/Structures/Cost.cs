using System;
using UnityEngine;

[Serializable]
public struct Cost
{
    [SerializeField] private int _value;

    public Cost(int value)
    {
        _value = value;
    }

    public int Value => _value;

    public static Cost Zero = new Cost(0);

    public bool IsEnoughFor(Cost price)
    {
        return _value >= price._value;
    }

    public Cost Add(Cost delta, int max = 99) => new Cost(Mathf.Clamp(_value + delta._value, 0, max));
    public Cost Subtract(Cost delta, int min = 0) => new Cost(Mathf.Clamp(_value - delta._value, min, int.MaxValue));
    public Cost RefundHalf() => new Cost(Mathf.CeilToInt(_value * 0.5f));
    public bool Equals(Cost other) => _value == other._value;
    public int CompareTo(Cost other) => _value.CompareTo(other._value);
    public override bool Equals(object obj) => obj is Cost other && Equals(other);
    public override int GetHashCode() => _value; 
    public static bool operator ==(Cost left, Cost right) => left.Equals(right);
    public static bool operator !=(Cost left, Cost right) => !left.Equals(right);

}