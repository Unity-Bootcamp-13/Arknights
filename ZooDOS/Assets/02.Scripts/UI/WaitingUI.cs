using UnityEngine;
using System.Collections.Generic;

public class WaitingUI : MonoBehaviour
{
    private const float SlotSize = 120f;
    private const float SlotSpacing = 5f;

    [Header("대기석 슬롯")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private RectTransform background;
    [SerializeField] private List<PlayerUnitData> unitDataList;

    [Header("의존성 주입")]
    [SerializeField] private PlayerUnitSpawner _spawner;
    [SerializeField] private PreviewSummoner _previewSummoner;   // 인스펙터 연결

    //private readonly List<WaitingSlotUI> _slots = new();
    private readonly Dictionary<int, WaitingSlotUI> _slotMap = new();
    void Start()
    {
        foreach (var data in unitDataList)
        {
            AddSlot(data);
        }
        RegisterExistingUnits();
    }

    private void HandleUnitDie(int id)
    {
        if (_slotMap.TryGetValue(id, out var slot))
            slot.StartCooldown();
    }
    private void AddSlot(PlayerUnitData data)
    {
        GameObject gameObject = Instantiate(slotPrefab, slotContainer);

        WaitingSlotUI slot = gameObject.GetComponent<WaitingSlotUI>();
        slot.SetupSlot(data, this);

        int index = _slotMap.Count;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(SlotSize, SlotSize);
        rectTransform.anchoredPosition = new Vector2(-(SlotSize + SlotSpacing) * _slotMap.Count, 0);

        _slotMap[data.Id] = slot;
        UpdateBackgroundSize();
    }

    private void UpdateBackgroundSize()
    {
        int count = _slotMap.Count;
        float width = _slotMap.Count * SlotSize + Mathf.Max(0, _slotMap.Count - 1) * SlotSpacing;
        background.sizeDelta = new Vector2(width, SlotSize);
    }

    public void OnSlotClicked(PlayerUnitData data)
    {
        _previewSummoner.StartPreview(data); 
    }


    private void RegisterExistingUnits()
    {
        foreach (var pair in _spawner.PlayerUnits)   // pair.Key = id
        {
            int id = pair.Key;
            PlayerUnit unit = pair.Value;

            if (_slotMap.TryGetValue(id, out var slot))
            {
                /* (1) 슬롯 자체 쿨타임과 연결 */
                unit.Die -= slot.StartCooldownProxy;      // 중복 방지
                unit.Die += slot.StartCooldownProxy;

                /* (2) WaitingUI가 ID만 받도록 람다 캡처 */
                int capturedId = id;                 // foreach 캡처 주의
                unit.Die -= _ => HandleUnitDie(capturedId);
                unit.Die += _ => HandleUnitDie(capturedId);
            }
        }
    }
}
