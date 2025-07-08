using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitSpUI : MonoBehaviour
{
    [SerializeField] private PlayerUnit _unit;
    [SerializeField] private Slider _slider;

    private Vector3 _offset;
    private Camera _mainCamera;


    public void Init(PlayerUnit unit)
    {
        _unit = unit;

        _unit.Sp.SubscribeSpEvent(() => SetSlider());

        _offset = new Vector3(0, 0.6f, -0.5f);
        _mainCamera = Camera.main;

        _slider.value = 0;
    }

    private void SetSlider()
    {
        _slider.value = _unit.Sp.SpRatio;
    }

    /// <summary>
    /// 체력 UI 표시 위치 설정
    /// </summary>
    /// <param name="ui"> 표출할 UI </param>
    /// <param name="position"> UI가 가리키는 유닛의 위치 </param>
    public void SetUIPosition(Vector3 position)
    {
        transform.position = position + _offset;
        transform.forward = -_mainCamera.transform.right;
    }

    public void DisableUI()
    {
        gameObject.SetActive(false);
    }
}
