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
    /// Maptile�� ��ġ�� �ʱ�ȭ�ϱ� ���� �޼���
    /// </summary>
    /// <param name="positionX">X ��ǥ</param>
    /// <param name="PositionY">Y ��ǥ</param>
    public void InitPosition(int positionX, int PositionY)
    {
        _tilePositionX = positionX;
        _tilePositionY = PositionY;
        _isUnitPlaced = false;
    }

    


}
