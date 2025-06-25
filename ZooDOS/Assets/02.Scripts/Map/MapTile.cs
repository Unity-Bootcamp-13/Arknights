using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Maptile : MonoBehaviour
{
    [SerializeField]
    private TileType _type = TileType.None;

    private int _tilePositionX;
    private int _tilePositionY;
    private bool _isUnitPlaced = false;

    public TileType TileType
    {
        get => _type;
    }

    /// <summary>
    /// Maptile의 위치를 초기화하기 위한 메서드
    /// </summary>
    /// <param name="positionX">X 좌표</param>
    /// <param name="PositionY">Y 좌표</param>
    public void InitPosition(int positionX, int PositionY)
    {
        _tilePositionX = positionX;
        _tilePositionY = PositionY;
        _isUnitPlaced = false;
    }

    


}
