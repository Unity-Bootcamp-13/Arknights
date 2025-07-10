using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Maptile[,] _map;

    const float GROUND_HEIGHT = 0.17f;
    const float HILL_HEIGHT = 0.6f;
    const float RESTRICTED_HEIGHT = 0.17f;

    [Header("맵 크기 조절")]
    [SerializeField] private int _mapCoordX = 12;       //맵의 가로 길이
    [SerializeField] private int _mapCoordY = 6;        //맵의 세로 길이

    [Header("타일 프리팹")]
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _hillPrefab;
    [SerializeField] private GameObject _restrictedPrefab;
    [SerializeField] private GameObject _enemyEntryPointPrefab;
    [SerializeField] private GameObject _defensePointPrefab;
    [SerializeField] private GameObject _restrictedGroundPrefab;

    [SerializeField] private StageTileInfoSO _stageLayoutInfoSO;
    
    
    //타일의 타입에 따른 리스트 저장. start에 호출.
    private Dictionary<TileType, List<Maptile>> _tilesByType;
    private void Start()
    {
        _tilesByType = new();

        _mapCoordX = _stageLayoutInfoSO.MapWidth;
        _mapCoordY = _stageLayoutInfoSO.MapHeight;
        
        _map = new Maptile[_mapCoordX, _mapCoordY];
        
        for (int i = 0; i < _mapCoordX; ++i)
        {
            for (int j = 0; j < _mapCoordY; ++j)
            {
                Position pos = new Position(i, j);
                int index = j * _mapCoordX + i;
                
                TileType tileType = _stageLayoutInfoSO.TileLayout[index];
                switch (tileType)
                {
                    case TileType.Restricted:
                        PlaceMaptile(_restrictedPrefab, pos);
                        break;
                    case TileType.Hill:
                        PlaceMaptile(_hillPrefab, pos);
                        break;
                    case TileType.EnemyEntryPoint:
                        PlaceMaptile(_enemyEntryPointPrefab, pos);
                        break;
                    case TileType.DefensePoint:
                        PlaceMaptile(_defensePointPrefab, pos);
                        break;
                    case TileType.Ground:
                        PlaceMaptile(_groundPrefab, pos);
                        break;
                    case TileType.RestrictedGround:
                        PlaceMaptile(_restrictedGroundPrefab, pos);
                        break;
                }
            }
        }
    }

    public Maptile GetTile(Position pos)
    {
        return _map[pos.X, pos.Y];
    }

    public int GetTileXSize()
    {
        return _map.GetLength(0);
    }
    public int GetTileYSize()
    {
        return _map.GetLength(1);
    }

    public IReadOnlyList<Maptile> GetTilesByType(TileType type)
    {
        if (_tilesByType.TryGetValue(type, out List<Maptile> list))
        {
            return list;
        }
        else
            return Array.Empty<Maptile>();
    }
        

    public void PlaceMaptile(GameObject tilePrefab, Position position)
    {
        Vector3 worldPos = new Vector3(
            position.X * Constants.MAPTILE_LENGTH, 0, position.Y * Constants.MAPTILE_LENGTH);

        GameObject tileObj = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
        tileObj.transform.localScale = Vector3.one * Constants.MAPTILE_LENGTH;

        Maptile tile = tileObj.GetComponent<Maptile>();
        tile.Init(position);
        // 타일 타입을 저장 (그 유닛의 배치에 맞는 타일 return 가능하도록)
        if (!_tilesByType.TryGetValue(tile.TileType, out var list))
        {
            list = new List<Maptile>();
            _tilesByType[tile.TileType] = list;
        }
        list.Add(tile);

        _map[position.X, position.Y] = tile;
    }

    public Vector3 CoordToVector3(Position position)
    {
        if (GetTile(position) == null)
        {
            return new Vector3(-5, -5, -5);
        }
        float height = GetTile(position).TileType switch
        {
            TileType.Ground => GROUND_HEIGHT * Constants.MAPTILE_LENGTH,
            TileType.Hill => HILL_HEIGHT * Constants.MAPTILE_LENGTH,
            TileType.Restricted => RESTRICTED_HEIGHT * Constants.MAPTILE_LENGTH,
            TileType.DefensePoint => GROUND_HEIGHT * Constants.MAPTILE_LENGTH,
            TileType.EnemyEntryPoint => GROUND_HEIGHT * Constants.MAPTILE_LENGTH,
            TileType.RestrictedGround => GROUND_HEIGHT * Constants.MAPTILE_LENGTH,
            _ => 0f
        };

        return new Vector3(Constants.MAPTILE_LENGTH * position.X, height, Constants.MAPTILE_LENGTH * position.Y);
    }

    public Position Vector3ToCoord(Vector3 worldPosition)
    {
        float X = Mathf.Round(worldPosition.x / Constants.MAPTILE_LENGTH);
        float Y = Mathf.Round(worldPosition.z / Constants.MAPTILE_LENGTH);
        Position pos = new Position((int)X, (int)Y);

        if (pos.X < 0 || pos.Y < 0 || pos.X >= _mapCoordX || pos.Y >= _mapCoordY)
        {
            Debug.LogWarning($"World: ({worldPosition.x:F2}, {worldPosition.z:F2}) → Coord: ({pos.X}, {pos.Y})");
        }
        else
        {
            Debug.Log($"World: ({worldPosition.x:F2}, {worldPosition.z:F2}) → Coord: ({pos.X}, {pos.Y})");
        }

        return pos;
    }

    public bool CanPlaceUnitAtGround(Position position)
    {
        if (position.X < 0 || position.Y < 0 || position.X >= _map.GetLength(0) || position.Y >= _map.GetLength(1))
            return false;

        return _map[position.X, position.Y].TileType == TileType.Ground;
    }

    public bool CanPlaceUnitAtHill(Position position)
    {
        if (position.X < 0 || position.Y < 0 || position.X >= _map.GetLength(0) || position.Y >= _map.GetLength(1))
            return false;

        return _map[position.X, position.Y].TileType == TileType.Hill;
    }

    public bool IsInsideMap(Position position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < _mapCoordX && position.Y < _mapCoordY;
    }
}
