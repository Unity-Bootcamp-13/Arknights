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
    [SerializeField] private float _coordOffset = 1.0f; // ���� ��ǥ ���� Ÿ�� ����

    [Header("Ÿ�� ������")]
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

        GameObject tileObj = Instantiate(tilePrefab, tilePosition, Quaternion.identity, this.transform); // ����!
        tileObj.transform.localScale = new Vector3(_coordOffset, _coordOffset, _coordOffset);
        Maptile tile = tileObj.GetComponent<Maptile>(); // ��ũ��Ʈ ��������
        tile.InitPosition(positionX, positionY);         // �ʱ�ȭ
        _map[positionX, positionY] = tile;
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
