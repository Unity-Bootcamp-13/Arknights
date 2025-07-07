using UnityEngine;

public class DirectionSelectUI : MonoBehaviour
{
    [Header("방향 선택 UI")]
    [SerializeField] private RectTransform _popupUI;
    [SerializeField] private Canvas _canvas;

    [Header("UI – 방향 인디케이터")]
    [SerializeField] private RectTransform _dirIndicator;

    [Header("의존성 주입")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Map _map;
    [SerializeField] private CostWallet _costWallet;
    [SerializeField] private PlayerUnitSpawner _playerUnitSpanwer;

    public bool IsActive { get; private set; }

    private GameObject _preview;
    private PlayerUnitData _playerUnitData;
    private PreviewSummoner _summoner;
    private bool _isDragging;
    private Vector2 _startMousePos;
    private Vector3 _curDirection = Vector3.forward;

    private Vector2 _indicatorBasePos;                      // 중앙 좌표 캐시
    private const float INDICATOR_OFFSET = 30f;

    public void OpenDirectionUI(GameObject preview, PlayerUnitData data, PreviewSummoner summoner)
    {
        _preview = preview;
        _playerUnitData = data;
        _summoner = summoner;
        IsActive = true;
        _isDragging = false;

        ShowPopupAtWorldPos(preview.transform.position);

        _indicatorBasePos = _dirIndicator.anchoredPosition = Vector2.zero;
        _dirIndicator.gameObject.SetActive(true);
    }

    void Update()
    {
        if (!IsActive)
        {
            return;          
        }
        if (_preview == null)           
        {
            ResetUnitSpawn();            
            return;
        }
        HandleInput();
    }

    private void ResetUnitSpawn()          // 코스트 차감 없이 그냥 종료
    {
        IsActive = false;
        _popupUI.gameObject.SetActive(false);
        _summoner.CancelPreview();
        _preview = null;
        _playerUnitData = null;
        _summoner = null;
    }

    private void HandleInput()
    {
        if (!_isDragging && Input.GetMouseButtonDown(0))
        {
            _startMousePos = Input.mousePosition;
            _isDragging = true;
        }

        if (_isDragging && Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - _startMousePos;
            if (delta.sqrMagnitude >= 50f)
            {
                ApplyPreviewDirection(delta);
            }
        }

        if (_isDragging && Input.GetMouseButtonUp(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - _startMousePos;
            if (delta.sqrMagnitude >= 50f)
            {
                FinalizePlacement(delta);
            }
            else
            {
                _isDragging = false;
            }
        }
    }

    private void ApplyPreviewDirection(Vector2 delta)
    {

        var dir = GetSwipeDirection(delta);

        if (dir == _curDirection) return;    // 방향이 바뀌지 않았으면 무시
        _curDirection = dir;

        _preview.transform.rotation = Quaternion.LookRotation(dir);

        UpdateDirIndicator(dir);

        Position pos = _map.Vector3ToCoord(_preview.transform.position);
        _summoner.ShowAttackRange(pos, dir); //사거리 하이라이트 갱신
    }

    private void UpdateDirIndicator(Vector3 dir)
    {
        Vector2 offset = Vector2.zero;
        if (dir == Vector3.forward) offset = Vector2.up;
        else if (dir == Vector3.back) offset = Vector2.down;
        else if (dir == Vector3.left) offset = Vector2.left;
        else if (dir == Vector3.right) offset = Vector2.right;

        Vector3 localDir = Quaternion.Inverse(_popupUI.rotation) * offset;

        _dirIndicator.anchoredPosition = _indicatorBasePos +
                                         (Vector2)(localDir * INDICATOR_OFFSET);
    }

    private void FinalizePlacement(Vector2 delta)
    {
        Vector3 dirVector = GetSwipeDirection(delta);
        Position pos = _map.Vector3ToCoord(_preview.transform.position);

        if (_map.IsInsideMap(pos) && _costWallet.TrySpendCost(new Cost(_playerUnitData.PlaceCost)))
        {
            _playerUnitSpanwer.PlayerUnitSpawn(pos, dirVector, _playerUnitData);
        }
        CloseDirectionPopup();
    }

    public void CloseDirectionPopup()
    {
        IsActive = false;
        _popupUI.gameObject.SetActive(false);
        _dirIndicator.gameObject.SetActive(false);
        _summoner.CancelPreview();   
    }

    private Vector3 GetSwipeDirection(Vector2 delta)
    {
        float ang = (Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg + 360) % 360;
        if (ang >= 45 && ang < 135)
        {
            return Vector3.forward;
        }
        else if (ang >= 135 && ang < 225)
        {
            return Vector3.left;
        }
        else if (ang >= 225 && ang < 315)
        {
            return Vector3.back;
        }
        else
        {
            return Vector3.right;
        }
    }

    private void ShowPopupAtWorldPos(Vector3 world)
    {
        Vector3 screen = _mainCamera.WorldToScreenPoint(world);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.transform as RectTransform,
            screen,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _mainCamera,
            out Vector2 local);
        _popupUI.anchoredPosition = local;
        _popupUI.gameObject.SetActive(true);
    }
}
