using UnityEngine;
using TMPro;

public class PreviewSummoner : MonoBehaviour
{
    [Header("의존성 주입")]
    [SerializeField] private Map _map;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private DirectionSelectUI _directionUI;

    [Header("유닛 정보 UI")]
    [SerializeField] private GameObject unitInfoPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;

    private PlayerUnitData _currentUnit;
    private GameObject _preview;
    private bool _cursorOnMap;

    public void StartPreview(PlayerUnitData data)
    {
        CancelPreview();
        _currentUnit = data;
        _preview = Instantiate(data.UnitTposePrefab);
        _preview.SetActive(false);
        ShowInfo(data);
    }

    public void CancelPreview()
    {
        if (_preview)
        {
            Destroy(_preview);
        }

        _preview = null;
        HideInfo();
    }

    void Update()
    {
        if (_preview == null || _directionUI.IsActive)
        {
            return;
        }
        UpdatePreviewPosition();
        HandleMouseClick();
    }

    bool IsSameTileType(Position pos)
    {
        return _currentUnit.TileType == _map.GetTile(pos).TileType;
    }

    private void UpdatePreviewPosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Maptile tile = hit.collider.GetComponent<Maptile>();
            if (tile != null)
            {
                Position pos = tile.GetPosition();
                if (_map.IsInsideMap(pos) && IsSameTileType(pos))
                {
                    _preview.transform.position = _map.CoordToVector3(pos);
                    if (!_cursorOnMap)
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
        if (Input.GetMouseButtonUp(0) && _cursorOnMap)
        {
            TryLockPreview();
        }
    }
    private void TryLockPreview()
    {
        Position pos = _map.Vector3ToCoord(_preview.transform.position);
        if (!_map.IsInsideMap(pos))
        {
            CancelPreview();
            return;
        }
        _directionUI.OpenDirectionUI(_preview, _currentUnit, this); // 의존성 주입
    }
    private void ShowInfo(PlayerUnitData data)
    {
        nameText.text = data.name;
        descText.text = $"HP {data.Hp}\nDef {data.Def}\nAtk {data.Atk}\nResist {data.ResistCapacity}";
        unitInfoPanel.SetActive(true);
    }
    private void HideInfo()
    {
        unitInfoPanel.SetActive(false);
    }
}