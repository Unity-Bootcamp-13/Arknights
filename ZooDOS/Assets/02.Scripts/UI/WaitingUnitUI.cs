using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;

public class WaitingUnitUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform background;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private RectTransform _popupUI;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Button _cancelButton;

    [Header("월드 관련")]
    [SerializeField] private Map _map;
    [SerializeField] private PlayerUnitSpawner _playerUnitSpawner;
    [SerializeField] private Camera _mainCamera;

    [Header("유닛 리스트")]
    [SerializeField] private List<PlayerUnitData> unitDataList;

    [Header("유닛 정보 UI")]
    [SerializeField] private GameObject unitInfoPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private List<WaitingUnitSlotUI> slotList = new();
    private PlayerUnitData _currentUnit;
    private GameObject _previewInstance;
    private bool _isSelectingDirection = false;
    private bool _isDirectionInputActive = false;
    private Vector2 _pointerDownPos;

    private const float SlotSize = 120f;
    private const float SlotSpacing = 5f;

    private void Start()
    {
        foreach (var data in unitDataList)
            AddUnitSlot(data);

        _cancelButton.onClick.AddListener(CancelPlacement);
    }

    private void Update()
    {
        if (_previewInstance != null && !_isSelectingDirection)
            UpdatePreviewPosition();

        if (_isSelectingDirection)
            HandleDirectionInput();
    }

    private void AddUnitSlot(PlayerUnitData data)
    {
        GameObject obj = Instantiate(slotPrefab, slotContainer);
        var slot = obj.GetComponent<WaitingUnitSlotUI>();
        slot.Setup(data, this);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(SlotSize, SlotSize);
        rt.anchoredPosition = new Vector2(-(SlotSize + SlotSpacing) * slotList.Count, 0);
        slotList.Add(slot);
        UpdateBackgroundSize();
    }

    public void OnUnitSlotSelected(PlayerUnitData data)
    {
        _currentUnit = data;

        if (_previewInstance != null)
            Destroy(_previewInstance);

        if (data.UnitTposePrefab != null)
            _previewInstance = Instantiate(data.UnitTposePrefab);

        ShowUnitInfoUI(data);
    }
    private void UpdatePreviewPosition()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            Position pos = _map.Vector3ToCoord(hit);

            if (_map.IsInsideMap(pos))
                _previewInstance.transform.position = _map.CoordToVector3(pos);
        }

        if (Input.GetMouseButtonUp(0))
            TryLockPreview();
    }

    private void TryLockPreview()
    {
        Position pos = _map.Vector3ToCoord(_previewInstance.transform.position);
        if (!IsValidTile(pos))
        {
            Destroy(_previewInstance);
            _previewInstance = null;
            return;
        }

        _isSelectingDirection = true;
        _pointerDownPos = Vector2.zero;
        ShowPopupAtWorldPosition(_previewInstance.transform.position);
    }

    private void HandleDirectionInput()
    {
        if (!_isDirectionInputActive && Input.GetMouseButtonDown(0))
        {
            _pointerDownPos = Input.mousePosition;
            _isDirectionInputActive = true;
        }

        if (_isDirectionInputActive && Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - _pointerDownPos;
            if (delta.magnitude >= 10f)
                ApplyDirectionPreview(delta);
        }

        if (_isDirectionInputActive && Input.GetMouseButtonUp(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - _pointerDownPos;
            if (delta.magnitude >= 10f)
                FinalizePlacement(GetSwipeDirection(delta));

            ResetDirectionSelection();
        }
    }

    private void ApplyDirectionPreview(Vector2 delta)
    {
        Vector3 dir = GetSwipeDirection(delta);
        if (dir == Vector3.forward)
            _previewInstance.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (dir == Vector3.back)
            _previewInstance.transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (dir == Vector3.left)
            _previewInstance.transform.rotation = Quaternion.Euler(0, -90, 0);
        else if (dir == Vector3.right)
            _previewInstance.transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    private void FinalizePlacement(Vector3 dir)
    {
        HideUnitInfoUI();

        Position pos = _map.Vector3ToCoord(_previewInstance.transform.position);
        if (IsValidTile(pos))
            _playerUnitSpawner.PlayerUnitSpawn(pos, dir, _currentUnit);

        Destroy(_previewInstance);
        _previewInstance = null;
    }

    private Vector3 GetSwipeDirection(Vector2 delta)
    {
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        angle = (angle + 360) % 360;
        if (angle >= 45 && angle < 135) return Vector3.forward;
        if (angle >= 135 && angle < 225) return Vector3.left;
        if (angle >= 225 && angle < 315) return Vector3.back;
        return Vector3.right;
    }

    private bool IsValidTile(Position pos)
    {
        return _currentUnit.TileType switch
        {
            TileType.Ground => _map.CanPlaceUnitAtGround(pos),
            TileType.Hill => _map.CanPlaceUnitAtHill(pos),
            _ => false,
        };
    }

    public void CancelPlacement()
    {
        if (_previewInstance != null)
        {
            Destroy(_previewInstance);
            _previewInstance = null;
        }
        HideUnitInfoUI();
        ResetDirectionSelection();
    }

    private void ResetDirectionSelection()
    {
        _isSelectingDirection = false;
        _isDirectionInputActive = false;
        _popupUI.gameObject.SetActive(false);
    }

    private void ShowPopupAtWorldPosition(Vector3 worldPos)
    {
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            screenPos,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _mainCamera,
            out Vector2 localPoint
        );
        _popupUI.anchoredPosition = localPoint;
        _popupUI.gameObject.SetActive(true);
    }

    private void UpdateBackgroundSize()
    {
        float totalWidth = slotList.Count * SlotSize + Mathf.Max(0, slotList.Count - 1) * SlotSpacing;
        background.sizeDelta = new Vector2(totalWidth, SlotSize);
    }

    

    private void ShowUnitInfoUI(PlayerUnitData data)
    {
        nameText.text = data.name;
        descriptionText.text = $"HP : {data.Hp}\nDef : {data.Def}\nAtk : {data.Atk}\nResist : {data.ResistCapacity}";
        
        unitInfoPanel.SetActive(true);
    }

    private void HideUnitInfoUI()
    {
        unitInfoPanel.SetActive(false);
    }

}