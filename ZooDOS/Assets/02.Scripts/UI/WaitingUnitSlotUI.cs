using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class WaitingUnitSlotUI : MonoBehaviour, IPointerDownHandler
{
    private PlayerUnitData _playerUnitData;
    private WaitingUnitUI _parentUI;
    [SerializeField] private TextMeshProUGUI costText;

    public void Setup(PlayerUnitData data, WaitingUnitUI parent)
    {
        _playerUnitData = data;
        _parentUI = parent;
        // 아이콘 등 UI 설정
        costText.text = data.PlaceCost.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _parentUI.OnUnitSlotSelected(_playerUnitData);
    }
}