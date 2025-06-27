using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Maptile[,] _map;

    public Maptile[,] MapTiles
    {
        get => _map;  
    }

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
        for (int i=0; i< _mapCoordX; ++i)
        {
            for (int j=0; j<_mapCoordY; ++j)
            {
                Position pos = new Position(i, j);
                if (j == 0 || j == 5)
                {
                    PlaceMaptile(_restrictedPrefab,TileType.Restricted, pos);
                }
                else if (j == 1 || j == 4)
                {
                    PlaceMaptile(_hillPrefab, TileType.Hill, pos);
                }
                else
                {
                    if (i==0 && j == 3)
                    {
                        PlaceMaptile(_enemyEntryPointPrefab, TileType.EnemyEntryPoint, pos);
                    }
                    else if (i==11 && (j==2 || j==3))
                    {
                        PlaceMaptile(_defensePointPrefab, TileType.DefensePoint, pos);
                    }
                    else
                    {
                        PlaceMaptile(_groundPrefab, TileType.Ground, pos);
                    }
                
                }
            }
        }
    }
    /// <summary>
    /// 타일을 깔고, _map으로 타일맵의 속성을 초기화하는 메서드
    /// </summary>
    /// <param name="tilePrefab"></param>
    /// <param name="tileType"></param>
    /// <param name="position"></param>
    public void PlaceMaptile(GameObject tilePrefab, TileType tileType, Position position)
    {
        
        Vector3 tilePosition = new Vector3(position.X * Constants.MAPTILE_LENGTH, 0, position.Y * Constants.MAPTILE_LENGTH);  //실제 게임오브젝트 좌표 계산
        GameObject tileObj = Instantiate(tilePrefab, tilePosition, Quaternion.identity, this.transform); // 생성!
        tileObj.transform.localScale = new Vector3(Constants.MAPTILE_LENGTH, Constants.MAPTILE_LENGTH, Constants.MAPTILE_LENGTH);
        //Maptile 생성 및 초기화
        Maptile tile = new Maptile(tileType, position);
        _map[position.X, position.Y] = tile;
    }
    
    /// <summary>
    /// Coord 좌표를 Vector3 좌표로 변환하는 메서드
    /// </summary>
    /// <param name="x">맵 좌표의 X값</param>
    /// <param name="y">맵 좌표의 Y값</param>
    /// <returns></returns>
    public Vector3 CoordToVector3(Position position)
    {
        float height = 0f;
        if (_map[position.X, position.Y].TileType == TileType.Ground)
        {
            height = 0.03f * Constants.MAPTILE_LENGTH;
        }
        else if (_map[position.X,position.Y].TileType == TileType.Hill)
        {
            height = 0.51f * Constants.MAPTILE_LENGTH;
        }
        return new Vector3(Constants.MAPTILE_LENGTH * position.X, height, Constants.MAPTILE_LENGTH * position.Y);
    }
    /// <summary>
    /// Vector3 좌표를 받았을때, Coord X, Y 좌표로 변환하는 메서드
    /// </summary>
    /// <param name="worldPosition">실제 World에서의 Position</param>
    /// <returns></returns>
    public Position Vector3ToCoord (Vector3 worldPosition)
    {
        float X = Mathf.Round(worldPosition.x / Constants.MAPTILE_LENGTH);
        float Y = Mathf.Round(worldPosition.z / Constants.MAPTILE_LENGTH);
        Position pos = new Position((int)X, (int)Y);
        return pos;
    }



    public bool CanPlaceUnitAtGround(Position pos)
    {
        if (pos.X < 0 || pos.Y < 0 || pos.X >= _map.GetLength(0) || pos.Y >= _map.GetLength(1))
            return false;

        return _map[pos.X, pos.Y].TileType == TileType.Ground;
    }

    public bool CanPlaceUnitAtHill(Position pos)
    {
        if (pos.X < 0 || pos.Y < 0 || pos.X >= _map.GetLength(0) || pos.Y >= _map.GetLength(1))
            return false;

        return _map[pos.X, pos.Y].TileType == TileType.Ground;
    }
}
