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
    public void PlayerUnitSpawn(int x, int y , PlayerUnitData playerUnitData)
    {
        Vector3 worldPos = _map.CoordToVector3(x, y);
        PlayerUnit playerUnit = Instantiate(playerUnitData.UnitPrefab);
        playerUnit.Init(playerUnitData);
        playerUnit.transform.position = worldPos; 
    }

   

    
}
