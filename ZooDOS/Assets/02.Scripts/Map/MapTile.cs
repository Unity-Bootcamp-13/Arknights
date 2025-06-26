using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Maptile
{
    [SerializeField]
    private TileType _type = TileType.None;
    private Position _tilePosition;
    private bool _isUnitPlaced = false;

    public TileType TileType
    {
        get => _type;
    }

    public Maptile(TileType type, Position position)
    {
        _type = type;
        _tilePosition = position;
        _isUnitPlaced = false;
    }

    /// <summary>
    /// 현재 타일의 점유현황을 변경하기 위한 메서드
    /// </summary>
    /// <param name="isOccupyChange"></param>
    public void OccupyChange(bool isOccupyChange)
    {
        _isUnitPlaced = isOccupyChange;
    }


}
