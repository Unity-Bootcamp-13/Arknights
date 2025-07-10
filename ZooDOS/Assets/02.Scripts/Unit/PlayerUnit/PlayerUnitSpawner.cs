using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitSpawner : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] UnitHpSpUIManager _unitHpSpUIManager;
    [SerializeField] EffectManager _effectManager;
    [SerializeField] PlayerUnitData[] _playerUnitDatas;
    [SerializeField] AudioManager _audioManager;

    Dictionary<int, PlayerUnit> _units;

    public Dictionary<int, PlayerUnit> PlayerUnits => _units;

    private void Awake()
    {
        _units = new Dictionary<int, PlayerUnit>(_playerUnitDatas.Length);
        for(int i = 0; i <_playerUnitDatas.Length; i++)
        {
            PlayerUnit playerUnit = Instantiate(_playerUnitDatas[i].UnitPrefab);
            playerUnit.Init(_playerUnitDatas[i]);
            playerUnit.SetSFXSound(_audioManager);
            playerUnit.PlayHitEffect += _effectManager.PlayHitEffect;
            playerUnit.PlayHealEffect += _effectManager.PlayHealEffect;
            playerUnit.GetSkillEffect += _effectManager.GetPlayerSkillEffect;
            _units.Add(_playerUnitDatas[i].Id, playerUnit);
        }
    }


    /// <summary>
    /// UI에서 적절한 배치 단계를 거치고 호출
    /// </summary>
    /// <param name="x"> 배치 좌표 x 인덱스 </param>
    /// <param name="y"> 배치 좌표 y 인덱스 </param>
    /// <param name="playerUnitData"> 스폰할 유닛 데이터 </param>
    public void PlayerUnitSpawn(Position position, Vector3 direction , PlayerUnitData playerUnitData)
    {
        Vector3 worldPos = _gameManager.Map.CoordToVector3(position);
        PlayerUnit playerUnit = _units[playerUnitData.Id];
        playerUnit.gameObject.SetActive(true);
        RotateUnitByDirection(playerUnit, direction);
        List<Maptile> Range = CalcRange(position, direction, playerUnitData);
        playerUnit.transform.position = worldPos;

        UnitHpUI hpUI = _unitHpSpUIManager.GetPlayerUnitHpUI();
        playerUnit.SetHpUI(hpUI);
        hpUI.Init(playerUnit);

        PlayerUnitSpUI spUI = _unitHpSpUIManager.GetPlayerUnitSpUI();
        playerUnit.SetSpUI(spUI);
        spUI.Init(playerUnit);

        playerUnit.OnPlace(Range, _gameManager.Map.GetTile(position));

        
    }

    public void RotateUnitByDirection(PlayerUnit playerUnit, Vector3 direction)
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

    public List<Maptile> CalcRange(Position center, Vector3 direction, PlayerUnitData playerUnitData)
    {
        List<Position> localRange = new List<Position>();

        for (int y = - playerUnitData.BackwardRange; y<=playerUnitData.ForwardRange; y++)
        {
            for (int x =-playerUnitData.SidewardRange; x<= playerUnitData.SidewardRange; x++)
            {
                localRange.Add(new Position(x, y));
            }
        }

        List<Maptile> result = new List<Maptile>();

        foreach (var offset in localRange)
        {
            Position rotated = RotateOffset(offset, direction);
            Position target = center + rotated;

            // 맵 범위 체크
            if (IsInsideMap(target))
            {
                result.Add(_gameManager.Map.GetTile(target));
            }
        }

        return result;
    }


    private Position RotateOffset(Position offset, Vector3 direction)
    {
        // forward 기준 (0도)
        if (direction == Vector3.forward)
            return offset;

        // left 기준 (좌측 90도 회전): (x, y) → (-y, x)
        if (direction == Vector3.left)
            return new Position(-offset.Y, offset.X);

        // right 기준 (우측 90도 회전): (x, y) → (y, -x)
        if (direction == Vector3.right)
            return new Position(offset.Y, -offset.X);

        // back 기준 (180도 회전): (x, y) → (-x, -y)
        if (direction == Vector3.back)
            return new Position(-offset.X, -offset.Y);

        // 예외 처리
        return offset;
    }


    private bool IsInsideMap(Position pos)
    {
        return pos.X >= 0 && pos.X < _gameManager.Map.GetTileXSize() &&
               pos.Y >= 0 && pos.Y < _gameManager.Map.GetTileYSize();
    }

}
