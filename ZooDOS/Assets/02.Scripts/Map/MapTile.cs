using UnityEngine;
using System.Collections.Generic;

public class Maptile : MonoBehaviour
{
    [SerializeField] private TileType _type = TileType.None;
    private Position _tilePosition;
    private PlayerUnit _playerUnit;
    private List<Unit> _enemyUnits = new List<Unit>();

    public TileType TileType => _type;

    public Position GetPosition()
    {
        return _tilePosition;
    }
    public void SetPlayerUnit(PlayerUnit unit)
    {
        if (_playerUnit != null)
            _playerUnit.Die -= HandlePlayerDeath;  

        _playerUnit = unit;

        if (_playerUnit != null)
            _playerUnit.Die += HandlePlayerDeath;
    }

    public PlayerUnit GetPlayerUnit()
    {
        return _playerUnit;
    }

    public void PlayerDeath()
    {
        _playerUnit.Die -= HandlePlayerDeath;
        _playerUnit = null;
    }

    public void Init(Position position)
    {
        _tilePosition = position;
        _playerUnit = null;
    }

    public void AddEnemyUnit(Unit enemy)
    {
        if (!_enemyUnits.Contains(enemy)) // 중복 방지
        {
            _enemyUnits.Add(enemy);
            enemy.Die += HandleEnemyDeath;
        }
    }

    public void RemoveEnemyUnit(Unit enemy)
    {
        _enemyUnits.Remove(enemy);
        enemy.Die -= HandleEnemyDeath;
    }

    public bool HasEnemyUnit(Unit enemy)
    {
        return _enemyUnits.Contains(enemy);
    }

    public IReadOnlyList<Unit> GetEnemyUnits()
    {
        _enemyUnits.RemoveAll(t => t == null);
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
    private void HandlePlayerDeath(Unit unit)
    {
        if (unit != _playerUnit)
        {
            return;
        }
        _playerUnit.Die -= HandlePlayerDeath;
        _playerUnit = null;
    }

    private void HandleEnemyDeath(Unit unit)
    {
        if (HasEnemyUnit(unit))
        {
            RemoveEnemyUnit(unit);
        }
    }
}
