using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using TMPro.EditorUtilities;

public class Maptile
{
    [SerializeField]
    private TileType _type = TileType.None;
    private Position _tilePosition;
    private PlayerUnit _playerUnit;
    private List<EnemyUnit> _enemyUnits;

    public TileType TileType
    {
        get => _type;
    }

    public PlayerUnit PlayerUnit
    {
        get
        {
            return _playerUnit;
        }
        set
        {
            _playerUnit = value;
        }
    }

    public List<EnemyUnit> EnemyUnits
    {
        get => _enemyUnits;
        set => _enemyUnits = value;
    }
    public Maptile(TileType type, Position position)
    {
        _type = type;
        _tilePosition = position;
        _playerUnit = null;
    }




}
