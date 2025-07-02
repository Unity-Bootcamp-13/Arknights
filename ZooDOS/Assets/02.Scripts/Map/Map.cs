using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    private Maptile[,] _map;

    const float GROUND_HEIGHT = 0.03f;
    const float HILL_HEIGHT = 0.31f;
    const float RESTRICTED_HEIGHT = 0.61f;

    [Header("맵 크기 조절")]
    [SerializeField] private int _mapCoordX = 12;       //맵의 가로 길이
    [SerializeField] private int _mapCoordY = 6;        //맵의 세로 길이

    [Header("타일 프리팹")]
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _hillPrefab;
    [SerializeField] private GameObject _restrictedPrefab;
    [SerializeField] private GameObject _enemyEntryPointPrefab;
    [SerializeField] private GameObject _defensePointPrefab;

    private void Start()
    {
        _map = new Maptile[_mapCoordX, _mapCoordY];
        //for문 내부는 Stage 1 구현부 (임시 맵 배치). 추후 함수 추가하여 SO 파일로 편집을 편하게 확장성을 가질 예정
        for (int i = 0; i < _mapCoordX; ++i)
        {
            for (int j = 0; j < _mapCoordY; ++j)
            {
                Position pos = new Position(i, j);
                if (j == 0 || j == 5)
                {
                    PlaceMaptile(_restrictedPrefab, pos);
                }
                else if (j == 1 || j == 4)
                {
                    PlaceMaptile(_hillPrefab, pos);
                }
                else
                {
                    if (i == 0 && j == 3)
                    {
                        PlaceMaptile(_enemyEntryPointPrefab, pos);
                    }
                    else if (i == 11 && (j == 2 || j == 3))
                    {
                        PlaceMaptile(_defensePointPrefab, pos);
                    }
                    else
                    {
                        PlaceMaptile(_groundPrefab, pos);
                    }
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
    public void PlaceMaptile(GameObject tilePrefab, Position position)
    {
        Vector3 worldPos = new Vector3(
            position.X * Constants.MAPTILE_LENGTH, 0, position.Y * Constants.MAPTILE_LENGTH);

        GameObject tileObj = Instantiate(tilePrefab, worldPos, Quaternion.identity, transform);
        tileObj.transform.localScale = Vector3.one * Constants.MAPTILE_LENGTH;

        Maptile tile = tileObj.GetComponent<Maptile>();
        tile.Init(position);
        _map[position.X, position.Y] = tile;
    }

    public Vector3 CoordToVector3(Position position)
    {
        if (GetTile(position) == null)
        {
            return Vector3.zero;
        }
        float height = GetTile(position).TileType switch
        {
            TileType.Ground => GROUND_HEIGHT * Constants.MAPTILE_LENGTH,
            TileType.Hill => HILL_HEIGHT * Constants.MAPTILE_LENGTH,
            TileType.Restricted => RESTRICTED_HEIGHT * Constants.MAPTILE_LENGTH,
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
