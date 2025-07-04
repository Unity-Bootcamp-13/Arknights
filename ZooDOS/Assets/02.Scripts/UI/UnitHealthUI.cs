using UnityEngine;
using UnityEngine.UI;

public class UnitHealthUI : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private Slider _slider;


    private void Start()
    {
        _unit.HpEventSubscribe(()=>SetSlider());
        SetSlider();
    }

    private void SetSlider()
    {
        _slider.value = _unit.Hp.HpRatio;
    }

}
