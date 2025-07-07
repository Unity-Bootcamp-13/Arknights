using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WaitingSlotUI : MonoBehaviour, IPointerDownHandler
{
    private PlayerUnitData _playerUnitData;
    private WaitingUI _waitingUI;
    private CostWallet _wallet;
    [SerializeField] private Image _unitPortrait;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Image _classIcon;

    [Header("Sprite")]
    [SerializeField] private Sprite _iconDefender;
    [SerializeField] private Sprite _iconSniper;
    [SerializeField] private Sprite _iconMedic;

    [SerializeField] private CooltimeUI _cooltimeUI;
    [SerializeField] private Image _notEnoughCostImage;
    
    private bool _isEnoughCost = true;
    private bool _isCooldown = false;
    private void Awake()
    {
        _cooltimeUI.gameObject.SetActive(false);
    }

    public void StartCooldownProxy(Unit _)
    {
        if (!this || !gameObject.activeInHierarchy)
        {
            return;
        }
        StartCooldown();            
    }

    public void StartCooldown()
    {
        _isCooldown = true;
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
    public void SetupSlot(PlayerUnitData data, WaitingUI waitingUI, CostWallet wallet)
    {
        _playerUnitData = data;
        _waitingUI = waitingUI;
        _wallet = wallet;
        _unitPortrait.sprite = data.UnitPortrait;
        _costText.text = data.PlaceCost.ToString();
        _cooltimeUI.CooldownSetting(_playerUnitData);
        GetClassIconSprite();
        _notEnoughCostImage.gameObject.SetActive(false);
        UpdateCostState(_wallet.Current);                
        _wallet.OnCostChanged += UpdateCostState;
        _cooltimeUI.CooldownFinished += OnCooldownFinished;
    }
    private void OnCooldownFinished()
    {
        _isCooldown = false;          
    }
    private void UpdateCostState(Cost cost)
    {
        _isEnoughCost = cost.Value >= _playerUnitData.PlaceCost;

        _notEnoughCostImage.gameObject.SetActive(!_isEnoughCost);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isEnoughCost && !_isCooldown)
        {
            _waitingUI.OnSlotClicked(_playerUnitData);
        }
        else
        {
            _waitingUI.OnInfoOnlyClicked(_playerUnitData);
        }
    }

    void OnDestroy()
    {
        if (_wallet != null)
        {
            _wallet.OnCostChanged -= UpdateCostState;
        }
        _cooltimeUI.CooldownFinished -= OnCooldownFinished;
    }
}