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
    [SerializeField] private float _coordOffset = 2.0f; // 월드 좌표 기준 타일 간격

    [Header("타일 프리팹")]
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _hillPrefab;
    [SerializeField] private GameObject _restrictedPrefab;
    [SerializeField] private GameObject _enemyEntryPointPrefab;
    [SerializeField] private GameObject _defensePointPrefab;

    [Header("타일 상자")]
    [SerializeField] private GameObject TileContainer;
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
        
        Vector3 tilePosition = new Vector3(position.X * _coordOffset, 0, position.Y * _coordOffset);  //실제 게임오브젝트 좌표 계산
        GameObject tileObj = Instantiate(tilePrefab, tilePosition, Quaternion.identity, this.transform); // 생성!
        tileObj.transform.localScale = new Vector3(_coordOffset, _coordOffset, _coordOffset);
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
    public Vector3 CoordToVector3(int x, int y)
    {
        float height = 0f;
        if (_map[x, y].TileType == TileType.Ground)
        {
            height = 0.03f * _coordOffset;
        }
        else if (_map[x,y].TileType == TileType.Hill)
        {
            height = 0.51f * _coordOffset;
        }
        return new Vector3(_coordOffset * x, height, _coordOffset * y);
    }
    /// <summary>
    /// Vector3 좌표를 받았을때, Coord X, Y 좌표로 변환하는 메서드
    /// </summary>
    /// <param name="worldPosition">실제 World에서의 Position</param>
    /// <returns></returns>
    public (int, int) Vector3ToCoord (Vector3 worldPosition)
    {
        float X = Mathf.Round(worldPosition.x / _coordOffset);
        float Y = Mathf.Round(worldPosition.z / _coordOffset);
        return ((int)X, (int)Y);
    }

    
}
