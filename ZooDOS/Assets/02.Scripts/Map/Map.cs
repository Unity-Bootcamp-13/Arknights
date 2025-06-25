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
    [SerializeField] private float _coordOffset = 1.0f; // 월드 좌표 기준 타일 간격

    [Header("타일 프리팹")]
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _hillPrefab;
    [SerializeField] private GameObject _restrictedPrefab;
    [SerializeField] private GameObject _enemyEntryPointPrefab;
    [SerializeField] private GameObject _defensePointPrefab;

    private void Start()
    {
        _map = new Maptile[_mapCoordX, _mapCoordY];
        //Stage 1
        for (int i=0; i< _mapCoordX; ++i)
        {
            for (int j=0; j<_mapCoordY; ++j)
            {
                if (j == 0 || j == 5)
                {
                    PlaceMaptile(_restrictedPrefab, i, j);
                }
                else if (j == 1 || j == 4)
                {
                    PlaceMaptile(_hillPrefab, i, j);
                }
                else
                {
                    if (i==0 && j == 3)
                    {
                        PlaceMaptile(_enemyEntryPointPrefab, i, j);
                    }
                    else if (i==11 && (j==2 || j==3))
                    {
                        PlaceMaptile(_defensePointPrefab, i, j);
                    }
                    else
                    {
                        PlaceMaptile(_groundPrefab, i, j);
                    }
                
                }
            }
        }
    }
    public void PlaceMaptile(GameObject tilePrefab, int positionX, int positionY)
    {
        Vector3 tilePosition = new Vector3(positionX * _coordOffset, 0, positionY * _coordOffset);

        GameObject tileObj = Instantiate(tilePrefab, tilePosition, Quaternion.identity, this.transform); // 생성!
        tileObj.transform.localScale = new Vector3(_coordOffset, _coordOffset, _coordOffset);
        Maptile tile = tileObj.GetComponent<Maptile>(); // 스크립트 가져오기
        tile.InitPosition(positionX, positionY);         // 초기화
        _map[positionX, positionY] = tile;
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
