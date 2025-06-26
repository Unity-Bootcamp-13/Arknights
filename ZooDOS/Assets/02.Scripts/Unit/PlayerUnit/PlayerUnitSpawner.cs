using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnitSpawner : MonoBehaviour
{
    [SerializeField] Map _map;

    /// <summary>
    /// UI���� ������ ��ġ �ܰ踦 ��ġ�� ȣ��
    /// </summary>
    /// <param name="x"> ��ġ ��ǥ x �ε��� </param>
    /// <param name="y"> ��ġ ��ǥ y �ε��� </param>
    /// <param name="playerUnitData"> ������ ���� ������ </param>
    public void PlayerUnitSpawn(int x, int y , PlayerUnitData playerUnitData)
    {
        Vector3 worldPos = _map.CoordToVector3(x, y);
        PlayerUnit playerUnit = Instantiate(playerUnitData.UnitPrefab);
        playerUnit.Init(playerUnitData);
        playerUnit.transform.position = worldPos; 
    }

   

    
}
