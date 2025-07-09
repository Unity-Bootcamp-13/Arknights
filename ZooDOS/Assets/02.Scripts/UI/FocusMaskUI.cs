using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIFocusMask : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Image _maskImage;            // 쉐이더가 적용된 이미지
    [SerializeField] private Camera _mainCamera;          // WorldToScreen 변환용
    
    [Header("Settings")]
    [SerializeField] private float _holeRadius = 0.1f;
    
    private Material _runtimeMaterial;
   

    private void Awake()
    {
        // 공유 마테리얼을 건드리지 않도록 인스턴스 복사
        _runtimeMaterial = Instantiate(_maskImage.material);
        _maskImage.material = _runtimeMaterial;

        // 처음에는 꺼두기
        gameObject.SetActive(false);
    }

    public void Show(Vector3 worldPos)
    {
        gameObject.SetActive(true);

        Vector3 screenPos = _mainCamera.WorldToScreenPoint(worldPos + Vector3.up * 1.2f); // 포커스 중심을 키만큼 위로 올림

        float aspect = (float)Screen.height / Screen.width;
        float uvX = screenPos.x / Screen.width;
        float uvY = screenPos.y / Screen.height;
        uvX = (uvX - 0.5f) * aspect + 0.5f;
        uvY = (uvY - 0.5f) * aspect + 0.5f; // 비율 보정값. (셰이더에서 보정하는중)
        _runtimeMaterial.SetVector("_HoleCenter", new Vector2(uvX, uvY));
        _runtimeMaterial.SetFloat("_HoleRadius", _holeRadius);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetRadius(float radius)
    {
        _holeRadius = radius;
        _runtimeMaterial?.SetFloat("_HoleRadius", _holeRadius);
    }
}
