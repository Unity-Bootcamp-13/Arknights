using UnityEngine;
using System;
using System.Collections.Generic;

public class Maptile : MonoBehaviour
{
    private TileType _type = TileType.None;
    private (int, int) _tilePosition;
    private bool _isUnitPlaced = false;

    public void Init(TileType type, (int, int) position)
    {
        _type = type;
        _tilePosition = position;
        _isUnitPlaced = false;
    }
}
