using UnityEngine;
using TMPro;

public class PreviewSummoner : MonoBehaviour
{
    [Header("Deps")]
    [SerializeField] private Map _map;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private DirectionSelectUI _directionUI;   // 인스펙터 연결

    [Header("Unit-Info UI")]
    [SerializeField] private GameObject unitInfoPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;

    private PlayerUnitData _currentUnit;
    private GameObject _preview;
    private bool _cursorOnMap;

    /* ---------- public API ---------- */
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
        if (_preview) Destroy(_preview);
        _preview = null;
        HideInfo();
    }

    /* ---------- MonoBehaviour ---------- */
    void Update()
    {
        if (_preview == null || _directionUI.IsActive) return;
        FollowMouse();
    }

    /* ---------- internals ---------- */
    private void FollowMouse()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!new Plane(Vector3.up, Vector3.zero).Raycast(ray, out float enter)) return;

        Vector3 world = ray.GetPoint(enter);
        Position pos = _map.Vector3ToCoord(world);

        if (_map.IsInsideMap(pos))
        {
            _preview.transform.position = _map.CoordToVector3(pos);
            if (!_cursorOnMap)
            {
                _preview.SetActive(true);
                _cursorOnMap = true;
            }

            if (Input.GetMouseButtonUp(0))
                TryLockPreview();
        }
        else
        {
            if (_cursorOnMap)
            {
                _preview.SetActive(false);
                _cursorOnMap = false;
            }
        }
    }

    private void TryLockPreview()
    {
        Position pos = _map.Vector3ToCoord(_preview.transform.position);
        if (!_map.IsInsideMap(pos)) { CancelPreview(); return; }

        // 방향 선택 UI로 제어 이양
        _directionUI.Open(_preview, _currentUnit, this);
    }

    private void ShowInfo(PlayerUnitData d)
    {
        nameText.text = d.name;
        descText.text = $"HP {d.Hp}\nDef {d.Def}\nAtk {d.Atk}\nResist {d.ResistCapacity}";
        unitInfoPanel.SetActive(true);
    }
    private void HideInfo()
    {
        unitInfoPanel.SetActive(false);
    }
}