using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WaitingUnitSlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _sprite; //추후 데이터에서 받아오기 
    [SerializeField] private GameObject _previewUnitPrefab; // 임시로 드래그할 프리팹
    [SerializeField] private GameObject _unitPrefab; // 실제 유닛
    private GameObject _previewInstance; // 현재 프리뷰 오브젝트
    private bool _isHolding = false;
    private float _holdTimer = 0f;
    private float _requiredHoldTime = 0.1f; // 0.5초 이상 누르면 발동

    private bool _isDraggingPreview = false;
    private float _maptileLength = 2.0f;
    private void Update()
    {
        if (_isHolding)
        {
            _holdTimer += Time.deltaTime;
            if (_holdTimer >= _requiredHoldTime && !_isDraggingPreview)
            {

                StartDraggingPreview();
            }

            if (_isDraggingPreview && _previewInstance != null)
            {
                UpdatePreviewPosition();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isHolding = true;
        _holdTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isDraggingPreview)
        {
            FinalizePlacement();
        }
        ResetState();
    }

    private void StartDraggingPreview()
    {
        _isDraggingPreview = true;
        _previewInstance = Instantiate(_previewUnitPrefab);
    }

    private void UpdatePreviewPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            float X = Mathf.Round(hitPoint.x / _maptileLength);
            float Y = Mathf.Round(hitPoint.z / _maptileLength);
            Vector3 snapped = new Vector3(X * _maptileLength, 0, Y * _maptileLength);

            _previewInstance.transform.position = snapped;
        }
    }

    private void FinalizePlacement()
    {
        if (_previewInstance == null) return;

        Vector3 finalPosition = _previewInstance.transform.position;
        Destroy(_previewInstance);
        _previewInstance = null;
        _isDraggingPreview = false;

        Instantiate(_unitPrefab, finalPosition, Quaternion.identity);
    }


    private void ResetState()
    {
        _isHolding = false;
        _holdTimer = 0f;
    }

    public void SetUnitUI()
    {
        _icon.sprite = _sprite;
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        gameObject.SetActive(false);
    }
}