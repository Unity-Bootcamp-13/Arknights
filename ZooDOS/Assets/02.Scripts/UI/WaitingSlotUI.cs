using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class WaitingSlotUI : MonoBehaviour, IPointerDownHandler
{
    private PlayerUnitData _playerUnitData;
    private WaitingUI _waitingUI;
    [SerializeField] private Image _unitPortrait;
    [SerializeField] private TextMeshProUGUI _costText;

    public void SetupSlot(PlayerUnitData data, WaitingUI waitingUI)
    {
        _playerUnitData = data;
        _waitingUI = waitingUI;
        _unitPortrait.sprite = data.UnitPortrait;
        _costText.text = data.PlaceCost.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _waitingUI.OnSlotClicked(_playerUnitData);
    }
}