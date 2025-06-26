using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Map : MonoBehaviour
{
    private Maptile[,] _map;

    public Maptile[,] MapTiles
    {
        get => _map;  
    }

    [Header("�� ũ�� ����")]
    [SerializeField] private int _mapCoordX = 12;       //���� ���� ����
    [SerializeField] private int _mapCoordY = 6;        //���� ���� ����
    [SerializeField] private float _coordOffset = 2.0f; // ���� ��ǥ ���� Ÿ�� ����

    [Header("Ÿ�� ������")]
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _hillPrefab;
    [SerializeField] private GameObject _restrictedPrefab;
    [SerializeField] private GameObject _enemyEntryPointPrefab;
    [SerializeField] private GameObject _defensePointPrefab;

    [Header("Ÿ�� ����")]
    [SerializeField] private GameObject TileContainer;
    private void Start()
    {
        _map = new Maptile[_mapCoordX, _mapCoordY];
        //for�� ���δ� Stage 1 ������ (�ӽ� �� ��ġ). ���� �Լ� �߰��Ͽ� SO ���Ϸ� ������ ���ϰ� Ȯ�强�� ���� ����
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
    /// Ÿ���� ���, _map���� Ÿ�ϸ��� �Ӽ��� �ʱ�ȭ�ϴ� �޼���
    /// </summary>
    /// <param name="tilePrefab"></param>
    /// <param name="tileType"></param>
    /// <param name="position"></param>
    public void PlaceMaptile(GameObject tilePrefab, TileType tileType, Position position)
    {
        
        Vector3 tilePosition = new Vector3(position.X * _coordOffset, 0, position.Y * _coordOffset);  //���� ���ӿ�����Ʈ ��ǥ ���
        GameObject tileObj = Instantiate(tilePrefab, tilePosition, Quaternion.identity, this.transform); // ����!
        tileObj.transform.localScale = new Vector3(_coordOffset, _coordOffset, _coordOffset);
        //Maptile ���� �� �ʱ�ȭ
        Maptile tile = new Maptile(tileType, position);
        _map[position.X, position.Y] = tile;
    }
    
    /// <summary>
    /// Coord ��ǥ�� Vector3 ��ǥ�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="x">�� ��ǥ�� X��</param>
    /// <param name="y">�� ��ǥ�� Y��</param>
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
    /// Vector3 ��ǥ�� �޾�����, Coord X, Y ��ǥ�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="worldPosition">���� World������ Position</param>
    /// <returns></returns>
    public (int, int) Vector3ToCoord (Vector3 worldPosition)
    {
        float X = Mathf.Round(worldPosition.x / _coordOffset);
        float Y = Mathf.Round(worldPosition.z / _coordOffset);
        return ((int)X, (int)Y);
    }

    
}
