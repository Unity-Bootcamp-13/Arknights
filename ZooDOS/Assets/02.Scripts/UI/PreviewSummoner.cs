using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PreviewSummoner : MonoBehaviour
{
    [Header("의존성 주입")]
    [SerializeField] private Map _map;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private DirectionSelectUI _directionUI;
    [SerializeField] private GameSpeedController _gameSpeedController;

    [Header("유닛 정보 UI")]
    [SerializeField] private UnitInfoUI _unitInfoUI;

    private PlayerUnitData _currentUnit;
    private GameObject _preview;
    private bool _cursorOnMap;
    private LayerMask _tileMask; 
    // BlinkSpriteBehaviour 색변환 호출용.
    private readonly List<BlinkSpriteBehaviour> _highlightBlinks = new();
    private readonly List<BlinkSpriteBehaviour> _rangeBlinks = new();

    private bool _isSameTileType = false;

    [Header("Sprite")]
    [SerializeField] private Sprite _iconDefender;
    [SerializeField] private Sprite _iconSniper;
    [SerializeField] private Sprite _iconMedic;

    private void Awake()
    {
        _tileMask = LayerMask.GetMask("Tile");
        _unitInfoUI.Hide();
    }

    public Sprite GetClassIconSprite(PlayerUnitData data)
    {
        Sprite sprite = data.PlayerUnitType switch
        {
            PlayerUnitType.Defender => _iconDefender,
            PlayerUnitType.Sniper => _iconSniper,
            PlayerUnitType.Medic => _iconMedic,
            _ => throw new ArgumentOutOfRangeException(
                    $"아이콘이 등록되지 않은 타입입니다.")
        };
        return sprite;


    }
    public void StartPreview(PlayerUnitData data)
    {
        CancelPreview();
        _currentUnit = data;
        _preview = Instantiate(data.UnitTposePrefab);
        _preview.SetActive(false);
        HighlightPlacableTiles();
        SkillData skillData = data.SkillAttackData[0];
        _unitInfoUI.Show(BuildStatusString(data), data.Name, data.StandingIllust, GetClassIconSprite(data),
                         skillData.SkillIcon, skillData.SkillCost.ToString(), skillData.SkillDescription);
        
    }

    public void ShowInfoOnly(PlayerUnitData data)
    {
        CancelPreview();            
        _currentUnit = data;
        SkillData skillData = data.SkillAttackData[0];
        _unitInfoUI.Show(BuildStatusString(data), data.Name, data.StandingIllust, GetClassIconSprite(data),
                         skillData.SkillIcon, skillData.SkillCost.ToString(), skillData.SkillDescription);
    }
    public void CancelPreview()
    {
        HideAttackRange();
        ClearPlacableHighlights();
        if (_preview)
        {
            Destroy(_preview);
        }

        _gameSpeedController.UpdateTimeScale();
        _preview = null;
        _unitInfoUI.Hide();
    }

    void Update()
    {
        if (_preview == null || _directionUI.IsActive)
        {
            return;
        }

        Time.timeScale = 0.2f;
        UpdatePreviewPosition();
        HandleMouseClick();
    }

    bool IsSameTileType(Position pos)
    {
        return _currentUnit.TileType == _map.GetTile(pos).TileType;
    }

    bool IsPlacableTile(Position pos)
    {
        return (_map.GetTile(pos).GetPlayerUnit() == null);
    }
    
    private void UpdatePreviewPosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _tileMask))
        {
            Maptile tile = hit.collider.GetComponent<Maptile>();
            if (tile != null)
            {
                Position pos = tile.GetPosition();
                _isSameTileType = IsSameTileType(pos);
                
                if (_isSameTileType)
                {
                    _preview.transform.position = _map.CoordToVector3(pos);
                    if (!IsPlacableTile(pos))
                    {
                        _preview.SetActive(false);
                        _cursorOnMap = false;

                    }
                    else if (!_cursorOnMap)
                    {
                        _preview.SetActive(true);
                        _cursorOnMap = true;
                    }
                }
                else
                {
                    _preview.SetActive(false);
                    _cursorOnMap = false;
                }
            }
            else
            {
                _preview.SetActive(false);
                _cursorOnMap = false;
            }
        }
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_isSameTileType)
            {
                TryLockPreview();
            }
            else
            {
                CancelPreview();
            }
        }
    }
    private void TryLockPreview()
    {
        Position pos = _map.Vector3ToCoord(_preview.transform.position);
        if (!pos.IsValid || !_map.IsInsideMap(pos) || !IsPlacableTile(pos))
        {
            CancelPreview();
            return;
        }
        ClearPlacableHighlights();

        _directionUI.OpenDirectionUI(_preview, _currentUnit, this); // 의존성 주입
    }

    
    
    

    private void HighlightPlacableTiles()
    {
        foreach (Maptile tile in _map.GetTilesByType(_currentUnit.TileType))
        {
            BlinkSpriteBehaviour blinkSprite = tile.GetComponentInChildren<BlinkSpriteBehaviour>();
            if (blinkSprite == null)
            {
                continue;
            }
            blinkSprite.StartPlacementBlink();
            _highlightBlinks.Add(blinkSprite);
        }
    }

    public void ShowAttackRange(Position center, Vector3 dir)
    {
        ClearRangeHighlights();

        foreach (Maptile tile in CalcRange(center, dir, _currentUnit))
        {
            var blink = tile.GetComponentInChildren<BlinkSpriteBehaviour>();
            if (blink == null) continue;

            blink.StartAttackRangeBlink();         // 검/회색 루틴
            _rangeBlinks.Add(blink);
        }
    }
    public void HideAttackRange()                  // ← 끄기 전용
    {
        ClearRangeHighlights();
    }

    private void ClearRangeHighlights()
    {
        foreach (var blink in _rangeBlinks)
        {
            if (blink != null)
            {
                blink.StopBlink();
            }
        }
        _rangeBlinks.Clear();
    }

    public List<Maptile> CalcRange(Position center, Vector3 direction, PlayerUnitData playerUnitData)
    {
        List<Position> localRange = new List<Position>();

        for (int y = -playerUnitData.BackwardRange; y <= playerUnitData.ForwardRange; y++)
        {
            for (int x = -playerUnitData.SidewardRange; x <= playerUnitData.SidewardRange; x++)
            {
                localRange.Add(new Position(x, y));
            }
        }

        List<Maptile> result = new List<Maptile>();

        foreach (var offset in localRange)
        {
            Position rotated = RotateOffset(offset, direction);
            Position target = center + rotated;

            if (_map.IsInsideMap(target))
            {
                result.Add(_map.GetTile(target));
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

    private void ClearPlacableHighlights()
    {
        foreach (BlinkSpriteBehaviour blinkTile in _highlightBlinks)
        {
            blinkTile?.StopBlink();
        }
        _highlightBlinks.Clear();
    }

    public bool _previewSummonerIsNull()
    {
        return _preview == null;
    }

    private string BuildStatusString(PlayerUnitData data)
      => $"코스트 : {data.PlaceCost}\n공격력 : {data.Atk}\n방어력 : {data.Def}\n저지 : {data.ResistCapacity} 명";
}