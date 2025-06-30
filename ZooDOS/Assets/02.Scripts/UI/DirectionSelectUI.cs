using UnityEngine;

public class DirectionSelectUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform popupUI;
    [SerializeField] private Canvas canvas;

    [Header("Deps")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Map _map;
    [SerializeField] private GameManager _gm;
    [SerializeField] private PlayerUnitSpawner _spawner;

    public bool IsActive { get; private set; }

    private GameObject _preview;
    private PlayerUnitData _unit;
    private PreviewSummoner _summoner;

    private bool _dragging;
    private Vector2 _start;

    /* ---------- public ---------- */
    public void Open(GameObject preview, PlayerUnitData unit, PreviewSummoner summoner)
    {
        _preview = preview;
        _unit = unit;
        _summoner = summoner;
        IsActive = true;
        _dragging = false;

        ShowPopupAtWorldPos(preview.transform.position);
    }

    /* ---------- MonoBehaviour ---------- */
    void Update()
    {
        if (!IsActive) return;          // 활성화 여부만 먼저 확인

        if (_preview == null)           // Destroy되었거나 참조가 끊겼다면
        {
            CloseSilently();            // 팝업·상태 초기화
            return;
        }

        HandleInput();
    }

    private void CloseSilently()          // 코스트 차감 없이 그냥 종료
    {
        IsActive = false;
        popupUI.gameObject.SetActive(false);
        _preview = null;
        _unit = null;
        _summoner = null;
    }
    /* ---------- internals ---------- */
    private void HandleInput()
    {
        if (!_dragging && Input.GetMouseButtonDown(0))
        {
            _start = Input.mousePosition;
            _dragging = true;
        }

        if (_dragging && Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - _start;
            if (delta.sqrMagnitude >= 100f)
                ApplyDirPreview(delta);
        }

        if (_dragging && Input.GetMouseButtonUp(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - _start;
            if (delta.sqrMagnitude >= 100f)
                Finalize(delta);
            else               // 스와이프 실패 → 다시 입력 대기
                _dragging = false;
        }
    }

    private void ApplyDirPreview(Vector2 delta)
    {
        var dir = SwipeToDir(delta);
        _preview.transform.rotation = Quaternion.LookRotation(dir);
    }

    private void Finalize(Vector2 delta)
    {
        Vector3 dirVector = SwipeToDir(delta);
        Position pos = _map.Vector3ToCoord(_preview.transform.position);

        if (_map.IsInsideMap(pos) && _gm.DeleteCost(_unit.PlaceCost))
            _spawner.PlayerUnitSpawn(pos, dirVector, _unit);

        Close();
    }

    private void Close()
    {
        IsActive = false;
        popupUI.gameObject.SetActive(false);
        _summoner.CancelPreview();   // 프리뷰 제거 & Info UI 숨김
    }

    /* ---------- helpers ---------- */
    private Vector3 SwipeToDir(Vector2 delta)
    {
        float ang = (Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg + 360) % 360;
        if (ang >= 45 && ang < 135) return Vector3.forward;
        if (ang >= 135 && ang < 225) return Vector3.left;
        if (ang >= 225 && ang < 315) return Vector3.back;
        return Vector3.right;
    }

    private void ShowPopupAtWorldPos(Vector3 world)
    {
        Vector3 screen = _mainCamera.WorldToScreenPoint(world);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screen,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _mainCamera,
            out Vector2 local);
        popupUI.anchoredPosition = local;
        popupUI.gameObject.SetActive(true);
    }
}
