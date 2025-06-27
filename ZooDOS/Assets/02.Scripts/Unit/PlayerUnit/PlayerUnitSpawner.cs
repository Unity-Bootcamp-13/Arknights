using Unity.VisualScripting;
using UnityEngine;

public class PlayerUnitSpawner : MonoBehaviour
{
    [SerializeField] Map _map;


    /// <summary>
    /// UI에서 적절한 배치 단계를 거치고 호출
    /// </summary>
    /// <param name="x"> 배치 좌표 x 인덱스 </param>
    /// <param name="y"> 배치 좌표 y 인덱스 </param>
    /// <param name="playerUnitData"> 스폰할 유닛 데이터 </param>
    public void PlayerUnitSpawn(Position position, Vector3 direction , PlayerUnitData playerUnitData)
    {
        Vector3 worldPos = _map.CoordToVector3(position);
        PlayerUnit playerUnit = Instantiate(playerUnitData.UnitPrefab);
        ApplyDirectionVector(direction, playerUnit);
        playerUnit.Init(playerUnitData);
        playerUnit.transform.position = worldPos; 
    }

    public void ApplyDirectionVector(Vector3 direction, PlayerUnit playerUnit)
    {
        if (direction == Vector3.forward)
        {
            playerUnit.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // 회전 없음
        }
        else if (direction == Vector3.left)
        {
            playerUnit.transform.rotation = Quaternion.Euler(0f, -90f, 0f); // 왼쪽 90도
        }
        else if (direction == Vector3.right)
        {
            playerUnit.transform.rotation = Quaternion.Euler(0f, 90f, 0f); // 오른쪽 90도
        }
        else if (direction == Vector3.back)
        {
            playerUnit.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // 뒤쪽 180도
        }
        else
        {
            return;
        }
    }
}
