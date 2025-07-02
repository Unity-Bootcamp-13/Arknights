using UnityEngine;
using System.Collections.Generic;

public class Maptile : MonoBehaviour
{
    [SerializeField] private TileType _type = TileType.None;
    private Position _tilePosition;
    private PlayerUnit _playerUnit;
    private List<EnemyUnit> _enemyUnits = new List<EnemyUnit>();

    public TileType TileType => _type;

    public Position GetPosition()
    {
        return _tilePosition;
    }
    public void SetPlayerUnit(PlayerUnit unit)
    {
        _playerUnit = unit;
    }

    public PlayerUnit GetPlayerUnit()
    {
        return _playerUnit;
    }

    public void PlayerDeath()
    {
        _playerUnit = null;
    }

    public void Init(Position position)
    {
        _tilePosition = position;
        _playerUnit = null;
    }

    public void AddEnemyUnit(EnemyUnit enemy)
    {
        if (!_enemyUnits.Contains(enemy)) // 중복 방지
        {
            _enemyUnits.Add(enemy);
        }
    }

    public void RemoveEnemyUnit(EnemyUnit enemy)
    {
        _enemyUnits.Remove(enemy);
    }

    public bool HasEnemyUnit(EnemyUnit enemy)
    {
        return _enemyUnits.Contains(enemy);
    }

    public IReadOnlyList<EnemyUnit> GetEnemyUnits()
    {
        return _enemyUnits.AsReadOnly();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out EnemyUnit enemy))
        {
            AddEnemyUnit(enemy);
        }
        if (other.TryGetComponent(out PlayerUnit unit))
        {
            SetPlayerUnit(unit);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out EnemyUnit enemy))
        {
            RemoveEnemyUnit(enemy);
        }
    }
}
