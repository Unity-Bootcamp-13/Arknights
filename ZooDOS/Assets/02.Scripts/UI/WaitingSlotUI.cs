using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WaitingSlotUI : MonoBehaviour, IPointerDownHandler
{
    private PlayerUnitData _playerUnitData;
    private WaitingUI _waitingUI;
    [SerializeField] private Image _unitPortrait;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Image _classIcon;

    [Header("Sprite")]
    [SerializeField] private Sprite _iconDefender;
    [SerializeField] private Sprite _iconSniper;
    [SerializeField] private Sprite _iconMedic;

    [SerializeField] private CooltimeUI _cooltimeUI;

    private void Awake()
    {
        _cooltimeUI.gameObject.SetActive(false);
    }

    public void StartCooldownProxy(Unit _)
    {
        StartCooldown();            
    }

    public void StartCooldown()
    {
        _cooltimeUI.gameObject.SetActive(true);  
        _cooltimeUI.SetCooldown(_playerUnitData.ReplaceTime);

    }

    public void GetClassIconSprite()
    {
        _classIcon.sprite = _playerUnitData.PlayerUnitType switch
        {
            PlayerUnitType.Defender => _iconDefender,
            PlayerUnitType.Sniper => _iconSniper,
            PlayerUnitType.Medic => _iconMedic,
            _ => throw new ArgumentOutOfRangeException(
                    $"아이콘이 등록되지 않은 타입입니다.")
        };
    }
    public void SetupSlot(PlayerUnitData data, WaitingUI waitingUI)
    {
        _playerUnitData = data;
        _waitingUI = waitingUI;
        _unitPortrait.sprite = data.UnitPortrait;
        _costText.text = data.PlaceCost.ToString();
        _cooltimeUI.CooldownSetting(_playerUnitData);
        GetClassIconSprite();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _waitingUI.OnSlotClicked(_playerUnitData);
    }
}