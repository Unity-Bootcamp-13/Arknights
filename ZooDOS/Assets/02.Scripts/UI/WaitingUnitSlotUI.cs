using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class WaitingUnitSlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private PlayerUnitData _playerUnitData;
    
    private bool _isHolding = false;
    private float _holdTimer = 0f;
    private float _requiredHoldTime = 0.1f; // 0.1초 이상 누르면 발동

    private bool _isDraggingPreview = false;
    private GameObject _previewInstance = null;

    private bool isSelectingDirection = false;
    
    private Map _map;                                //의존성 주입
    private PlayerUnitSpawner _playerUnitSpawner;    //의존성 주입


    private void Update()
    {
        if (_isHolding)
        {
            _holdTimer += Time.deltaTime;
            if (_holdTimer >= _requiredHoldTime && !_isDraggingPreview)
            {

                StartDraggingPreview();
            }

            if (_isDraggingPreview && _playerUnitData.UnitTposePrefab != null) //
            {
                UpdatePreviewPosition();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isSelectingDirection)
        {
            _isHolding = true;
            _holdTimer = 0f;
        }
        else
        {
            
        }
       
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        if (_isDraggingPreview)
        {
            LockPreviewPosition();
            //FinalizePlacement();
        }
        ResetState();

    }

    private void LockPreviewPosition()
    {
        
        if (_previewInstance == null)
            return;

        Vector3 LockPreviewPosition = _previewInstance.transform.position;
        Position pos = _map.Vector3ToCoord(LockPreviewPosition);
        _isDraggingPreview = false;
        if (IsValidTileToPlace(pos))
        {
            isSelectingDirection = true;
            SelectingDirection();
        }
        
            
    }

    public void SelectingDirection()
    {
        // ui setactive.true; 트래킹 ui랑 배치취소버튼 ui가 같이떠야됨.
        // 위치를 preview prefab 의 중심으로 맞춰줌 real -> UI 
        // if 상하좌우 트래킹 받기
        // 1. pointerdown일때 초기값 잡고
        // 2. 지금 마우스포인터 위치 기준 프리펩을 회전 (90도 단위)
        // 3. pointerup일때 지금 위치기준 회전값 return.
        // 4. return값을 이용해서 FinalizePlacement() 호출
        // 5. 팝업 ui 없애기
        // else if 배치 취소 버튼을 누르기
        // 1. isSelectingDirection= false
        // 2. Tpose null값
        // 3. 팝업 ui 없애기
    }


    public void Initialize(PlayerUnitData data, PlayerUnitSpawner spawner, Map map)
    {
        _playerUnitData = data;
        _playerUnitSpawner = spawner;
        _map = map;
        SetUnitUI();
    }

    private void StartDraggingPreview()
    {
        _isDraggingPreview = true;
        if (_playerUnitData.UnitTposePrefab == null)
        {
            Debug.LogError("UnitTposePrefab is null in PlayerUnitData!");
            return;
        }
        _previewInstance = Instantiate(_playerUnitData.UnitTposePrefab);
    }

    private void UpdatePreviewPosition()
    {
        if (_previewInstance == null) 
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Position pos = _map.Vector3ToCoord(hitPoint);
            if (_map.IsInsideMap(pos))
            {
                Vector3 snapped = _map.CoordToVector3(pos);
                _previewInstance.transform.position = snapped;
               
            }
        }
    }

    private bool IsValidTileToPlace(Position pos)
    {
        if (_playerUnitData.TileType == TileType.Ground)
        {
            return _map.CanPlaceUnitAtGround(pos);
        }
        else if (_playerUnitData.TileType == TileType.Hill)
        {
            return _map.CanPlaceUnitAtHill(pos);
        }
        else
        {
            return false;
        }
    }

    private void FinalizePlacement()
    {
        if (_previewInstance == null)
            return;

        Vector3 finalPosition = _previewInstance.transform.position;
        Position pos = _map.Vector3ToCoord(finalPosition);
        Destroy(_previewInstance);
        _previewInstance = null;
        //_isDraggingPreview = false;
        if (IsValidTileToPlace(pos))
        {
            _playerUnitSpawner.PlayerUnitSpawn(pos, Vector3.forward, _playerUnitData);
        }
    }

    public PlayerUnitData GetPlayerUnitData()
    {
        return _playerUnitData;
    }
    private void ResetState()
    {
        _isHolding = false;
        _holdTimer = 0f;
    }

    public void SetUnitUI()
    {
        _icon.sprite = _playerUnitData.UnitPortrait;
    }

}